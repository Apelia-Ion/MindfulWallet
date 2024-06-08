import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { EventService } from '../../services/event.service';
import { Event } from '../../models/event.model';
import { AuthService } from '../../services/auth.service';

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

  monthNames = [
    "January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
  ];

  constructor(
    private eventService: EventService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    const today = new Date();
    this.currentYear = today.getFullYear();
    this.currentMonth = today.getMonth();

    this.authService.getUserId().subscribe(
      id => {
        this.userId = id;
        this.loadEvents(this.userId);
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
      calendarDays.push({ date: null, events: [] });
    }

    // Fill in the days of the month
    for (let i = 1; i <= lastDate; i++) {
      const dayEvents = this.events.filter(event => new Date(event.date).getDate() === i);
      calendarDays.push({ date: i, events: dayEvents });
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
    this.loadEvents(this.userId);
  }

  nextMonth(): void {
    if (this.currentMonth === 11) {
      this.currentMonth = 0;
      this.currentYear++;
    } else {
      this.currentMonth++;
    }
    this.loadEvents(this.userId);
  }
}
