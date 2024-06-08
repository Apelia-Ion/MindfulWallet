import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HomeService {
  private baseUrl: string = "https://localhost:7245/api/User/";

  constructor(private http: HttpClient) { }

  getUsers()
  {
    return this.http.get<any>(this.baseUrl);
  }

  getUserAccounts(userId: number): Observable<any> {
    return this.http.get<any>(`https://localhost:7245/api/Account/user/${userId}`);
  }

  getCurrentMonthReport(accountId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/Report/current/${accountId}`);
  }

  getUserAchievements(userId: number): Observable<any> {
    return this.http.get(`https://localhost:7245/api/Account/user/${userId}/achievements`);
  }
}
