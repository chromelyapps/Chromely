import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Router } from '@angular/router';

import { AppComponent } from './app.component';
import { AppRoutedComponents, AppRoutes } from './app-routes';
import { DemoComponent } from './demo/demo.component';
import { MessagerouterService } from './demo/messagerouter.service';
import { HttpService } from './demo/http.service';

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
        MessagerouterService,
        HttpService],
  bootstrap: [AppComponent]
})
export class AppModule { }
