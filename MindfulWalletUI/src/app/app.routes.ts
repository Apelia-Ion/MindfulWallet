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
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgToastModule } from 'ng-angular-popup';
import { authGuard } from './guards/auth.guard';
import { StartComponent } from './components/start/start.component';
import { tokenInterceptor } from './interceptors/token.interceptor';

export const routes: Routes = [
    { path: '', component: StartComponent }  ,
    { path:'home', component: HomeComponent, canActivate:[authGuard] },
    { path:'expenses', component: ExpensesComponent },
    { path:'calendar', component: CalendarComponent},
    { path:'goals', component: GoalsComponent},
    { path:'login', component: LoginComponent},
    { path:'signup', component: SignupComponent}


];

@NgModule({
    imports: [
        RouterModule. forRoot(routes), 
        FormsModule, ReactiveFormsModule, 
        CommonModule, 
        HttpClientModule,
        NgToastModule
    ],
    exports: [RouterModule, FormsModule, ReactiveFormsModule],
    providers: [
        { provide: HTTP_INTERCEPTORS, useValue: tokenInterceptor, multi: true }
      ]
})
export class AppRoutingModule { }