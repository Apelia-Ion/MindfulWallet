import { Component, OnInit } from '@angular/core';
import { HomeService } from '../../services/home.service';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { UserStoreService } from '../../services/user-store.service';
import { ReportService } from '../../services/report.service';
import { forkJoin, map } from 'rxjs';

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

  constructor(
    private api: HomeService, 
    private authentication: AuthService, 
    private userStore: UserStoreService,
    private reportService: ReportService
  ) {}

  ngOnInit(): void {
    this.userStore.getFullNameFromStore().subscribe(val => {
      let fullNameFromToken = this.authentication.getFullNameFromToken();
      this.userName = val || fullNameFromToken;
    });
    this.userStore.getRoleFromStore().subscribe(val => {
      let role = this.authentication.getRoleFromToken();
      this.userRole = val || role;
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
              map(report => ({ ...report, accountType: account.type }))
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
    });
  }

  toggleUsers(): void {
    this.showUsers = !this.showUsers;
    if (this.showUsers) {
      this.api.getUsers().subscribe(res => this.users = res);
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
}
