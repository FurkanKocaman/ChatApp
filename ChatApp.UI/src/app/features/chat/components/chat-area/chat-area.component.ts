import {
  AfterContentChecked,
  AfterViewChecked,
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  NgZone,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  ViewChild,
} from "@angular/core";
import { ThemeService } from "../../../../core/services/theme.service";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { ServerService } from "../../../../core/services/server.service";
import { ChannelService } from "../../../../core/services/channel.service";
import { MessageService } from "../../../../core/services/message.service";
import { mapMessageResponse, Message } from "../../../../core/models/entities";
import { MessageSendRequest } from "../../../../core/models/requests";
import { SignalChatService } from "../../../../core/services/signal-chat.service";
import { BehaviorSubject, fromEvent, Subscription } from "rxjs";

@Component({
  selector: "app-chat-area",
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: "./chat-area.component.html",
  styleUrl: "./chat-area.component.css",
})
export class ChatAreaComponent implements OnInit, AfterViewInit, OnChanges, AfterViewChecked {
  @ViewChild("chatContainer") private chatContainer!: ElementRef;

  @Output() open = new EventEmitter<void>();

  constructor(
    private themeService: ThemeService,
    private channelService: ChannelService,
    private messageService: MessageService,
    private signalService: SignalChatService,
    private zone: NgZone,
    private cdRef: ChangeDetectorRef
  ) {}

  isDark: boolean = this.themeService.isDarkMode();

  private scrollPending: boolean = false;

  isMicrophoneOpen: boolean = false;
  isHeadsetOpen: boolean = false;

  messages: Message[] = [];

  request: MessageSendRequest = {
    channelId: "",
    content: "",
    type: 0,
  };

  ngOnInit(): void {
    this.channelService.channel$.subscribe({
      next: (res) => {
        if (res) {
          this.request.channelId = res.id;
          this.messageService.getMessages(res!.id).subscribe((res) => {
            this.messages = res;
            this.scrollToBottom();
          });
          this.signalService.receiveChannelMessage((message) => {
            this.messages.push(mapMessageResponse([message])[0]);
            this.scrollToBottom();
          });
        }
      },
    });
  }

  ngAfterViewInit() {
    // const messageContainers = document.querySelectorAll(".message-container");
    // messageContainers.forEach((el) => {
    //   el.classList.add("notranslate");
    //   el.setAttribute("translate", "no");
    // });
  }

  ngAfterViewChecked(): void {
    if (this.scrollPending) {
      this.scrollToBottom();
      this.scrollPending = false;
    }
  }

  ngOnChanges() {
    this.scrollToBottom();
  }

  sendMessage() {
    if (this.request.channelId) {
      this.messageService.sendMessage(this.request).subscribe((res) => {
        this.request.content = "";
        this.scrollPending = true;
        this.cdRef.detectChanges();
      });
    }
  }

  toggleTheme(): void {
    this.themeService.toggleTheme();
    this.isDark = this.themeService.isDarkMode();
  }

  toggleMicrophone(): void {
    this.isMicrophoneOpen = !this.isMicrophoneOpen;
  }

  toggleHeadset(): void {
    this.isHeadsetOpen = !this.isHeadsetOpen;
  }

  isSameDay(date1: string, date2: string): boolean {
    const date11 = new Date(date1);
    const date12 = new Date(date2);

    return (
      date11.getFullYear() === date12.getFullYear() &&
      date11.getMonth() === date12.getMonth() &&
      date11.getDate() === date12.getDate()
    );
  }
  private scrollToBottom(): void {
    setTimeout(() => {
      this.zone.runOutsideAngular(() => {
        try {
          const element = this.chatContainer.nativeElement;
          element.scrollTop = element.scrollHeight;
        } catch (err) {
          console.error("Scroll error:", err);
        }
      });
    }, 10);
  }

  openSidebar() {
    this.open.emit();
  }
}
