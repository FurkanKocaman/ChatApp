export interface Message {
  id: string;
  chat_Id: string;
  sender_Id: string;
  messageType: string;
  content: string;
  fileName?: string;
  fileUrl?: string;
  fileSize?: number;
  send_Date: Date;
}
