import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Channel } from '../../../core/models/channel.model';
import { UserService } from '../../../core/services/user.service';
import { ChannelService } from '../../../core/services/channel.service';
import { User } from '../../../core/models/user.model';
import { AuthService } from '../../../core/services/auth.service';
import { environment } from '../../../../environments/environment.development';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-channel-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './channel-list.component.html',
  styleUrl: './channel-list.component.css',
})
export class ChannelListComponent implements OnInit {
  isListExpanded: boolean = false;
  channels: Channel[] = [];
  isChannelCreating: boolean = false;

  user: User | undefined = undefined;
  envImageUrl: string = '';

  constructor(
    private userService: UserService,
    private channelService: ChannelService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.user = this.authService.getCurrentUser();
    this.envImageUrl = environment.wwwrootUrl + this.user.profileImageUrl;

    this.userService.getUserChannels().subscribe({
      next: (res) => {
        this.channels = res;
      },
      error: (err) => {
        console.error(err);
      },
    });
  }
  setCurrentChannel(channel: Channel): void {
    this.channelService.setCurrentChannel(channel);
  }

  toggleChannelCreating() {
    this.isChannelCreating = !this.isChannelCreating;
  }

  expandList(): void {
    this.isListExpanded = !this.isListExpanded;
  }
  logout(): void {
    localStorage.clear();
    this.router.navigate(['auth/login']);
  }
}
