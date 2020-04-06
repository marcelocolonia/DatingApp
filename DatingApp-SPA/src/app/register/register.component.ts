import { Component, EventEmitter, Output } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { AlertService } from '../services/alert.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  model: any = {};

  @Output()
  cancel = new EventEmitter<any>();

  constructor(private authService: AuthService, private alert: AlertService) {}

  register(): void {
    this.authService.register(this.model).subscribe(
      response => {
        this.alert.success('Registered successfully');
      },
      error => {
        this.alert.error(error);
      }
    );
  }

  onCancel(): void {
    this.cancel.emit(false);
  }
}
