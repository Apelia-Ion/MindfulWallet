import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { GoalModel } from '../models/goal.model';


@Injectable({
  providedIn: 'root'
})
export class GoalService {

  private baseUrl: string = 'https://localhost:7245/api/Goal';

  constructor(private http: HttpClient) {}
  
  private longTermGoalsSubject = new BehaviorSubject<string[]>(this.getLongTermGoalsFromStorage());
  longTermGoals$ = this.longTermGoalsSubject.asObservable();

  setLongTermGoals(goals: string[]) {
    this.longTermGoalsSubject.next(goals);
    this.saveLongTermGoalsToStorage(goals);
  }

  private saveLongTermGoalsToStorage(goals: string[]) {
    localStorage.setItem('longTermGoals', JSON.stringify(goals));
  }

  private getLongTermGoalsFromStorage(): string[] {
    const goals = localStorage.getItem('longTermGoals');
    return goals ? JSON.parse(goals) : [];
  }

  clearLongTermGoals() {
    localStorage.removeItem('longTermGoals');
    this.longTermGoalsSubject.next([]);
  }


  addGoal(goal: GoalModel): Observable<GoalModel> {
    return this.http.post<GoalModel>(`${this.baseUrl}`, goal);
  }

  deleteGoal(goalId: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${goalId}`);
  }

  getAllGoals(userId: number): Observable<GoalModel[]> {
    return this.http.get<GoalModel[]>(`${this.baseUrl}/${userId}`);
  }



}
