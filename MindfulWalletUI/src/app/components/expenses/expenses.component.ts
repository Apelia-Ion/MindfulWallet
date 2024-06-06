import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FinanceService } from '../../services/finance.service';
import { HttpClientModule } from '@angular/common/http';
import { AuthService } from '../../services/auth.service'; // Calea corectă către AuthService

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
  public totalAmount: number = 0;
  public cashExpenses: any[] = [];
  public cardExpenses: any[] = [];
  public savingsExpenses: any[] = [];

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
    this.authService.getUserId().subscribe((userId: number) => { // Specificăm tipul pentru userId
      console.log('User ID:', userId); // Log ID-ul utilizatorului
      this.financeService.getAccounts(userId).subscribe(data => {
        console.log('Accounts data received from API:', data); // Log datele preluate
        this.accounts = data.$values; // Extrage array-ul de conturi din obiectul primit
        console.log('Accounts assigned in component:', this.accounts); // Log datele după atribuire
        this.calculateTotalAndExpenses();
      }, (error: any) => { // Specificăm tipul pentru error
        console.error('Error loading accounts:', error);
      });
    }, (error: any) => { // Specificăm tipul pentru error
      console.error('Error getting user ID:', error);
    });
  }

  calculateTotalAndExpenses(): void {
    this.totalAmount = 0;
    this.cashExpenses = [];
    this.cardExpenses = [];
    this.savingsExpenses = [];

    this.accounts.forEach(account => {
      this.totalAmount += account.amount;

      if (account.expenses) {
        switch (account.type.toLowerCase()) {
          case 'cash':
            this.cashExpenses.push(...account.expenses);
            break;
          case 'card':
            this.cardExpenses.push(...account.expenses);
            break;
          case 'economii':
            this.savingsExpenses.push(...account.expenses);
            break;
        }
      }
    });

    console.log('Total Amount:', this.totalAmount);
    console.log('Cash Expenses:', this.cashExpenses);
    console.log('Card Expenses:', this.cardExpenses);
    console.log('Savings Expenses:', this.savingsExpenses);
  }

  toggleForm(): void {
    this.showForm = !this.showForm;
  }

  addAccount(): void {
    this.authService.getUserId().subscribe((userId: number) => { // Specificăm tipul pentru userId
      const account = this.accountForm.value;
      this.financeService.addAccount(userId, account).subscribe(() => {
        this.loadAccounts();
        this.accountForm.reset();
        this.showForm = false;
      }, (error: any) => { // Specificăm tipul pentru error
        console.error('Error adding account:', error);
      });
    }, (error: any) => { // Specificăm tipul pentru error
      console.error('Error getting user ID:', error);
    });
  }

  deleteAccount(accountId: number): void {
    this.financeService.deleteAccount(accountId).subscribe(() => {
      this.loadAccounts();
    }, (error: any) => { // Specificăm tipul pentru error
      console.error('Error deleting account:', error);
    });
  }
}
