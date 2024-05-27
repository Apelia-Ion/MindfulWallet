import { Component, OnInit } from '@angular/core';
import { HomeService } from '../../services/home.service';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit{
  public users: any = [];
  constructor(private api : HomeService, private authentication : AuthService) {}

  ngOnInit(): void {
    this.api.getUsers()
    .subscribe(res=>
      this.users = res
    )
  }

}
