import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormControl} from '@angular/forms';
import ValidateForm from '../../helpers/validateForm';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;

  constructor(private fb: FormBuilder, private auth: AuthService, private router: Router ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [
        Validators.required,
        Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&]).{8,}$/) 
      ]]
    });
  }

  onLogin(){
    if(this.loginForm.valid){
      console.log(this.loginForm.value)
      //send the obj to database
      this.auth.login(this.loginForm.value)
      .subscribe({
        next:(res)=>{
          alert(res.message);
          this.loginForm.reset();
          this.router.navigate(['home']);
        },
        error:(err)=>(
          alert(err.error.message)
        )
      })


    }else{
      //throw err
      console.log("Form is not valid")
      ValidateForm.validateFormFields(this.loginForm);
      alert("Your form is invalid")
    }
  }


}