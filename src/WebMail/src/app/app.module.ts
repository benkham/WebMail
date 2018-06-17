import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';

import { WebMailService } from './webmail.module';
import { AppComponent } from './app.component';
import { ApiClient } from './apiClient.service';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    ReactiveFormsModule
  ],
  providers: [
    ApiClient,
    WebMailService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
