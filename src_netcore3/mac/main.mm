/* main.m
 */

// include the Cocoa Frameworks
#import <Cocoa/Cocoa.h>         

// include Chromely custom header
#include "chromely_mac.h"


void initCallback(void *app, void *pool) {
     NSLog(@"Init: testInitCallback app:%@ pool:%@", app, pool);
}

void createCallback(void *window, void *view) {
     NSLog(@"Created: testCallback window:%@ view:%@", window, view);
}

void movingCallback() {
     NSLog(@"Moving:testMovingCallback");
}

void resizeCallback(int width, int height) {
     NSLog(@"testCallback width:%d height:%d", width, height);
}

void quitCallback() {
    NSLog(@"Quit:testQuitCallback");
}


int main(int argc, char * argv[]) {
    CHROMELYPARAM temp;
    temp.initCallback = initCallback;
    temp.createCallback = createCallback;
    temp.movingCallback = movingCallback;
    temp.resizeCallback = resizeCallback;
    temp.exitCallback = quitCallback;

    temp.centerscreen = 1;
    //temp.frameless = 1;
    //temp.fullscreen = 1;
    //temp.noresize = 1;
    //temp.nominbutton = 1;
    //temp.nomaxbutton = 1;

    memcpy(temp.title, "chromely", 8);

    temp.x = 0;
    temp.y = 0;
    temp.width = 800;
    temp.height = 600;

    APPDATA appData = createwindowdata(&temp);
    run(appData.app);
    quit(appData.app, appData.pool);
    return EXIT_SUCCESS;
}
