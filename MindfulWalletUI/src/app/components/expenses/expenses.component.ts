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
  public accounts: any[] = [];
  public accountForm: FormGroup;
  public expenseForm: FormGroup;
  public showForm: boolean = false;
  public showAddAccountModal: boolean = false;
  public showAddExpenseModal: boolean = false;
  public currentAccountId: number | null = null;
  public totalAmount: number = 0;
  public cashExpenses: any[] = [];
  public cardExpenses: any[] = [];
  public savingsExpenses: any[] = [];
  public accountsJson: string = '';

  constructor(
    private fb: FormBuilder,
    private financeService: FinanceService,
    private authService: AuthService
  ) {
    this.accountForm = this.fb.group({
      type: [''],
      amount: ['']
    });

    this.expenseForm = this.fb.group({
      description: [''],
      amount: [''],
      date: [new Date().toISOString().split('T')[0]] // Set default date to today
    });
  }

  ngOnInit(): void {
    this.loadAccounts();
  }

  loadAccounts(): void {
    this.authService.getUserId().subscribe((userId: number) => {
      this.financeService.getAccounts(userId).subscribe(data => {
        this.accounts = data.$values;
        this.accountsJson = JSON.stringify(this.accounts, null, 2);
        console.log('Accounts data:', this.accounts);
        this.accounts.forEach(account => this.loadExpenses(account));
        this.calculateTotalAndExpenses(); // Recalculăm sumele disponibile
      });
    });
  }
  
  loadExpenses(account: any): void {
    this.financeService.getLastThreeExpenses(account.id).subscribe(
      expenses => {
        if (expenses && expenses.$values) {
          // Sortăm cheltuielile după dată în ordine descrescătoare și luăm ultimele 3
          account.expenses = expenses.$values
            .sort((a: any, b: any) => new Date(b.date).getTime() - new Date(a.date).getTime())
            .slice(0, 3);
        } else {
          account.expenses = [];
        }
        console.log(`Expenses for account ${account.id}:`, account.expenses);
        this.calculateAccountBalance(account); // Recalculăm balanța contului
      },
      error => {
        if (error.status === 404) {
          account.expenses = []; // În cazul în care nu sunt găsite cheltuieli
        } else {
          console.error('Error loading expenses:', error);
        }
        this.calculateAccountBalance(account); // Recalculăm balanța contului chiar dacă nu sunt cheltuieli
      }
    );
  }


  calculateAccountBalance(account: any): void {
    const totalExpenses = account.expenses.reduce((sum: number, expense: any) => sum + expense.amount, 0);
    account.balance = account.amount - totalExpenses;
    this.calculateTotalAndExpenses(); // Recalculăm sumele disponibile totale
  }
  
  
  

  calculateTotalAndExpenses(): void {
    this.totalAmount = this.accounts.reduce((sum, account) => sum + account.balance, 0);
    this.cashExpenses = [];
    this.cardExpenses = [];
    this.savingsExpenses = [];
  
    this.accounts.forEach(account => {
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
  }

  toggleForm(): void {
    this.showForm = !this.showForm;
  }

  addAccount(): void {
    this.authService.getUserId().subscribe((userId: number) => {
      const account = this.accountForm.value;
      this.financeService.addAccount(userId, account).subscribe(() => {
        this.loadAccounts();
        this.accountForm.reset();
        this.closeAddAccountModal();
      });
    });
  }

  deleteAccount(accountId: number): void {
    this.financeService.deleteAccount(accountId).subscribe(() => {
      this.loadAccounts();
    });
  }

  addExpense(accountId: number | null): void {
    if (accountId === null) {
      console.error('Account ID is null');
      return;
    }
    const expense = this.expenseForm.value;
    expense.accountId = accountId;
    this.financeService.addExpense(expense).subscribe(() => {
      this.loadAccounts();
      this.expenseForm.reset({ date: new Date().toISOString().split('T')[0] }); // Reset form and set date to today
      this.closeAddExpenseModal();
    });
  }

  deleteExpense(expenseId: number): void {
    this.financeService.deleteExpense(expenseId).subscribe(() => {
      this.loadAccounts();
    });
  }

  openAddAccountModal(): void {
    this.showAddAccountModal = true;
  }

  closeAddAccountModal(): void {
    this.showAddAccountModal = false;
  }

  openAddExpenseModal(accountId: number): void {
    this.currentAccountId = accountId;
    this.showAddExpenseModal = true;
  }

  closeAddExpenseModal(): void {
    this.showAddExpenseModal = false;
    this.currentAccountId = null;
  }
}