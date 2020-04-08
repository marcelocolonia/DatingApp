import { Component, OnInit } from '@angular/core';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  constructor(private usersService: UserService) { }

  ngOnInit(): void {
    this.usersService.getAll().subscribe(users => {
      console.log(users);
    });
  }

}
