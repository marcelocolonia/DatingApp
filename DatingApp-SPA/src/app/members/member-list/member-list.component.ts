import { Component, OnInit } from '@angular/core';
import { UserService } from '../../_services/user.service';
import { IUser } from '../../_models/user';
import { AlertService } from '../../_services/alert.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  users: IUser[] = [];

  constructor(
    private usersService: UserService,
    private alertService: AlertService
  ) {}

  ngOnInit(): void {
    this._loadUsers();
  }

  private _loadUsers(): void {
    this.usersService.getAll().subscribe(
      (users: IUser[]) => {
        this.users = users;
      },
      error => {
        this.alertService.error(error);
      }
    );
  }
}
