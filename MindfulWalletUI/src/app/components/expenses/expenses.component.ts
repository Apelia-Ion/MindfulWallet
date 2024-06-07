import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FinanceService } from '../../services/finance.service';
import { HttpClientModule } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';
import { ReportService } from '../../services/report.service';

interface Report {
  id: number; // Adăugăm proprietatea 'id'
  month: string;
  totalExpenses: number;
  numberOfExpenses: number;
  expenses: { description: string, amount: number }[];
}

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
  public fundsForm: FormGroup;

  public showAddAccountModal: boolean = false;
  public showAddExpenseModal: boolean = false;
  public showAddFundsModal: boolean = false;

  public accounts: any[] = [];
  public totalAmount: number = 0;
  public currentAccountId: number | null = null;
  public finance: any;

  public expenseReports: any[] = [];
  public cashExpenses: any[] = [];
  public cardExpenses: any[] = [];
  public savingsExpenses: any[] = [];

  public allExpenses: { [key: number]: any[] } = {}; // Cheltuieli pentru fiecare cont, cheile sunt ID-urile conturilor

  public expenseReportsFromDb: { [key: number]: Report[] } = {};
  public currentMonthIndex: { [key: number]: number } = {};
  public currentAccountIndex: number = 0;

  constructor(
    private fb: FormBuilder,
    private financeService: FinanceService,
    private authService: AuthService,
    private reportService: ReportService // Injectăm serviciul de rapoarte
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

    this.fundsForm = this.fb.group({
      amount: ['']
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

          // Load all expenses for each account
          this.accounts.forEach(account => {
            this.loadExpenses(account);
            this.loadReports(account.id); // Load reports for each account
          });
        } else {
          console.error('Accounts is not an array:', finance.accounts);
        }
      });
    });
  }

  loadExpenses(account: any): void {
    this.financeService.getAllExpenses(account.id).subscribe(expenses => {
      if (expenses && Array.isArray(expenses.$values)) {
        this.allExpenses[account.id] = expenses.$values; // Store all expenses separately
        account.expenses = expenses.$values.slice(0, 3); // Store last three expenses in the account object
        this.generateMonthlyReports(account.id); // Generate and save reports for each month with expenses
      } else {
        account.expenses = []; // Initialize as empty array if not an array
        this.allExpenses[account.id] = [];
      }
      this.updateExpenses(account); // Update expense arrays
    }, error => {
      console.error('Error fetching all expenses:', error);
      account.expenses = []; // Initialize as empty array on error
      this.allExpenses[account.id] = [];
      this.updateExpenses(account); // Update expense arrays
    });
  }

  loadReports(accountId: number): void {
    this.reportService.getReportsByAccount(accountId).subscribe(reports => {
      if (reports.$values && Array.isArray(reports.$values)) {
        this.expenseReportsFromDb[accountId] = reports.$values.sort((a: Report, b: Report) => new Date(a.month).getTime() - new Date(b.month).getTime());
        this.currentMonthIndex[accountId] = this.expenseReportsFromDb[accountId].length - 1; // Set the index to the last report
      } else {
        this.expenseReportsFromDb[accountId] = [];
      }
      console.log('Reports for account', accountId, ':', this.expenseReportsFromDb[accountId]);
    }, error => {
      console.error('Error fetching reports:', error);
      this.expenseReportsFromDb[accountId] = [];
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
      amount: parseFloat(this.expenseForm.value.amount), // Ensure the amount is a number
      date: this.expenseForm.value.date,
      description: this.expenseForm.value.description
    };
    this.financeService.addExpense(expenseDto).subscribe(() => {
      this.loadFinance();
      this.expenseForm.reset({ date: new Date().toISOString().split('T')[0] });
      this.closeAddExpenseModal();
      window.location.reload();      //reload page
    }, error => {
      console.error('Error adding expense:', error);
    });
  }

  addFunds(accountId: number | null): void {
    if (accountId === null) {
      console.error('Account ID is null');
      return;
    }
    const fundsDto = {
      accountId: accountId,
      amount: parseFloat(this.fundsForm.value.amount) // Ensure the amount is a number
    };
    this.financeService.addFunds(fundsDto).subscribe(() => {
      this.loadFinance();
      this.fundsForm.reset();
      this.closeAddFundsModal();
    }, error => {
      console.error('Error adding funds:', error);
    });
  }

  deleteExpense(accountId: number, expenseId: number): void {
    this.financeService.deleteExpense(expenseId).subscribe(() => {
      this.loadFinance();
      this.expenseReportsFromDb[accountId].forEach(report => {
        const reportExpenses = this.allExpenses[accountId].filter(expense => new Date(expense.date).toISOString().substring(0, 7) === report.month);
        if (reportExpenses.length === 0) {
          this.reportService.deleteReport(report.id).subscribe(() => {
            console.log(`Report ${report.id} deleted successfully`);
            this.loadReports(accountId); // Reîncarcă rapoartele după ștergere
            window.location.reload();
          }, error => {
            console.error('Error deleting report:', error);
          });
        }
      });
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
    this.generateExpenseReports(); // Generate expense reports
  }

  generateExpenseReports(): void {
    this.expenseReports = this.accounts.filter(account => this.allExpenses[account.id] && this.allExpenses[account.id].length > 0).map(account => {
      const totalExpenses = this.allExpenses[account.id].reduce((sum: number, expense: any) => sum + expense.amount, 0);
      const numberOfExpenses = this.allExpenses[account.id].length;
      return {
        accountType: account.type,
        totalExpenses,
        numberOfExpenses,
        expenses: this.allExpenses[account.id]
      };
    });
  }

  generateMonthlyReports(accountId: number): void {
    const expenses = this.allExpenses[accountId];
    const monthlyExpenses: { [key: string]: any[] } = {};

    expenses.forEach(expense => {
      const month = new Date(expense.date).toISOString().substring(0, 7); // Get month in format 'YYYY-MM'
      if (!monthlyExpenses[month]) {
        monthlyExpenses[month] = [];
      }
      monthlyExpenses[month].push(expense);
    });

    Object.keys(monthlyExpenses).forEach(month => {
      const filteredExpenses = monthlyExpenses[month];
      const totalExpenses = filteredExpenses.reduce((sum: number, expense: any) => sum + expense.amount, 0);
      const numberOfExpenses = filteredExpenses.length;

      const report = {
        accountId: accountId,
        month: month + "-01T00:00:00Z", // Ensure the month is in 'YYYY-MM-DDTHH:mm:ssZ' format
        totalExpenses: totalExpenses,
        numberOfExpenses: numberOfExpenses
      };

      this.reportService.createOrUpdateReport(report).subscribe(() => {
        console.log(`Monthly report for ${month} created or updated successfully for account ${accountId}`);
      }, error => {
        console.error('Error creating or updating monthly report: ', error);
      });
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
    this.expenseForm.reset({ date: new Date().toISOString().split('T')[0] });
  }

  closeAddExpenseModal(): void {
    this.showAddExpenseModal = false;
    this.currentAccountId = null;
  }

  openAddFundsModal(accountId: number): void {
    this.currentAccountId = accountId;
    this.showAddFundsModal = true;
  }

  closeAddFundsModal(): void {
    this.showAddFundsModal = false;
    this.currentAccountId = null;
  }

  showPreviousReport(accountId: number): void {
    if (this.currentMonthIndex[accountId] > 0) {
      this.currentMonthIndex[accountId]--;
    }
  }

  showNextReport(accountId: number): void {
    if (this.currentMonthIndex[accountId] < this.expenseReportsFromDb[accountId].length - 1) {
      this.currentMonthIndex[accountId]++;
    }
  }

  showPreviousAccount(): void {
    if (this.currentAccountIndex > 0) {
      this.currentAccountIndex--;
    }
  }

  showNextAccount(): void {
    if (this.currentAccountIndex < this.accounts.length - 1) {
      this.currentAccountIndex++;
    }
  }

  formatMonth(month: string): string {
    const date = new Date(month);
    const options: Intl.DateTimeFormatOptions = { month: 'long' };
    return new Intl.DateTimeFormat('en-US', options).format(date);
  }
}
