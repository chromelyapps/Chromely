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
- (void)createApplication:(id)object;
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

// Create the application on the UI thread.
- (void)createApplication:(id)object {

  // Set the delegate for application events.
  [NSApp setDelegate:self];

  [NSApp setActivationPolicy:NSApplicationActivationPolicyRegular];

  // Create the main window.

  // The CEF framework library is loaded at runtime so we need to use this
  // mechanism for retrieving the class.
  Class window_class = NSClassFromString(@"UnderlayOpenGLHostingWindow");

  NSUInteger customStyleMask = [self windowCustomStyle];
  window = [[window_class alloc]
               initWithContentRect: NSMakeRect(chromelyParam.x, chromelyParam.y, chromelyParam.width, chromelyParam.height)
               styleMask:customStyleMask
               backing:NSBackingStoreBuffered
               defer:NO ];

    NSString *title = [NSString stringWithFormat:@"%s", chromelyParam.title];
	[window setTitle:title];
    [window setAcceptsMouseMovedEvents:YES];
	[window setDelegate:self];
	
    // No dark mode, please
    window.appearance = [NSAppearance appearanceNamed:NSAppearanceNameAqua];

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
	
	// Rely on the window delegate to clean us up rather than immediately
	// releasing when the window gets closed. We use the delegate to do
	// everything from the autorelease pool so the window isn't on the stack
	// during cleanup (ie, a window close from javascript).
	[window setReleasedWhenClosed:NO];
	
    [NSApp activateIgnoringOtherApps:YES];
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
        NSObject* appDelegate = [[ChromelyAppDelegate alloc] init];
        [appDelegate setParams:*pParam];
        [appDelegate performSelectorOnMainThread:@selector(createApplication:)
                               withObject:nil
                               waitUntilDone:NO];

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


    // Create the application delegate.
    NSObject* appDelegate = [[[ChromelyAppDelegate alloc] init] autorelease];
    NSLog(@"Created: appDelegate appDelegate:%@", appDelegate);

    [appDelegate setParams:*pParam];
    NSLog(@"appDelegate setParams set");

    [appDelegate performSelectorOnMainThread:@selector(createApplication:)
                            withObject:nil
                            waitUntilDone:NO];
    NSLog(@"appDelegate performSelectorOnMainThread");

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

