import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  plants: any[] = [];
  filteredPlants: any[] = [];
  selectedPlant: any = null;
  selectedPlantId: number | null = null;
  selectedDate: string = '';
  plantData: any[] = [];
  showAddPlantPopup = false;
  showEditPlantPopup = false;
  realtimeData: any = null;
  filterText: string = '';

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.selectedDate = new Date().toISOString().split('T')[0]; // Inicializa con la fecha actual
    this.fetchPlants();
    this.setupWebSocket();
  }

  fetchPlants() {
    this.http.get<any[]>('http://localhost:5000/api/plants')
      .subscribe(
        (data) => {
          console.log('Fetched plants:', data);
          this.plants = data;
          this.filteredPlants = data; // Inicialmente, sin filtros
          this.applyFilter(); // Llama a applyFilter después de establecer los datos
        },
        (error) => {
          console.error('Error fetching plants:', error);
        }
      );
  }


  showDetails(plant: any) {
    this.selectedPlant = plant;
    this.selectedPlantId = plant.id;
    this.realtimeData = null; // Limpiar los datos en tiempo real
    this.fetchDataByDate(this.selectedDate);
  }

  fetchDataByDate(date: string) {
    if (!this.selectedPlantId) {
      console.error('Plant ID is undefined');
      return;
    }

    if (!date) {
      console.error('Date is empty');
      this.plantData = [];
      return;
    }

    const url = `http://localhost:5000/api/DataRecord/plant/${this.selectedPlantId}/date?date=${date}`;
    console.log('Fetching data from URL:', url);

    this.http.get<any[]>(url)
      .subscribe(
        (data) => {
          console.log('Data fetched:', data);
          this.plantData = data;
        },
        (error) => {
          console.error('Error fetching data:', error);
        }
      );
  }

  openAddPlantPopup() {
    this.showAddPlantPopup = true;
  }

  closeAddPlantPopup() {
    this.showAddPlantPopup = false;
  }

  openEditPlantPopup(plant: any) {
    this.selectedPlant = plant;
    this.selectedPlantId = plant.id;
    this.showEditPlantPopup = true;
  }

  closeEditPlantPopup() {
    this.showEditPlantPopup = false;
  }

  updatePlant(event: { data: any }) {
    const data = event.data;
    const id = this.selectedPlant.id;
    console.log('ID:', id);

    data.id = id;

    console.log('Objet:', this.selectedPlant);

    console.log('Edit:', data);

    this.http.put(`http://localhost:5000/api/plants/${id}`, data)
      .subscribe(
        () => {
          console.log('Success');
        },
        (error) => {
          console.error('Error:', error);

          if (error.status === 400) {
            console.error('Check sent data');
          } else if (error.status === 404) {
            console.error('Check id');
          } else {
            console.error('??');
          }

          console.log('?????:', error);
        }
      );
  }

  deletePlant(id: number) {
    this.http.delete(`http://localhost:5000/api/plants/${id}`)
      .subscribe(
        () => {
          this.fetchPlants();
        },
        (error) => {
          console.error('Error deleting plant:', error);
        }
      );
  }

  isSelectedPlant(plant: any): boolean {
    return this.selectedPlant && this.selectedPlant.plantName === plant.plantName;
  }

  setupWebSocket() {
    const socket = new WebSocket('ws://localhost:8080');

    socket.onopen = () => {
      console.log('Conectado al servidor WebSocket');
    };

    socket.onmessage = (event) => {
      const data = JSON.parse(event.data);
      console.log('Mensaje recibido del servidor:', data);
      this.updateRealtimeData(data);
    };

    socket.onclose = () => {
      console.log('Desconectado del servidor WebSocket');
    };
  }

  updateRealtimeData(data: any) {
    if (data && data.temperature !== undefined && data.humidity !== undefined && this.selectedPlantId === data.idPlant) {
      const realtimeDataDiv = document.getElementById('realtimeData');
      if (realtimeDataDiv) {
        realtimeDataDiv.innerHTML = `
        <p>Fecha y Hora: ${data.timeReceived}</p>
        <p>Temperatura: ${data.temperature.toFixed(2)} °C</p>
        <p>Humedad: ${data.humidity.toFixed(2)} %</p>
      `;
      }
    } else {
      // Si el idPlant del dato en tiempo real no coincide con el idPlant de la planta seleccionada, oculta los datos
      const realtimeDataDiv = document.getElementById('realtimeData');
      if (realtimeDataDiv) {
        realtimeDataDiv.innerHTML = '';
      }
    }
  }


  applyFilter() {
    const filterTextLower = this.filterText.toLowerCase();
    this.filteredPlants = this.plants.filter(plant =>
      (plant.plantName && plant.plantName.toLowerCase().includes(filterTextLower)) ||
      (plant.location && plant.location.toLowerCase().includes(filterTextLower)) ||
      (plant.type && plant.type.toLowerCase().includes(filterTextLower))
    );
  }

}
