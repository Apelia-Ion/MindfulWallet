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
  public accountForm: FormGroup;
  public expenseForm: FormGroup;
  public showAddAccountModal: boolean = false;
  public showAddExpenseModal: boolean = false;

  public accounts: any[] = [];
  public totalAmount: number = 0;
  public cashExpenses: any[] = [];
  public cardExpenses: any[] = [];
  public savingsExpenses: any[] = [];
  public currentAccountId: number | null = null;
  public finance: any;

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
    this.loadFinance();
  }

  loadFinance(): void {
    this.authService.getUserId().subscribe((userId: number) => {
      this.financeService.getFinance(userId).subscribe(finance => {
        console.log('Finance data:', finance);

        this.finance = finance;

        if (finance.accounts && Array.isArray(finance.accounts.$values)) {
          this.accounts = finance.accounts.$values;
          this.totalAmount = finance.totalAmount;

          // Load the last three expenses for each account
          this.accounts.forEach(account => {
            this.financeService.getLastThreeExpenses(account.id).subscribe(expenses => {
              if (expenses && Array.isArray(expenses.$values)) {
                account.expenses = expenses.$values; // Store last three expenses in the account object
                this.updateExpenses(account); // Update separate arrays
              } else {
                console.error('Expenses is not an array:', expenses);
              }
            }, error => {
              console.error('Error fetching last three expenses:', error);
            });
          });
        } else {
          console.error('Accounts is not an array:', finance.accounts);
        }
      });
    });
  }

  addAccount(): void {
    this.authService.getUserId().subscribe((userId: number) => {
      const account = this.accountForm.value;
      this.financeService.addAccount(userId, account).subscribe(() => {
        this.loadFinance();
        this.accountForm.reset();
        this.closeAddAccountModal();
      }, error => {
        console.error('Error adding account:', error);
      });
    });
  }

  deleteAccount(accountId: number): void {
    this.financeService.deleteAccount(accountId).subscribe(() => {
      this.loadFinance();
    });
  }

  addExpense(accountId: number | null): void {
    if (accountId === null) {
      console.error('Account ID is null');
      return;
    }
    const expenseDto = {
      accountId: accountId,
      amount: parseFloat(this.expenseForm.value.amount),  // Ensure the amount is a number
      date: this.expenseForm.value.date,
      description: this.expenseForm.value.description
    };
    this.financeService.addExpense(expenseDto).subscribe(() => {
      this.loadFinance();
      this.expenseForm.reset({ date: new Date().toISOString().split('T')[0] });
      this.closeAddExpenseModal();
    }, error => {
      console.error('Error adding expense:', error);
    });
  }

  deleteExpense(accountId: number, expenseId: number): void {
    this.financeService.deleteExpense(expenseId).subscribe(() => {
      this.loadFinance();
    });
  }

  updateExpenses(account: any): void {
    switch (account.type.toLowerCase()) {
      case 'cash':
        this.cashExpenses = account.expenses;
        break;
      case 'card':
        this.cardExpenses = account.expenses;
        break;
      case 'economii':
        this.savingsExpenses = account.expenses;
        break;
    }
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
    this.expenseForm.reset({ date: new Date().toISOString().split('T')[0] });
  }

  closeAddExpenseModal(): void {
    this.showAddExpenseModal = false;
    this.currentAccountId = null;
  }
}
