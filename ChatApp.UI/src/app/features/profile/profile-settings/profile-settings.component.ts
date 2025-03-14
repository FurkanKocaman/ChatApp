import { Component, OnInit } from '@angular/core';
import { User } from '../../../core/models/user.model';
import { AuthService } from '../../../core/services/auth.service';
import { FormsModule } from '@angular/forms';
import { environment } from '../../../../environments/environment.development';
import { UserService } from '../../../core/services/user.service';
import { ToastService } from '../../../core/services/toast.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-profile-settings',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './profile-settings.component.html',
  styleUrl: './profile-settings.component.css',
})
export class ProfileSettingsComponent implements OnInit {
  currentUser: User = {
    id: '',
    firstname: '',
    lastname: '',
    userName: '',
    email: '',
    isOnline: false,
    about: '',
  };
  imageUrl: string | ArrayBuffer | null = '';

  selectedProfileImage: File | undefined = undefined;
  isUsernameAvaliable: boolean = true;

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.currentUser = this.authService.getCurrentUser();

    this.imageUrl = environment.wwwrootUrl + this.currentUser.profileImageUrl;
  }
  updateUser(): void {
    if (this.isUsernameAvaliable) {
      if (this.selectedProfileImage) {
        this.userService
          .uploadProfilePicture(this.selectedProfileImage, this.currentUser.id)
          .subscribe((res) => {
            console.log('UPLOADPROFİLEIMAGERES', res);
          });
      }
      this.userService.uploadUser(this.currentUser).subscribe({
        next: (res) => {
          console.log('Updated user', res);
          this.toastService.showToast(
            'Changes updated successfully.',
            'success'
          );
        },
        error: (err) => {
          console.error(err);
          this.toastService.showToast(
            'An error occured during saving.',
            'error'
          );
        },
      });
    }
  }

  checkUsername(username: string) {
    this.userService.checkUsername(username).subscribe({
      next: (res) => {
        console.log('USERNAMERES', res);
        this.isUsernameAvaliable = res;
      },
      error: (err) => {
        this.isUsernameAvaliable = false;
        console.error(err);
      },
    });
  }

  onFileSelected(event: Event): void {
    console.log('On file selected');
    const target = event.target as HTMLInputElement;
    if (target.files && target.files.length > 0) {
      console.log('Target files', target.files[0]);
      this.selectedProfileImage = target.files[0];

      const reader = new FileReader();
      reader.onload = () => {
        this.imageUrl = reader.result;
      };
      reader.readAsDataURL(this.selectedProfileImage);
    }
  }
}
