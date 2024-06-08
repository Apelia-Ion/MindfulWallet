import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { GoalService } from '../../services/goal.service';
import { GoalModel } from '../../models/goal.model';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-goals',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule, HttpClientModule],
  templateUrl: './goals.component.html',
  styleUrls: ['./goals.component.css']
})
export class GoalsComponent implements OnInit {
  userId!: number;
  longTermGoals: string[] = [];
  specificGoals: GoalModel[] = [];
  isLongTermModalOpen: boolean = false;
  isSpecificGoalModalOpen: boolean = false;
  longTermOptions: string[] = [
    'Economii pe termen lung', 
    'Reducerea cheltuielilor', 
    'Investiții pe termen lung',
    'Planificarea pentru pensionare',
    'Plata datoriilor',
    'Asigurarea viitorului copiilor'
  ];

  newGoal: GoalModel = { id: 0, userId: 0, title: '', description: '', motivation: '', dueDate: new Date(), amount: 0, status: 'pending' };

  constructor(private goalService: GoalService, private authService: AuthService) {}

  ngOnInit() {
    this.authService.getUserId().subscribe(
      id => {
        this.userId = id;
        this.loadSpecificGoals(this.userId);
      },
      error => {
        console.error('Error fetching user ID:', error);
      }
    );
  
    this.goalService.longTermGoals$.subscribe(goals => {
      this.longTermGoals = goals;
    });
  }

  openLongTermModal() {
    this.isLongTermModalOpen = true;
  }

  closeLongTermModal() {
    this.isLongTermModalOpen = false;
  }

  toggleLongTermGoal(option: string) {
    const index = this.longTermGoals.indexOf(option);
    if (index === -1) {
      this.longTermGoals.push(option);
    } else {
      this.longTermGoals.splice(index, 1);
    }
  }

  saveLongTermGoals() {
    this.goalService.setLongTermGoals(this.longTermGoals);
    this.closeLongTermModal();
  }

  clearLongTermGoals() {
    this.goalService.clearLongTermGoals();
    this.longTermGoals = [];
  }

  openSpecificGoalModal() {
    this.isSpecificGoalModalOpen = true;
  }

  closeSpecificGoalModal() {
    this.isSpecificGoalModalOpen = false;
  }

  saveSpecificGoal() {
    // Asigură-te că setăm userId corect în newGoal
    this.newGoal.userId = this.userId;
    console.log('Preparing to save goal:', this.newGoal);  // Log pentru newGoal
  
    this.goalService.addGoal(this.newGoal).subscribe(
      response => {
        console.log('Goal saved successfully:', response);  // Log pentru răspunsul de la server
        this.specificGoals.push(response);
        this.newGoal = { id: 0, userId: 0, title: '', description: '', motivation: '', dueDate: new Date(), amount: 0, status: 'pending' };
        console.log('Reset newGoal:', this.newGoal);  // Log pentru resetarea newGoal
        this.closeSpecificGoalModal();
      },
      error => {
        console.error('Error adding goal:', error);  // Log pentru erori
      }
    );
  }
  

  loadSpecificGoals(userId: number) {
    this.goalService.getAllGoals(userId).subscribe(
      (response: any) => {
        this.specificGoals = response.$values || []; // Asigură-te că extragi array-ul de obiecte
        console.log('Loaded specific goals:', this.specificGoals); // Debug: Verifică structura datelor
      },
      error => {
        console.error('Error fetching goals:', error);
      }
    );
  }

  deleteGoal(goalId: number) {
    this.goalService.deleteGoal(goalId).subscribe(
      () => {
        this.specificGoals = this.specificGoals.filter(goal => goal.id !== goalId);
      },
      error => {
        console.error('Error deleting goal:', error);
      }
    );
  }
}
