import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IUser } from 'src/app/_models/user';
import { NgForm } from '@angular/forms';
import { UserService } from 'src/app/_services/user.service';
import { AlertService } from 'src/app/_services/alert.service';
import { AuthService } from 'src/app/_services/auth.service';
import { Photo } from 'src/app/_models/photo';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css'],
})
export class MemberEditComponent implements OnInit {
  user: IUser;

  @ViewChild(NgForm) editForm: NgForm;

  @HostListener('window:beforeunload', ['$event']) onUnload(event: Event) {
    if (this.editForm.dirty) {
      event.returnValue = true;
    }
  }

  constructor(
    private router: ActivatedRoute,
    private authService: AuthService,
    private alert: AlertService
  ) {}

  ngOnInit(): void {
    this.router.data.subscribe((data) => {
      this.user = data.user;
    });
  }

  updateUser(): void {
    this.authService.updateProfile(this.user).subscribe(
      (result) => {
        this.alert.success('Saved changes');
        this.editForm.reset(this.user);
      },
      (error) => {
        this.alert.error(error);
      }
    );
  }

  mainPhotoSet(photo: Photo): void {
    this.user.photoUrl = photo.url;
  }
}
