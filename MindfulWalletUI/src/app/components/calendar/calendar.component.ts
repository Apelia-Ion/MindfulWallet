import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { EventService } from '../../services/event.service';
import { Event } from '../../models/event.model';
import { AuthService } from '../../services/auth.service';
import { GoalService } from '../../services/goal.service';
import { GoalModel } from '../../models/goal.model';

@Component({
  selector: 'app-calendar',
  imports: [CommonModule],
  standalone: true,
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.css']
})
export class CalendarComponent implements OnInit {
  currentYear!: number;
  currentMonth!: number;
  weeks: any[] = [];
  events: Event[] = [];
  userId!: number;
  showModal = false;
  modalEvents: Event[] = [];
  specificGoals: GoalModel[] = [];

  monthNames = [
    "January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
  ];

  constructor(
    private eventService: EventService,
    private authService: AuthService,
    private goalService: GoalService
  ) {}

  ngOnInit(): void {
    const today = new Date();
    this.currentYear = today.getFullYear();
    this.currentMonth = today.getMonth();
  
    this.authService.getUserId().subscribe(
      id => {
        this.userId = id;
        this.loadEvents(this.userId);
        this.loadSpecificGoals(this.userId); // Mutat aici pentru a te asigura că userId este setat
      },
      error => {
        console.error('Error fetching user ID:', error);
      }
    );
  }
  

  loadEvents(userId: number): void {
    this.eventService.getEvents(userId).subscribe(events => {
      this.events = Array.isArray(events) ? events : [];
      console.log('Loaded events:', this.events); // Debug: Afișează evenimentele încărcate
      this.generateCalendar(this.currentYear, this.currentMonth);
    });
  }

  generateCalendar(year: number, month: number): void {
    const firstDay = new Date(year, month, 1).getDay();
    const lastDate = new Date(year, month + 1, 0).getDate();
    const calendarDays = [];
  
    // Fill in days before the first day of the month
    for (let i = 0; i < firstDay; i++) {
      calendarDays.push({ date: null, events: [], goals: [] });
    }
  
    // Fill in the days of the month
    for (let i = 1; i <= lastDate; i++) {
      const dayEvents = this.events.filter(event => {
        const eventDate = new Date(event.date);
        return eventDate.getDate() === i && eventDate.getMonth() === month && eventDate.getFullYear() === year;
      });
  
      const dayGoals = this.specificGoals.filter(goal => {
        const goalDate = new Date(goal.dueDate);
        return goalDate.getDate() === i && goalDate.getMonth() === month && goalDate.getFullYear() === year;
      });
  
      calendarDays.push({ date: i, events: dayEvents, goals: dayGoals });
    }
  
    // Group the days into weeks
    this.weeks = [];
    while (calendarDays.length > 0) {
      this.weeks.push(calendarDays.splice(0, 7));
    }
  }
  

  prevMonth(): void {
    if (this.currentMonth === 0) {
      this.currentMonth = 11;
      this.currentYear--;
    } else {
      this.currentMonth--;
    }
    this.generateCalendar(this.currentYear, this.currentMonth);
  }

  nextMonth(): void {
    if (this.currentMonth === 11) {
      this.currentMonth = 0;
      this.currentYear++;
    } else {
      this.currentMonth++;
    }
    this.generateCalendar(this.currentYear, this.currentMonth);
  }

  openModal(events: Event[]): void {
    this.modalEvents = events;
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
    this.modalEvents = [];
  }



  /// aduc si goals in calendar

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
}
