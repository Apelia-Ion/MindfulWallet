import { Component, OnInit } from '@angular/core';
import { HomeService } from '../../services/home.service';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { UserStoreService } from '../../services/user-store.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  public users: any = [];
  public userName: string = "";
  public userRole: string = "";
  public showUsers: boolean = false;

  constructor(private api: HomeService, private authentication: AuthService, private userStore: UserStoreService) {}

  ngOnInit(): void {
    this.userStore.getFullNameFromStore().subscribe(val => {
      let fullNameFromToken = this.authentication.getFullNameFromToken();
      this.userName = val || fullNameFromToken;
    });
    this.userStore.getRoleFromStore().subscribe(val => {
      let role = this.authentication.getRoleFromToken();
      this.userRole = val || role;
    });
  }

  toggleUsers(): void {
    this.showUsers = !this.showUsers;
    if (this.showUsers) {
      this.api.getUsers().subscribe(res => this.users = res);
    }
  }
}
