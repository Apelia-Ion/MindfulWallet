import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import ValidateForm from '../../helpers/validateForm';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { UserStoreService } from '../../services/user-store.service';
import { FormsModule } from '@angular/forms';
import { ResetPasswordService } from '../../services/reset-password.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  public resetPasswordEmail!: string;
  public isValidEmail: boolean = false;

  constructor(
    private fb: FormBuilder,
    private auth: AuthService, 
    private router: Router,
    private userStore: UserStoreService,
    private resetService: ResetPasswordService
  ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [
        Validators.required,
        Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&]).{8,}$/) 
      ]]
    });
  }

  onLogin() {
    if (this.loginForm.valid) {
      console.log(this.loginForm.value);
      this.auth.login(this.loginForm.value)
        .subscribe({
          next: (res) => {
            alert(res.message);
            this.loginForm.reset();
            this.auth.storeToken(res.accessToken);
            this.auth.storeRefreshToken(res.refreshToken);
            const tokenPayload = this.auth.decodeToken();
            this.userStore.setFullNameForStore(tokenPayload.name);
            this.userStore.setRoleForStore(tokenPayload.role);
            this.router.navigate(['home']);
          },
          error: (err) => {
            alert(err.error.message);
          }
        });
    } else {
      console.log("Form is not valid");
      ValidateForm.validateFormFields(this.loginForm);
      alert("Your form is invalid");
    }
  }

  openModal(event: Event) {
    event.preventDefault();
  }

  checkValidEmail(event: any) {
    const value = event.target ? event.target.value : event;
    const pattern = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,3}$/; 
    this.isValidEmail = pattern.test(value);
  }

  sendResetEmail() {
    if(this.isValidEmail) {
      console.log(this.resetPasswordEmail);
      // Store the email temporarily before resetting the field
      const emailToSend = this.resetPasswordEmail;
      this.resetPasswordEmail = '';
      const buttonRef = document.getElementById("closeModalbtn");
      buttonRef?.click();
      
      // API Call pentru resetarea parolei
      this.resetService.sendResetPasswordLink(emailToSend)
        .subscribe({
          next: (res) => {
            alert('Success: Reset Success!');
            const buttonRef = document.getElementById("closeBtn");
            buttonRef?.click();
          },
          error: (err) => {
            alert('ERROR: Something went wrong!');
          }
        });
    } 
  }
  
  
}
