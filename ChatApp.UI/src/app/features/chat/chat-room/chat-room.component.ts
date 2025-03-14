import {
  Component,
  ElementRef,
  HostListener,
  OnInit,
  ViewChild,
} from '@angular/core';
import { ChatUsersComponent } from '../chat-users/chat-users.component';
import { NavbarComponent } from '../../../shared/components/navbar/navbar.component';
import { Message } from '../../../core/models/message.model';
import { ChatService } from '../../../core/services/chat.service';
import { MessageSend } from '../../../core/models/send-message.model';
import { FormsModule } from '@angular/forms';
import { environment } from '../../../../environments/environment.development';
import { concat } from 'rxjs';
import { SignalChatService } from '../../../core/services/signal-chat.service';
import { CommonModule } from '@angular/common';
import { User } from '../../../core/models/user.model';
import { ChannelService } from '../../../core/services/channel.service';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { UserService } from '../../../core/services/user.service';

@Component({
  selector: 'app-chat-room',
  standalone: true,
  imports: [NavbarComponent, FormsModule, CommonModule],
  templateUrl: './chat-room.component.html',
  styleUrl: './chat-room.component.css',
})
export class ChatRoomComponent implements OnInit {
  @ViewChild('chatContainer') chatContainer!: ElementRef;

  currentChatId: string = '';
  isCurrentChannel: boolean = false;

  isListExpanded: boolean = true;

  messages: Message[] = [];
  users: User[] = [];
  isLoading: boolean = false;

  messageToSend: MessageSend = {
    chatId: '',
    content: '',
    fileName: '',
    fileUrl: '',
    fileSize: 0,
  };

  fileToUpload: File | undefined = undefined;
  fileNameToUpload: string = '';

  imageApiUrl: string = environment.wwwrootUrl;

  constructor(
    private chatService: ChatService,
    private signalChatService: SignalChatService,
    private channelService: ChannelService,
    private userService: UserService
  ) {}
  ngOnInit(): void {
    this.channelService.currentChannel$.subscribe((channel) => {
      if (channel) {
        this.isCurrentChannel = true;
        this.channelService.getUsersByChannelId(channel.id).subscribe({
          next: (res) => {
            this.users = res;
            this.userService.setCurrentChannelUsers(res);
          },
          error: (err) => {
            console.error(err);
          },
        });
      } else {
        this.isCurrentChannel = false;
      }
    });

    this.isLoading = true;
    this.chatService.currentChat$.subscribe((chat) => {
      if (chat) {
        this.messageToSend.chatId = chat.id;
        this.currentChatId = chat.id;

        this.chatService.getMessages(chat.id, undefined).subscribe({
          next: (res) => {
            if (res) {
              this.messages = res.reverse();
            } else {
              this.messages = [];
            }
            this.isLoading = false;
            this.scrollToBottom();
          },
        });
      }
    });

    this.signalChatService.currentSignal$.subscribe((value) => {
      if (value) {
        this.signalChatService.receiveMessage((message) => {
          if (this.currentChatId == message.chat_Id) {
            this.messages.push(message);
            this.scrollToBottom();
          }
        });
      }
    });
  }

  sendMessage(): void {
    if (this.fileToUpload) {
      console.log('File uploading started');
      this.chatService.uploadChatFile(this.fileToUpload).subscribe({
        next: (res) => {
          console.log('File uploading completed');
          this.messageToSend.fileName = res.fileName;
          this.messageToSend.fileSize = res.fileSize;
          this.messageToSend.fileUrl = res.fileUrl;

          this.chatService.sendMessage(this.messageToSend).subscribe({
            next: (res) => {
              this.messages.push(res);
              this.messageToSend.content = '';
              this.removeSelectedFile();
              this.scrollToBottom();
            },
            error: (err) => {
              console.error(err);
            },
          });
        },
      });
    } else {
      if (!this.messageToSend.content.trim()) {
        return;
      }
      this.chatService.sendMessage(this.messageToSend).subscribe({
        next: (res) => {
          this.messages.push(res);
          this.messageToSend.content = '';
          this.scrollToBottom();
        },
        error: (err) => {
          console.error(err);
        },
      });
    }
  }

  loadMessages(): void {
    if (this.messages.length) {
      this.isLoading = true;
      this.chatService
        .getMessages(this.currentChatId, this.messages[0].send_Date)
        .subscribe({
          next: (res) => {
            if (res) {
              this.messages = [...res.reverse(), ...this.messages];
            }

            this.isLoading = false;
          },
          error: (err) => {
            console.error(err);
          },
        });
    }
  }

  getMessageSender(userId: string): string {
    const user: User | undefined = this.users.find((user) => user.id == userId);
    if (user) {
      return user.userName;
    }
    return '';
  }

  getSenderProfileImageUrl(userId: string): string | undefined {
    const user: User | undefined = this.users.find((user) => user.id == userId);
    if (user) {
      const imageUrl = this.imageApiUrl + user.profileImageUrl;
      return imageUrl;
    }
    return '';
  }

  onFileSelected(event: Event): void {
    const target = event.target as HTMLInputElement;
    if (target.files && target.files.length > 0) {
      this.fileToUpload = target.files[0];
      this.fileNameToUpload = this.fileToUpload.name;
    }
  }

  onScroll() {
    if (this.chatContainer.nativeElement.scrollTop <= 100 && !this.isLoading) {
      this.loadMessages();
    }
  }

  @HostListener('window:paste', ['$event'])
  onPaste(event: ClipboardEvent) {
    const items = event.clipboardData?.items;
    if (!items) return;

    for (const item of Array.from(items)) {
      if (item.type.startsWith('image/')) {
        const blob = item.getAsFile();
        if (blob) {
          this.fileToUpload = blob;
          this.fileNameToUpload = blob.name;
        }
      }
    }
  }

  removeSelectedFile(): void {
    this.fileToUpload = undefined;
    this.fileNameToUpload = '';

    this.messageToSend.fileName = '';
    this.messageToSend.fileSize = 0;
    this.messageToSend.fileUrl = '';
  }
  downloadFile(fileUrl: string, fileName: string) {
    if (fileUrl) {
      const link = document.createElement('a');
      link.href = fileUrl;
      link.download = fileName || 'File';
      link.click();
    }
  }
  public toggleListExpanded(): void {
    this.isListExpanded = !this.isListExpanded;
  }

  public scrollToBottom() {
    setTimeout(() => {
      this.chatContainer.nativeElement.scrollTop =
        this.chatContainer.nativeElement.scrollHeight;
    }, 10);
  }
}
