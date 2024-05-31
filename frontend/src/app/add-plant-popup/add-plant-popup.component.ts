import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-add-plant-popup',
  templateUrl: './add-plant-popup.component.html',
  styleUrls: ['./add-plant-popup.component.css']
})
export class AddPlantPopupComponent {
  @Input() showPopup: boolean = false;
  @Output() closePopup = new EventEmitter<void>();

  createPlantForm: FormGroup; // Definición del formulario

  constructor(
    private formBuilder: FormBuilder,
    private http: HttpClient
  ) {
    // Inicialización del formulario en el constructor
    this.createPlantForm = this.formBuilder.group({
      plantName: ['', Validators.required],
      plantType: ['', Validators.required],
      location: ['', Validators.required]
    });
  }

  addPlant() {
    // Enviar la solicitud HTTP POST al servidor para crear una nueva planta
    this.http.post<any>('http://localhost:5000/api/plants', this.createPlantForm.value)
      .subscribe(response => {
        console.log('Planta agregada con éxito:', response);
        // Cerrar el popup después de agregar la planta con éxito
        this.onClose();
        // Aquí puedes tomar otras acciones después de agregar la planta, si es necesario
      }, error => {
        console.error('Error al agregar la planta:', error);
        // Manejar errores si la solicitud falla
        // Aquí puedes mostrar un mensaje de error al usuario, si es necesario
      });
  }

  onClose() {
    // Método para cerrar el popup
    this.closePopup.emit();
  }
}
