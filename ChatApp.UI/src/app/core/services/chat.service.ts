import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Chat } from '../models/chat.model';
import { Message } from '../models/message.model';
import { MessageSend } from '../models/send-message.model';

@Injectable({
  providedIn: 'root',
})
export class ChatService {
  constructor(private httpClient: HttpClient) {}

  private currentChatSubject = new BehaviorSubject<Chat | undefined>(undefined);
  public currentChat$ = this.currentChatSubject.asObservable();

  setCurrentChat(channel: Chat): void {
    this.currentChatSubject.next(channel);
  }
  getCurrentChat(): Chat | undefined {
    return this.currentChatSubject.value;
  }

  getChats(channelId: string): Observable<Chat[]> {
    return this.httpClient.get<Chat[]>(
      `${environment.apiUrl}/Chat/GetChats?channelId=${channelId}`
    );
  }

  getChatById(chatId: string): Observable<Chat> {
    return this.httpClient.get<Chat>(
      `${environment.apiUrl}/Chat/GetChatById?chatId=${chatId}`
    );
  }

  // createChat(createChat: CreateChat): Observable<Chat> {
  //   return this.httpClient.post<Chat>(
  //     environment.apiUrl + '/Chat/Create',
  //     createChat
  //   );
  // }

  // deleteChat(deleteChat: DeleteChat): Observable<Chat> {
  //   return this.httpClient.delete<Chat>(
  //     `${environment.apiUrl}/Chat/DeleteChat`,
  //     { body: deleteChat }
  //   );
  // }

  getMessages(chatId: string, lastMessageDate?: Date): Observable<Message[]> {
    let params = new HttpParams().set('chatId', chatId);

    if (lastMessageDate) {
      const date =
        lastMessageDate instanceof Date
          ? lastMessageDate
          : new Date(lastMessageDate);

      const localDate = new Date(date.getTime() + 3 * 60 * 60 * 1000); // UTC+3 saat farkını ekle

      const formattedDate = localDate.toISOString();

      params = params.set('lastMessageDate', formattedDate);
    }

    return this.httpClient.get<Message[]>(
      `${environment.apiUrl}/Chat/GetMessages`,
      { params }
    );
  }

  sendMessage(message: MessageSend): Observable<Message> {
    return this.httpClient.post<Message>(
      `${environment.apiUrl}/Chat/SendMessage`,
      message
    );
  }

  uploadChatFile(
    file: File
  ): Observable<{ fileName: string; fileSize: number; fileUrl: string }> {
    const formData = new FormData();
    formData.append('file', file);

    return this.httpClient.post<{
      fileName: string;
      fileSize: number;
      fileUrl: string;
    }>(`${environment.apiUrl}/Chat/UploadChatFile`, formData);
  }
}
