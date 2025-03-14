import { Component, OnInit } from '@angular/core';
import { ChatService } from '../../../core/services/chat.service';
import { ChannelService } from '../../../core/services/channel.service';
import { Chat } from '../../../core/models/chat.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-chat-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './chat-list.component.html',
  styleUrl: './chat-list.component.css',
})
export class ChatListComponent implements OnInit {
  chats: Chat[] = [];

  isListExpanded: boolean = false;

  constructor(
    private chatService: ChatService,
    private channelService: ChannelService
  ) {}

  ngOnInit(): void {
    this.channelService.currentChannel$.subscribe((channel) => {
      if (channel) {
        this.chatService.getChats(channel.id).subscribe({
          next: (res) => {
            this.chats = res;
            this.isListExpanded = true;
          },
        });
      }
    });
  }
  setCurrentChat(chat: Chat): void {
    this.chatService.setCurrentChat(chat);
  }

  toggleListExpanded(): void {
    this.isListExpanded = !this.isListExpanded;
  }
}
