import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-quiz',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule, HttpClientModule],
  templateUrl: './quiz.component.html',
  styleUrls: ['./quiz.component.css']
})
export class QuizComponent {
  questions: string[] = [
    'Iti doresti produsul?',
    'Această achiziție va face viața mai bună sau mai ușoară?',
    'Chiar AI NEVOIE de acest articol?',
    'Îți permiți sa cumperi produsul?',
    'Îți aduce bucurie?',
    'Ai mai folosit un produs similar înainte?',
    'Este un produs de calitate?',
    'Ai verificat recenziile produsului?',
    'Achiziționarea acestui produs se aliniază cu obiectivele tale financiare pe termen lung?',
    'Achiziționarea acestui produs îți va aduce beneficii pe termen lung?',
    'Este un produs pe care îl vei folosi frecvent?',
    'Este un produs ușor de întreținut?',
    'Este cumva la reducere?',
    'Banii tăi ar putea fi cheltuiți mai bine pe altceva?',
    'Este strălucitor?',
    'Poți găsi același produs mai ieftin în altă parte?',
    'Te simți presat să cumperi acum?'
  ];

  answers: number[] = [];
  resultMessage: string = '';

  addAnswer(answer: string, index: number) {
    if (index < 12) {
      this.answers[index] = answer === 'da' ? 1 : -1;
    } else {
      this.answers[index] = answer === 'da' ? -1 : 1;
    }
  }

  submitQuiz() {
    const totalScore = this.answers.reduce((acc, curr) => acc + curr, 0);

    if (totalScore > 5) {
      this.resultMessage = 'Achiziția pare una utilă.';
    } else if (totalScore >= 0) {
      this.resultMessage = 'Ar trebui să te mai gândești, revino mai târziu.';
    } else {
      this.resultMessage = 'Nu ar trebui să faci această achiziție.';
    }

    // Afisarea modalului
    const modal = document.getElementById('resultModal');
    if (modal) {
      modal.style.display = 'block';
    }
  }

  closeModal() {
    const modal = document.getElementById('resultModal');
    if (modal) {
      modal.style.display = 'none';
    }
  }
}
