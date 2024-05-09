import { RouterModule} from '@angular/router';
import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { NgModule } from '@angular/core';
import { ExpensesComponent } from './components/expenses/expenses.component';
import { CalendarComponent } from './components/calendar/calendar.component';
import { GoalsComponent } from './components/goals/goals.component';
import { LoginComponent } from './components/login/login.component';
import { SignupComponent } from './components/signup/signup.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

export const routes: Routes = [
    { path: '', component: HomeComponent}  ,
    { path:'home', component: HomeComponent },
    { path:'expenses', component: ExpensesComponent },
    { path:'calendar', component: CalendarComponent},
    { path:'goals', component: GoalsComponent},
    { path:'login', component: LoginComponent},
    { path:'signup', component: SignupComponent}


];

@NgModule({
    imports: [RouterModule. forRoot(routes), FormsModule, ReactiveFormsModule, CommonModule],
    exports: [RouterModule, FormsModule, ReactiveFormsModule]
})
export class AppRoutingModule { }