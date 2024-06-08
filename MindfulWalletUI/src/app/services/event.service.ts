import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Event } from '../models/event.model'; // Import corect al modelului Event

@Injectable({
  providedIn: 'root'
})
export class EventService {
  private baseUrl = 'https://localhost:7245/api/Event'

  constructor(private http: HttpClient) { }

  getEvents(userId: number): Observable<Event[]> {
    return this.http.get<{ $values: Event[] }>(`${this.baseUrl}/user/${userId}`).pipe(
      map(response => response.$values)
    );
  }
}
