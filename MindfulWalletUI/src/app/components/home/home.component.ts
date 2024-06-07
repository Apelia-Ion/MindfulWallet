import { Component, OnInit } from '@angular/core';
import { HomeService } from '../../services/home.service';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { UserStoreService } from '../../services/user-store.service';
import { ReportService } from '../../services/report.service';
import { forkJoin } from 'rxjs';

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

    // Fetch user accounts and their respective current month reports
    this.authentication.getUserId().subscribe(userId => {
      console.log('User ID:', userId);
      this.api.getUserAccounts(userId).subscribe((response: any) => {
        if (response && response.$values) {
          // Filtrăm doar obiectele contului valide
          this.accounts = response.$values.filter((account: any) => account.id !== undefined); 
          console.log('Accounts:', this.accounts);
          const reportRequests = this.accounts.map((account: any) => {
            const accountId = account.id; // Accesăm corect proprietatea id
            return this.reportService.getCurrentMonthReport(accountId);
          });

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
}
