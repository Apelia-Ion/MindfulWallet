import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { GoalService } from '../../services/goal.service';

interface SpecificGoal {
  title: string;
  description: string;
  motivation: string;
  date: Date;
}

@Component({
  selector: 'app-goals',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule, HttpClientModule],
  templateUrl: './goals.component.html',
  styleUrls: ['./goals.component.css']
})
export class GoalsComponent implements OnInit {
  longTermGoals: string[] = [];
  specificGoals: SpecificGoal[] = [];
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
  newGoal: SpecificGoal = { title: '', description: '', motivation: '', date: new Date() };

  constructor(private goalService: GoalService) {}

  ngOnInit() {
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
    this.specificGoals.push(this.newGoal);
    this.newGoal = { title: '', description: '', motivation: '', date: new Date() };
    this.closeSpecificGoalModal();
  }
}
