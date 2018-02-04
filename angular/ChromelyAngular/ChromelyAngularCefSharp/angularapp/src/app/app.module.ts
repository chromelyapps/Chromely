import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Router } from '@angular/router';

import { AppComponent } from './app.component';
import { AppRoutedComponents, AppRoutes } from './app-routes';
import { DemoComponent } from './demo/demo.component';
import { HttpService } from './demo/http.service';
import { RegisteredJsObjectService } from './demo/registered-js-object.service';

@NgModule({
  declarations: [
    AppComponent,
    AppRoutedComponents,
    DemoComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    RouterModule.forRoot(AppRoutes)
  ],
  providers: [
        RegisteredJsObjectService,
        HttpService],
  bootstrap: [AppComponent]
})
export class AppModule { }
