/* 
* chromely_mac.m
*/

// include the Cocoa Frameworks
#import <Cocoa/Cocoa.h>    
#import <objc/runtime.h>
#include "cef_application_mac.h"

// include Chromely custom header
#include "chromely_mac.h"

namespace {

// static NSAutoreleasePool* g_autopool = nil;
BOOL g_handling_send_event = false;

}  // namespace

/*
* ChromelyApplication manages events.
* Provide the CefAppProtocol implementation required by CEF.
*/
@interface ChromelyApplication : NSApplication <CefAppProtocol> 
    - (BOOL)isHandlingSendEvent;
    - (void)setHandlingSendEvent:(BOOL)handlingSendEvent;
    - (void)_swizzled_sendEvent:(NSEvent*)event;
    - (void)_swizzled_terminate:(id)sender;

@end

/*
* ChromelyAppDelegate manages events.
*/
@interface ChromelyAppDelegate : NSObject <NSApplicationDelegate, NSWindowDelegate> {
    NSWindow * window;
    NSView   *cefParentView;
    CHROMELYPARAM chromelyParam;
}

- (void)setParams:(CHROMELYPARAM)param;
- (NSUInteger)windowCustomStyle;

- (void)tryToTerminateApplication:(NSApplication*)app;
@end


/*
* ChromelyApplication manages events.
* Provide the CefAppProtocol implementation required by CEF.
*/
@implementation ChromelyApplication

// This selector is called very early during the application initialization.
+ (void)load {
  // Swap NSApplication::sendEvent with _swizzled_sendEvent.
  Method original = class_getInstanceMethod(self, @selector(sendEvent));
  Method swizzled =
      class_getInstanceMethod(self, @selector(_swizzled_sendEvent));
  method_exchangeImplementations(original, swizzled);

  Method originalTerm = class_getInstanceMethod(self, @selector(terminate:));
  Method swizzledTerm =
      class_getInstanceMethod(self, @selector(_swizzled_terminate:));
  method_exchangeImplementations(originalTerm, swizzledTerm);
}

- (BOOL)isHandlingSendEvent {
  return g_handling_send_event;
}

- (void)setHandlingSendEvent:(BOOL)handlingSendEvent {
  g_handling_send_event = handlingSendEvent;
}

- (void)_swizzled_sendEvent:(NSEvent*)event {
  CefScopedSendingEvent sendingEventScoper;
  // Calls NSApplication::sendEvent due to the swizzling.
  [self _swizzled_sendEvent:event];
}

- (void)_swizzled_terminate:(id)sender {
  [self _swizzled_terminate:sender];
}

@end


/*
* ChromelyAppDelegate manages events.
*/

@implementation ChromelyAppDelegate 

- (void)setParams:(CHROMELYPARAM)param {
    chromelyParam = param;
}

- (NSUInteger)windowCustomStyle {
    if (chromelyParam.fullscreen == 1)
        return NSWindowStyleMaskFullScreen;

    if (chromelyParam.frameless == 1)
        return NSWindowStyleMaskBorderless;

    NSUInteger styleMask = NSTitledWindowMask 
                           | NSClosableWindowMask
                           | NSMiniaturizableWindowMask;

    if (chromelyParam.noresize == 0)
        styleMask |= NSWindowStyleMaskResizable;

    return styleMask;
}

- (void)applicationWillFinishLaunching:(NSNotification *)notification {

    // create window programmatically
    NSUInteger customStyleMask = [self windowCustomStyle];
    window = [ [NSWindow alloc]             
               initWithContentRect: NSMakeRect(chromelyParam.x, chromelyParam.y, chromelyParam.width, chromelyParam.height)
               styleMask:customStyleMask
               backing:NSBackingStoreBuffered
               defer:NO ];

    NSString *title = [NSString stringWithFormat:@"%s", chromelyParam.title];
	[window setTitle:title];
    [window setAcceptsMouseMovedEvents:YES];
	[window setDelegate:self];

    cefParentView = [window contentView];
    chromelyParam.createCallback(window, cefParentView);

    if (chromelyParam.centerscreen == 1)
        [window center];

    if (chromelyParam.nominbutton == 1)
        [[window standardWindowButton:NSWindowMiniaturizeButton] setHidden:YES];

    if (chromelyParam.nomaxbutton == 1)
        [[window standardWindowButton:NSWindowZoomButton] setHidden:YES];

	[window.contentView setWantsLayer:YES];
    [window makeKeyAndOrderFront:NSApp];
    [NSApp activateIgnoringOtherApps:YES];
}

- (void) windowWillMove:(NSNotification *)notification {
    chromelyParam.movingCallback();
}

- (void)windowDidResize:(NSNotification *)notification {
    NSRect windowRect = [window frame];
    NSRect contentRect = [window contentRectForFrameRect:windowRect];
    chromelyParam.resizeCallback(contentRect.size.width, contentRect.size.height);
} 

- (void) applicationWillTerminate:(NSNotification *)notification {
    chromelyParam.exitCallback();
}

-(BOOL) applicationShouldTerminateAfterLastWindowClosed:(NSApplication *)notification {
    return YES;
}

- (void)tryToTerminateApplication:(NSApplication*)app {
    chromelyParam.exitCallback();
}

- (NSApplicationTerminateReply)applicationShouldTerminate:
    (NSApplication*)sender {
  return NSTerminateNow;
}

- (void)dealloc {
    [cefParentView release];
    [window release];
    [super dealloc];
}

@end

/*
* Exported methods implementation
*/

void createwindow(CHROMELYPARAM* pParam) {

      @autoreleasepool {
        // Initialize the SimpleApplication instance.
        NSApp = [ChromelyApplication sharedApplication];

        // Create the application delegate.
        ChromelyAppDelegate* appDelegate = [[ChromelyAppDelegate alloc] init];
        [appDelegate setParams:*pParam];
        [NSApp setDelegate:appDelegate];

        // Run the CEF message loop. This will block until CefQuitMessageLoop() is
        // called.
        pParam->runMessageLoopCallback();

        // Shut down CEF.
        pParam->cefShutdownCallback();

        // Release the delegate.
        #if !__has_feature(objc_arc)
        [appDelegate release];
        #endif  // !__has_feature(objc_arc)
        appDelegate = nil;
    }  // @autoreleasepool
}

APPDATA createwindowdata(CHROMELYPARAM* pParam) {
    NSAutoreleasePool * pool = [[NSAutoreleasePool alloc] init];
    NSApp = [ChromelyApplication sharedApplication];

    ChromelyAppDelegate * appDelegate = [[[ChromelyAppDelegate alloc] init] autorelease];
    [appDelegate setParams:*pParam];
    [NSApp setDelegate:appDelegate];

    APPDATA appData;
    appData.app = NSApp;
    appData.pool = pool;

    return appData;
}

void run(void* application) {
    [application run];
}

void quit(void* application, void* pool) {
    [pool release];       
    [application performSelector:@selector(terminate:) withObject:nil afterDelay:0.0];
}

