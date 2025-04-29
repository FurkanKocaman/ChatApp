import { MessageGetAllResponse } from "../responses";
import { EntityDto } from "./entity.model";

export interface Message extends EntityDto {
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

export function mapMessageResponse(response: MessageGetAllResponse[]): Message[] {
  return response.map((r) => ({
    id: r.id,
    channelId: r.channelId,
    content: r.content,
    type: r.type,
    imageUrl: r.imageUrl,
    fileName: r.fileName,
    fileSize: r.fileSize,
    fileUrl: r.fileUrl,
    senderName: r.senderName,
    sendDate: r.sendDate,
    isActive: true,
    createdAt: new Date(),
    createUserId: "",
    createUserName: "",
    updateAt: undefined,
    updateUserId: undefined,
    updateUserName: undefined,
    isDeleted: false,
    deleteAt: undefined,
  }));
}
