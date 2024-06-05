import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { TokenApiModel } from '../models/token-api.model';
import { Observable, map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl: string = "https://localhost:7245/api/User/";
  private userPayload:any;

  constructor(private http: HttpClient, private router : Router) { 
    this.userPayload=this.decodeToken();
  }

  signup(userObj:any){
    return this.http.post<any>(`${this.baseUrl}register`, userObj)
  }

  login(loginObj: any){
    return this.http.post<any>(`${this.baseUrl}authenticate`, loginObj)
  }

  storeToken(tokenValue: string){
    localStorage.setItem('token', tokenValue)
  }

  getToken(){
    return localStorage.getItem('token')
  }

  isLoggedIn(): boolean{
    return !!localStorage.getItem('token')
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken')
    this.router.navigate(['login']);
  }

  decodeToken(){
    const jwtHelper = new JwtHelperService();
    const token = this.getToken()!;
    console.log(jwtHelper.decodeToken(token));
    return jwtHelper.decodeToken(token);
  }

  getFullNameFromToken(){
    if(this.userPayload)
      return this.userPayload.unique_name;

  }

  getRoleFromToken(){
    if(this.userPayload)
      return this.userPayload.role;
  }

  renewToken(tokenApi : TokenApiModel){
    return this.http.post<any>(`${this.baseUrl}refresh`, tokenApi);
  }

  storeRefreshToken(tokenValue: string){
    localStorage.setItem('refreshToken', tokenValue)
  }

  getRefreshToken(){
    return localStorage.getItem('refreshToken')
  }

  getUserDetailsByUsername(username: string): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}getUserByUsername/${username}`);
  }

  getUserId(): Observable<number> {
    const username = this.getFullNameFromToken();
    return this.getUserDetailsByUsername(username).pipe(
      map(user => user.id)
    );
  }



}
