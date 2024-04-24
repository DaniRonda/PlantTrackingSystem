import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

interface ConversionRecord {
  id: number;
  fromCurrency: string;
  toCurrency: string;
  amount: number;
  convertedAmount: number;
  date: string;
}

interface ConversionResponse {
  amount: number;
  fromCurrency: string;
  convertedAmount: number;
  toCurrency: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  amount: number = 0;
  fromCurrency: string = 'USD';
  toCurrency: string = 'EUR';
  convertedAmount: number = 0;
  conversionHistory: ConversionRecord[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.getConversionHistory();
  }

  convertCurrency(): void {
    const request = {
      amount: this.amount,
      fromCurrency: this.fromCurrency,
      toCurrency: this.toCurrency
    };

    this.http.post<ConversionResponse>('http://localhost:5000/api/Conversion/ConvertCurrency', request)
      .subscribe(
        response => {
          this.convertedAmount = response.convertedAmount;
          console.log('Conversion successful:', response);
          this.getConversionHistory();
        },
        error => {
          console.error('Error converting currency:', error);
        }
      );
  }

  getConversionHistory(): void {
    this.http.get<ConversionRecord[]>('http://localhost:5000/api/Conversion/history')
      .subscribe(
        history => {
          this.conversionHistory = history;
          console.log('Conversion history:', history);
        },
        error => {
          console.error('Error fetching conversion history:', error);
        }
      );
  }
}
