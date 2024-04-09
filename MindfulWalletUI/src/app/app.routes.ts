import { RouterModule} from '@angular/router';
import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { NgModule } from '@angular/core';
import { ExpensesComponent } from './components/expenses/expenses.component';
import { CalendarComponent } from './components/calendar/calendar.component';
import { AuthComponent } from './components/auth/auth.component';
import { GoalsComponent } from './components/goals/goals.component';

export const routes: Routes = [
    { path: '', component: HomeComponent}  ,
    { path:'home', component: HomeComponent },
    { path:'expenses', component: ExpensesComponent },
    { path:'calendar', component: CalendarComponent},
    { path:'goals', component: GoalsComponent},
    { path:'auth', component: AuthComponent},


];

@NgModule({
    imports: [RouterModule. forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }