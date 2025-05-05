import { Injectable } from "@angular/core";
import * as signalR from "@microsoft/signalr";
import { UserService } from "./user.service";
import { BehaviorSubject, Subscription } from "rxjs";
import { Message, User } from "../models/entities";
import { environmentSignal } from "../../../environments/environment.development";
import { ServerService } from "./server.service";
import { MessageGetAllResponse } from "../models/responses";

@Injectable({
  providedIn: "root",
})
export class SignalChatService {
  private hubConnection!: signalR.HubConnection;

  private currentSignalSubject = new BehaviorSubject<boolean>(false);
  public currentSignal$ = this.currentSignalSubject.asObservable();

  constructor(private userService: UserService, private serverService: ServerService) {}

  user: User | null = null;
  userId: string | undefined = undefined;

  startConnection() {
    this.userService.user$.subscribe((res) => {
      this.user = res;
    });
    if (this.user) {
      if (this.user) {
        this.userId = this.user.id;
        this.hubConnection = new signalR.HubConnectionBuilder()
          .withUrl(environmentSignal.apiUrl)
          .withAutomaticReconnect()
          .build();
        this.hubConnection
          .start()
          .then(() => {
            // this.hubConnection.invoke("Connect", this.userId).catch((err) => console.error(err));
            this.currentSignalSubject.next(true);
            this.hubConnection.on("UserConnected", (userId: string) => {});

            this.hubConnection.on("UserDisconnected", (userId: string) => {});
          })
          .catch((err) => {
            console.error("Error while establishing connection", err);
          });
      }
    }
  }
  JoinServers() {
    this.userService.user$.subscribe({
      next: (user) => {
        this.serverService.getUserJoinedServers().subscribe({
          next: (res) => {
            if (user && res) {
              this.hubConnection
                .invoke(
                  "JoinServers",
                  res.map((r) => r.id),
                  user.id
                )
                .catch((err) => console.error("JoinChannel error:", err));
            }
          },
          error: (err) => {
            console.error(err);
          },
        });
      },
      error: (err) => {
        console.error(err);
      },
    });
  }
  joinChannel(channelId: string) {
    this.userService.user$.subscribe({
      next: (res) => {
        console.log("Joined channel");
        this.hubConnection
          .invoke("JoinChannel", channelId)
          .catch((err) => console.error("JoinChannel error:", err));
      },
      error: (err) => {
        console.error(err);
      },
    });
  }
  leaveChannel(channelId: string) {
    this.userService.user$.subscribe({
      next: (res) => {
        this.hubConnection
          .invoke("LeaveChannel", channelId)
          .catch((err) => console.error("JoinChannel error:", err));
      },
      error: (err) => {
        console.error(err);
      },
    });
  }

  receiveMessage(callback: (message: Message) => void) {
    this.hubConnection.on("ReceiveMessage", callback);
  }
  receiveChannelMessage(callback: (message: MessageGetAllResponse) => Subscription) {
    return this.hubConnection.on("ReceiveChannelMessage", callback);
  }

  userConnected(callback: (userId: string) => void) {
    this.hubConnection.on("UserConnected", callback);
  }
  userDisconnected(callback: (userId: string) => void) {
    this.hubConnection.on("UserDisconnected", callback);
  }
}
