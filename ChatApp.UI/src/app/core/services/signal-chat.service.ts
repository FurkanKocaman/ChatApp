import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environmentSignal } from '../../../environments/environment';
import { UserService } from './user.service';
import { Message } from '../models/message.model';
import { AuthService } from './auth.service';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SignalChatService {
  private hubConnection!: signalR.HubConnection;

  private currentSignalSubject = new BehaviorSubject<boolean>(false);
  public currentSignal$ = this.currentSignalSubject.asObservable();

  constructor(private authService: AuthService) {}

  userId: string | undefined = undefined;

  startConnection() {
    if (this.authService.getCurrentUser()) {
      this.userId = this.authService.getCurrentUser().id;
      if (this.userId) {
        this.hubConnection = new signalR.HubConnectionBuilder()
          .withUrl(environmentSignal.apiUrl)
          .withAutomaticReconnect()
          .build();
        this.hubConnection
          .start()
          .then(() => {
            this.hubConnection
              .invoke('Connect', this.userId)
              .catch((err) => console.error(err));

            this.currentSignalSubject.next(true);
          })
          .catch((err) => {
            console.error('Error while establishing connection', err);
          });
      }
    }
  }

  receiveMessage(callback: (message: Message) => void) {
    this.hubConnection.on('ReceiveMessage', callback);
  }

  userConnected(callback: (userId: string) => void) {
    this.hubConnection.on('UserConnected', callback);
  }
  userDisconnected(callback: (userId: string) => void) {
    this.hubConnection.on('UserDisconnected', callback);
  }
}
