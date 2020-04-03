import { Component, EventEmitter, Output } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  model: any = {};

  @Output()
  cancel = new EventEmitter<any>();

  constructor(private authService: AuthService) {}

  register(): void {
    this.authService.register(this.model).subscribe(
      response => {
        console.log('registered');
      },
      error => {
        console.log(error);
      }
    );
  }

  onCancel(): void {
    this.cancel.emit(false);
  }
}
