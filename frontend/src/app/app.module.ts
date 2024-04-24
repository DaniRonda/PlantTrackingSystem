import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http'; // Importa HttpClientModule
import { FormsModule } from "@angular/forms";
import { AppComponent } from './app.component';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    HttpClientModule, // Agrega HttpClientModule
    FormsModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
