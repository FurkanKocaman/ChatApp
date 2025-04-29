export interface MessageGetAllResponse {
  id: string;
  channelId: string;
  content: string;
  type: number;
  imageUrl?: string;
  fileUrl?: string;
  fileName?: string;
  fileSize?: number;
  senderName: string;
  sendDate: string;
}
