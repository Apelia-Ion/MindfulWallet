import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  private apiUrl = 'https://localhost:7245/api';

  constructor(private http: HttpClient) {}



  getReportsByAccount(accountId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/Report/account/${accountId}`);
  }

  createReport(report: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/Report`, report);
  }

  deleteReport(reportId: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/Report/${reportId}`);
  }

  getCurrentMonthReport(accountId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/Report/current/${accountId}`);
  }

  createOrUpdateReport(report: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/Report`, report);
  }
}
