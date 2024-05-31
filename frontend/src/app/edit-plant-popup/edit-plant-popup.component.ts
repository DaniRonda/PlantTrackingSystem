import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-edit-plant-popup',
  templateUrl: './edit-plant-popup.component.html',
  styleUrls: ['./edit-plant-popup.component.css']
})
export class EditPlantPopupComponent implements OnInit {
  @Input() showPopup: boolean = false;
  @Input() plant: any = null;
  @Output() closePopup = new EventEmitter<void>();
  @Output() updatePlant = new EventEmitter<any>();

  editPlantForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.editPlantForm = this.fb.group({
      plantName: ['', Validators.required],
      plantType: ['', Validators.required],
      location: ['', Validators.required]
    });
  }

  ngOnInit() {
    if (this.plant) {
      this.editPlantForm.patchValue({
        plantName: this.plant.plantName,
        plantType: this.plant.plantType,
        location: this.plant.location
      });
    }
  }

  onClose(): void {
    this.closePopup.emit();
  }

  onSubmit(): void {
    if (this.editPlantForm.valid) {
      const updatedData = this.editPlantForm.value;
      this.updatePlant.emit({ id: this.plant.id, data: updatedData });
      this.onClose();
    }
  }
}
