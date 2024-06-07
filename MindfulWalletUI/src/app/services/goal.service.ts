import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GoalService {
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
}
