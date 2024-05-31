import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditPlantPopupComponent } from './edit-plant-popup.component';

describe('EditPlantPopupComponent', () => {
  let component: EditPlantPopupComponent;
  let fixture: ComponentFixture<EditPlantPopupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditPlantPopupComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EditPlantPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
