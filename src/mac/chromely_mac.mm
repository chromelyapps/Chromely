/* 
* chromely_mac.m
*/

// include the Cocoa Frameworks
#import <Cocoa/Cocoa.h>    
#import <objc/runtime.h>
#include "cef_application_mac.h"

// include Chromely custom header
#include "chromely_mac.h"

/*
* I1 - ChromelyApplication manages events.
* Provide the CefAppProtocol implementation required by CEF.
*/
@interface ChromelyApplication : NSApplication <CefAppProtocol> {
 @private
  BOOL handlingSendEvent_;
}
@end


// I2 - Receives notifications from controls and the browser window. Will delete
// itself when done.
@interface RootWindowDelegate : NSObject <NSWindowDelegate> {
 @private
  NSWindow* window_;
  CHROMELYPARAM chromelyParam_;
  bool force_close_;
}

@property(nonatomic, readwrite) bool force_close;

- (id)initWithWindow:(NSWindow*)window
       andParams:(CHROMELYPARAM)param;
@end  // @interface RootWindowDelegate


/*
* I3 - ChromelyAppDelegate manages events.
*/
@interface ChromelyAppDelegate : NSObject <NSApplicationDelegate>  {
    NSWindow * window_;
    RootWindowDelegate* window_delegate_;
    NSView   *cefParentView_;
    CHROMELYPARAM chromelyParam_;
}

- (void)setParams:(CHROMELYPARAM)param;
- (void)createApplication:(id)object;
- (NSUInteger)windowCustomStyle;

- (void)tryToTerminateApplication:(NSApplication*)app;
@end

/*:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::*/

@implementation RootWindowDelegate

@synthesize force_close = force_close_;

- (id)initWithWindow:(NSWindow*)window
        andParams:(CHROMELYPARAM)param {
  if (self = [super init]) {
    window_ = window;
    chromelyParam_ = param;
    [window_ setDelegate:self];
    force_close_ = false;

    // Register for application hide/unhide notifications.
    [[NSNotificationCenter defaultCenter]
        addObserver:self
           selector:@selector(applicationDidHide:)
               name:NSApplicationDidHideNotification
             object:nil];
    [[NSNotificationCenter defaultCenter]
        addObserver:self
           selector:@selector(applicationDidUnhide:)
               name:NSApplicationDidUnhideNotification
             object:nil];
  }
  return self;
}

- (void)dealloc {
  [[NSNotificationCenter defaultCenter] removeObserver:self];
#if !__has_feature(objc_arc)
  [super dealloc];
#endif  // !__has_feature(objc_arc)
}

// Called when we are activated (when we gain focus).
- (void)windowDidBecomeKey:(NSNotification*)notification {
    /// CHROMELYPARAM- function pointer?
}

// Called when we are deactivated (when we lose focus).
- (void)windowDidResignKey:(NSNotification*)notification {
    /// CHROMELYPARAM- function pointer?
}

// Called when we have been minimized.
- (void)windowDidMiniaturize:(NSNotification*)notification {
    /// CHROMELYPARAM- function pointer?
}

// Called when we have been unminimized.
- (void)windowDidDeminiaturize:(NSNotification*)notification {
    /// CHROMELYPARAM- function pointer?
}

// Called when the application has been hidden.
- (void)applicationDidHide:(NSNotification*)notification {
  // If the window is miniaturized then nothing has really changed.
  if (![window_ isMiniaturized]) {
    /// CHROMELYPARAM- function pointer?
  }
}

// Called when the application has been unhidden.
- (void)applicationDidUnhide:(NSNotification*)notification {
  // If the window is miniaturized then nothing has really changed.
  if (![window_ isMiniaturized]) {
    /// CHROMELYPARAM- function pointer?
  }
}

// Called when the window is about to close. Perform the self-destruction
// sequence by getting rid of the window. By returning YES, we allow the window
// to be removed from the screen.
- (BOOL)windowShouldClose:(NSWindow*)window {
  if (!force_close_) {
        /// CHROMELYPARAM- function pointer?
    }

  // Don't want any more delegate callbacks after we destroy ourselves.
  window_.delegate = nil;
  // Delete the window.
#if !__has_feature(objc_arc)
  [window autorelease];
#endif  // !__has_feature(objc_arc)
  window_ = nil;

  // Clean ourselves up after clearing the stack of anything that might have the
  // window on it.
  [self cleanup];

  // Allow the close.
  return YES;
}

// Deletes itself.
- (void)cleanup {
  // Don't want any more delegate callbacks after we destroy ourselves.
  window_.delegate = nil;
  window_.contentView = [[NSView alloc] initWithFrame:NSZeroRect];
  // Delete the window.
#if !__has_feature(objc_arc)
  [window_ autorelease];
#endif  // !__has_feature(objc_arc)
  window_ = nil;
 /// root_window_->OnNativeWindowClosed(); -     /// CHROMELYPARAM- function pointer?

  chromelyParam_.exitCallback();
}

@end  // @implementation RootWindowDelegate

/*:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::*/


/*
* ChromelyApplication manages events.
* Provide the CefAppProtocol implementation required by CEF.
*/

@implementation ChromelyApplication
- (BOOL)isHandlingSendEvent {
  return handlingSendEvent_;
}

- (void)setHandlingSendEvent:(BOOL)handlingSendEvent {
  handlingSendEvent_ = handlingSendEvent;
}

- (void)sendEvent:(NSEvent*)event {
  CefScopedSendingEvent sendingEventScoper;
  [super sendEvent:event];
}

// |-terminate:| is the entry point for orderly "quit" operations in Cocoa. This
// includes the application menu's quit menu item and keyboard equivalent, the
// application's dock icon menu's quit menu item, "quit" (not "force quit") in
// the Activity Monitor, and quits triggered by user logout and system restart
// and shutdown.
//
// The default |-terminate:| implementation ends the process by calling exit(),
// and thus never leaves the main run loop. This is unsuitable for Chromium
// since Chromium depends on leaving the main run loop to perform an orderly
// shutdown. We support the normal |-terminate:| interface by overriding the
// default implementation. Our implementation, which is very specific to the
// needs of Chromium, works by asking the application delegate to terminate
// using its |-tryToTerminateApplication:| method.
//
// |-tryToTerminateApplication:| differs from the standard
// |-applicationShouldTerminate:| in that no special event loop is run in the
// case that immediate termination is not possible (e.g., if dialog boxes
// allowing the user to cancel have to be shown). Instead, this method tries to
// close all browsers by calling CloseBrowser(false) via
// ClientHandler::CloseAllBrowsers. Calling CloseBrowser will result in a call
// to ClientHandler::DoClose and execution of |-performClose:| on the NSWindow.
// DoClose sets a flag that is used to differentiate between new close events
// (e.g., user clicked the window close button) and in-progress close events
// (e.g., user approved the close window dialog). The NSWindowDelegate
// |-windowShouldClose:| method checks this flag and either calls
// CloseBrowser(false) in the case of a new close event or destructs the
// NSWindow in the case of an in-progress close event.
// ClientHandler::OnBeforeClose will be called after the CEF NSView hosted in
// the NSWindow is dealloc'ed.
//
// After the final browser window has closed ClientHandler::OnBeforeClose will
// begin actual tear-down of the application by calling CefQuitMessageLoop.
// This ends the NSApplication event loop and execution then returns to the
// main() function for cleanup before application termination.
//
// The standard |-applicationShouldTerminate:| is not supported, and code paths
// leading to it must be redirected.
- (void)terminate:(id)sender {
  ChromelyAppDelegate* delegate =
      static_cast<ChromelyAppDelegate*>([NSApp delegate]);
  [delegate tryToTerminateApplication:self];
  // Return, don't exit. The application is responsible for exiting on its own.
}
@end


/*
* ChromelyAppDelegate manages events.
*/

@implementation ChromelyAppDelegate 

- (void)setParams:(CHROMELYPARAM)param {
    chromelyParam_ = param;
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
  window_ = [[window_class alloc]
               initWithContentRect: NSMakeRect(chromelyParam_.x, chromelyParam_.y, chromelyParam_.width, chromelyParam_.height)
               styleMask:customStyleMask
               backing:NSBackingStoreBuffered
               defer:NO ];

    NSString *title = [NSString stringWithFormat:@"%s", chromelyParam_.title];
	[window_ setTitle:title];
    [window_ setAcceptsMouseMovedEvents:YES];
	
    // No dark mode, please
    window_.appearance = [NSAppearance appearanceNamed:NSAppearanceNameAqua];

    // Create the delegate for browser window events.
    window_delegate_ = [[RootWindowDelegate alloc] initWithWindow:window_
                                                  andParams:chromelyParam_];

    cefParentView_ = [window_ contentView];
    chromelyParam_.createCallback(window_, cefParentView_);

    if (chromelyParam_.centerscreen == 1)
        [window_ center];

    if (chromelyParam_.nominbutton == 1)
        [[window_ standardWindowButton:NSWindowMiniaturizeButton] setHidden:YES];

    if (chromelyParam_.nomaxbutton == 1)
        [[window_ standardWindowButton:NSWindowZoomButton] setHidden:YES];

	[window_.contentView setWantsLayer:YES];
    [window_ makeKeyAndOrderFront:NSApp];
	
	// Rely on the window delegate to clean us up rather than immediately
	// releasing when the window gets closed. We use the delegate to do
	// everything from the autorelease pool so the window isn't on the stack
	// during cleanup (ie, a window close from javascript).
	[window_ setReleasedWhenClosed:NO];
	
    [NSApp activateIgnoringOtherApps:YES];
}

- (NSUInteger)windowCustomStyle {
    if (chromelyParam_.fullscreen == 1)
        return NSWindowStyleMaskFullScreen;

    if (chromelyParam_.frameless == 1)
        return NSWindowStyleMaskBorderless;

    NSUInteger styleMask = NSTitledWindowMask 
                           | NSClosableWindowMask
                           | NSMiniaturizableWindowMask;

    if (chromelyParam_.noresize == 0)
        styleMask |= NSWindowStyleMaskResizable;

    return styleMask;
}

- (void) windowWillMove:(NSNotification *)notification {
    chromelyParam_.movingCallback();
}

- (void)windowDidResize:(NSNotification *)notification {
    NSRect windowRect = [window_ frame];
    NSRect contentRect = [window_ contentRectForFrameRect:windowRect];
    chromelyParam_.resizeCallback(contentRect.size.width, contentRect.size.height);
} 

- (void)tryToTerminateApplication:(NSApplication*)app {
    chromelyParam_.exitCallback();
}

- (NSApplicationTerminateReply)applicationShouldTerminate:
    (NSApplication*)sender {
  return NSTerminateNow;
}

- (void)dealloc {
    [cefParentView_ release];
    [window_ release];
    [super dealloc];

    chromelyParam_.exitCallback();
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

