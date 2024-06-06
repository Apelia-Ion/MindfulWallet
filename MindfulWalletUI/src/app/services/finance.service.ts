import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FinanceService {
  private apiUrl = 'https://localhost:7245/api'; // Actualizați URL-ul API după nevoie

  constructor(private http: HttpClient) {}

  getAccounts(userId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/Finance/${userId}`);
  }

  addAccount(userId: number, account: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/Account/${userId}`, account);
  }

  deleteAccount(accountId: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/Account/${accountId}`);
  }
}
