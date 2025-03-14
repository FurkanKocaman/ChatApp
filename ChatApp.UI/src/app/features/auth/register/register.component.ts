import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { RegisterRequest } from '../../../core/models/register-req.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [RouterLink, FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  registerReq: RegisterRequest = {
    firstname: '',
    lastname: '',
    username: '',
    email: '',
    password: '',
  };

  register(): void {}
}
