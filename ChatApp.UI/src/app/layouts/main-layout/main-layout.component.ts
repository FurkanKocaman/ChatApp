import { Component, OnInit } from '@angular/core';
import { ChannelListComponent } from '../../features/channel/channel-list/channel-list.component';
import { ChatListComponent } from '../../features/chat/chat-list/chat-list.component';
import { ChatRoomComponent } from '../../features/chat/chat-room/chat-room.component';
import { ChannelService } from '../../core/services/channel.service';
import { CommonModule } from '@angular/common';
import { ChatUsersComponent } from '../../features/chat/chat-users/chat-users.component';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [
    ChannelListComponent,
    ChatListComponent,
    ChatRoomComponent,
    CommonModule,
    ChatUsersComponent,
  ],
  templateUrl: './main-layout.component.html',
  styleUrl: './main-layout.component.css',
})
export class MainLayoutComponent implements OnInit {
  isCurrentChannel: boolean = false;
  constructor(private channelService: ChannelService) {}

  ngOnInit(): void {
    this.channelService.currentChannel$.subscribe((channel) => {
      if (channel) {
        this.isCurrentChannel = true;
      } else {
        this.isCurrentChannel = false;
      }
    });
  }
}
