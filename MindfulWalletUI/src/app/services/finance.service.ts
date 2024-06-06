import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FinanceService {
  private apiUrl = 'https://localhost:7245/api';

  constructor(private http: HttpClient) {}

  getFinance(userId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/Finance/${userId}`);
  }

  addAccount(userId: number, account: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/Account/${userId}`, account);
  }

  deleteAccount(accountId: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/Account/${accountId}`);
  }

  addExpense(expense: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/Expense`, expense);
  }

  deleteExpense(expenseId: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/Expense/${expenseId}`);
  }

  getLastThreeExpenses(accountId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/Expense/lastThree/${accountId}`);
  }

  getAllExpenses(accountId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/Account/${accountId}/expenses`);
  }

  addFunds(funds: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/Account/addFunds`, funds);
  }
}
