import { Component, OnInit } from '@angular/core';
import { HomeService } from '../../services/home.service';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { UserStoreService } from '../../services/user-store.service';
import { ReportService } from '../../services/report.service';
import { forkJoin, map, catchError, of } from 'rxjs';
import { GoalService } from '../../services/goal.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  public users: any[] = [];
  public userName: string = "";
  public userRole: string = "";
  public showUsers: boolean = false;
  public accounts: any[] = [];
  public currentMonthReports: any[] = [];
  public currentAccountIndex: number = 0;

  longTermGoals: string[] = [];
  personalizedAdvice: string[] = [];

  constructor(
    private api: HomeService, 
    private authentication: AuthService, 
    private userStore: UserStoreService,
    private reportService: ReportService,
    private goalService: GoalService
  ) {}

  ngOnInit(): void {
    this.userStore.getFullNameFromStore().subscribe(val => {
      let fullNameFromToken = this.authentication.getFullNameFromToken();
      this.userName = val || fullNameFromToken;
    }, error => {
      console.error('Error fetching full name from store:', error);
    });
    
    this.userStore.getRoleFromStore().subscribe(val => {
      let role = this.authentication.getRoleFromToken();
      this.userRole = val || role;
    }, error => {
      console.error('Error fetching role from store:', error);
    });

    this.authentication.getUserId().subscribe(userId => {
      console.log('User ID:', userId);
      this.api.getUserAccounts(userId).subscribe((response: any) => {
        if (response && response.$values) {
          this.accounts = response.$values;
          console.log('User Accounts Response:', response);
          console.log('Accounts:', this.accounts);
          
          const reportRequests = this.accounts.map((account: any) => 
            this.reportService.getCurrentMonthReport(account.id).pipe(
              map(report => ({ ...report, accountType: account.type })),
              catchError(error => {
                console.error('Error fetching report for account:', account.id, error);
                return of(null);
              })
            )
          );

          forkJoin(reportRequests).subscribe((reports: any[]) => {
            console.log('Reports:', reports);
            this.currentMonthReports = reports.filter(report => report != null);
          }, error => {
            console.error('Error fetching current month reports:', error);
          });
        } else {
          console.error('Invalid response structure', response);
        }
      }, error => {
        console.error('Error fetching user accounts:', error);
      });
    }, error => {
      console.error('Error fetching user ID:', error);
    });

    this.goalService.longTermGoals$.subscribe(goals => {
      this.longTermGoals = goals;
      this.generateAdvice();
    }, error => {
      console.error('Error fetching long term goals:', error);
    });
  }

  toggleUsers(): void {
    this.showUsers = !this.showUsers;
    if (this.showUsers) {
      this.api.getUsers().subscribe(res => this.users = res, error => {
        console.error('Error fetching users:', error);
      });
    }
  }

  showNextReport(): void {
    if (this.currentAccountIndex < this.currentMonthReports.length - 1) {
      this.currentAccountIndex++;
    }
  }

  showPreviousReport(): void {
    if (this.currentAccountIndex > 0) {
      this.currentAccountIndex--;
    }
  }

  generateAdvice(): void {
    const adviceList: { [key: string]: string } = {
      'Economii pe termen lung': 'Stabilește un buget lunar și încearcă să economisești cel puțin 10% din venitul tău.',
      'Reducerea cheltuielilor': 'Fă o listă de priorități și evită cheltuielile impulsive. Încearcă să cumperi doar ce este necesar.',
      'Investiții pe termen lung': 'Caută oportunități de investiții și educă-te în domeniul financiar pentru a lua decizii informate.',
      'Planificarea pentru pensionare': 'Planifică-ți pensionarea economisind și investind în mod regulat.',
      'Plata datoriilor': 'Prioritizează plata datoriilor cu cele mai mari dobânzi și încearcă să faci plăți suplimentare.',
      'Asigurarea viitorului copiilor': 'Deschide un cont de economii pentru educația copiilor și contribuie periodic.'
    };

    this.personalizedAdvice = this.longTermGoals.map(goal => adviceList[goal]);
  }
}
