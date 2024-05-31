import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddPlantPopupComponent } from './add-plant-popup.component';

describe('AddPlantPopupComponent', () => {
  let component: AddPlantPopupComponent;
  let fixture: ComponentFixture<AddPlantPopupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddPlantPopupComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddPlantPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
