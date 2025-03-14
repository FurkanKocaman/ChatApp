import { Component, OnInit } from '@angular/core';
import { User } from '../../../core/models/user.model';
import { AuthService } from '../../../core/services/auth.service';
import { environment } from '../../../../environments/environment.development';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { ChannelService } from '../../../core/services/channel.service';
import { Channel } from '../../../core/models/channel.model';
import { Chat } from '../../../core/models/chat.model';
import { ChatService } from '../../../core/services/chat.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent implements OnInit {
  currentUser: User | undefined = undefined;
  currentChannel: Channel | undefined = undefined;
  currentChat: Chat | undefined = undefined;

  envImageUrl: string = '';
  isDropdownOpen: boolean = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private channelService: ChannelService,
    private chatService: ChatService
  ) {}

  ngOnInit(): void {
    this.envImageUrl = environment.wwwrootUrl;

    this.authService.currentUser$.subscribe((user) => {
      this.currentUser = user;
    });

    this.channelService.currentChannel$.subscribe((channel) => {
      this.currentChannel = channel;
    });
    this.chatService.currentChat$.subscribe((chat) => {
      this.currentChat = chat;
    });
  }

  toggleDropdown(): void {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  logout(): void {
    localStorage.clear();
    this.router.navigate(['/auth/login']);
  }
}
