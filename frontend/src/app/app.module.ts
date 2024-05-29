import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { AddPlantPopupComponent } from './add-plant-popup/add-plant-popup.component';
import {EditPlantPopupComponent} from "./edit-plant-popup/edit-plant-popup.component";

import { SocketIoModule, SocketIoConfig } from 'ngx-socket-io';

const config: SocketIoConfig = { url: 'http://localhost:3000', options: {} };
@NgModule({
  declarations: [
    AppComponent,
    AddPlantPopupComponent, // Declara el componente aquí
    EditPlantPopupComponent
  ],
    imports: [
        BrowserModule,
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule,
      SocketIoModule.forRoot(config)
    ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
