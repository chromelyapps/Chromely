/* 
* chromely_mac.m
*/

// include the Cocoa Frameworks
#import <Cocoa/Cocoa.h>    

// include Chromely custom header
#include "chromely_mac.h"

/*
* ChromelyAppDelegate manages events.
*/

@interface ChromelyAppDelegate : NSObject <NSApplicationDelegate, NSWindowDelegate> {
    NSApplication *application;
    NSWindow * window;
    NSView   *cefParentView;
    CHROMELYPARAM chromelyParam;
}

- (void)setParams:(CHROMELYPARAM)param;
- (NSUInteger)windowCustomStyle;

@end

@implementation ChromelyAppDelegate : NSObject

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
	[window makeKeyAndOrderFront:nil];
	[window setDelegate:self];
	[application activateIgnoringOtherApps:YES];

    cefParentView = [window contentView];
    chromelyParam.createCallback(window, cefParentView);

    if (chromelyParam.centerscreen == 1)
        [window center];

    if (chromelyParam.nominbutton == 1)
        [[window standardWindowButton:NSWindowMiniaturizeButton] setHidden:YES];

    if (chromelyParam.nomaxbutton == 1)
        [[window standardWindowButton:NSWindowZoomButton] setHidden:YES];

	[window.contentView setWantsLayer:YES];
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

- (void)dealloc {
    [cefParentView release];
    [window release];
    [application release];
    [super dealloc];
}

@end

/*
* Exported methods implementation
*/

void createwindow(CHROMELYPARAM* pParam) {
    NSAutoreleasePool * pool = [[NSAutoreleasePool alloc] init];
    NSApp = [NSApplication sharedApplication];

    ChromelyAppDelegate * appDelegate = [[[ChromelyAppDelegate alloc] init] autorelease];
    [appDelegate setParams:*pParam];
    [NSApp setDelegate:appDelegate];

    pParam->initCallback(NSApp, pool);
}

APPDATA createwindowdata(CHROMELYPARAM* pParam) {
    NSAutoreleasePool * pool = [[NSAutoreleasePool alloc] init];
    NSApp = [NSApplication sharedApplication];

    ChromelyAppDelegate * appDelegate = [[[ChromelyAppDelegate alloc] init] autorelease];
    [appDelegate setParams:*pParam];
    [NSApp setDelegate:appDelegate];

    pParam->initCallback(NSApp, pool);

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

