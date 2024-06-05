import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FinanceService } from '../../services/finance.service';
import { HttpClientModule } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-expenses',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule, HttpClientModule],
  templateUrl: './expenses.component.html',
  styleUrls: ['./expenses.component.css']
})
export class ExpensesComponent implements OnInit {
  public accounts: any[] = []; // Utilizează un array simplu pentru accounts
  public accountForm: FormGroup;
  public showForm: boolean = false;

  constructor(
    private fb: FormBuilder,
    private financeService: FinanceService,
    private authService: AuthService
  ) {
    this.accountForm = this.fb.group({
      type: [''],
      amount: ['']
    });
  }

  ngOnInit(): void {
    this.loadAccounts();
  }

  loadAccounts(): void {
    this.authService.getUserId().subscribe(userId => {
      console.log('User ID:', userId); // Log ID-ul utilizatorului
      this.financeService.getAccounts(userId).subscribe(data => {
        console.log('Accounts data received from API:', data); // Log datele preluate
        this.accounts = data.$values; // Extrage array-ul de conturi din obiectul primit
        console.log('Accounts assigned in component:', this.accounts); // Log datele după atribuire
      }, error => {
        console.error('Error loading accounts:', error);
      });
    }, error => {
      console.error('Error getting user ID:', error);
    });
  }

  toggleForm(): void {
    this.showForm = !this.showForm;
  }

  addAccount(): void {
    this.authService.getUserId().subscribe(userId => {
      const account = this.accountForm.value;
      this.financeService.addAccount(userId, account).subscribe(() => {
        this.loadAccounts();
        this.accountForm.reset();
        this.showForm = false;
      }, error => {
        console.error('Error adding account:', error);
      });
    }, error => {
      console.error('Error getting user ID:', error);
    });
  }
}
