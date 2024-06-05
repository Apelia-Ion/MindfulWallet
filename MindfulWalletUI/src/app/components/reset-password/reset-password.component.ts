import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ResetPassword } from '../../models/reset-password.model';
import { ActivatedRoute, Router } from '@angular/router';
import ValidateForm from '../../helpers/validateForm';
import { ResetPasswordService } from '../../services/reset-password.service';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css'] 
})
export class ResetPasswordComponent implements OnInit {
  resetPasswordForm!: FormGroup;
  emailToReset!: string;
  emailToken!: string;
  resetPasswordObj = new ResetPassword();

  constructor(private fb: FormBuilder, private activatedRoute : ActivatedRoute, private resetService: ResetPasswordService, private router: Router) {} 

  ngOnInit(): void {
    this.resetPasswordForm = this.fb.group({
      password: ['', [
        Validators.required,
        Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&]).{8,}$/)
      ]],
      confirmPassword: ['', Validators.required]
    }, {
      validator: this.mustMatch('password', 'confirmPassword') 
    });

    this.activatedRoute.queryParams.subscribe(val =>{
      this.emailToReset = val['email'];
      let uriToken = val['code'];


      this.emailToken = uriToken.replace(/ /g,'+');  //to remove spaces
      console.log(this.emailToken);
      console.log(this.emailToReset);
    })
  }

  mustMatch(password: string, confirmPassword: string) {
    return (formGroup: FormGroup) => {
      const passwordControl = formGroup.controls[password];
      const confirmPasswordControl = formGroup.controls[confirmPassword];

      if (confirmPasswordControl.errors && !confirmPasswordControl.errors['mustMatch']) {
        // Return if another validator has already found an error on the matchingControl
        return;
      }

      // Set error on matchingControl if validation fails
      if (passwordControl.value !== confirmPasswordControl.value) {
        confirmPasswordControl.setErrors({ mustMatch: true });
      } else {
        confirmPasswordControl.setErrors(null);
      }
    };
  }

  reset(){
    if(this.resetPasswordForm.valid)
      {
        this.resetPasswordObj.email = this.emailToReset;
        this.resetPasswordObj.newPassword = this.resetPasswordForm.value.password;
        this.resetPasswordObj.confirmPassword = this.resetPasswordForm.value.confirmPassword;
        this.resetPasswordObj.emailToken=this.emailToken;

        this.resetService.resetPassword(this.resetPasswordObj)
        .subscribe({
            next: (res) => {
                alert('Password Reset Successfully');
                this.router.navigate(['/']);
            },
            error: (err) => {
                alert('There was an error resetting your password. Please try again.');
            }
        });
        
      }
      else 
      {
        ValidateForm.validateFormFields(this.resetPasswordForm);
      }
  }
}
