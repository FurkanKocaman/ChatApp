import { ChannelResponse } from "../responses";
import { EntityDto } from "./entity.model";

export interface Channel extends EntityDto {
  name: string;
  description?: string;
  iconUrl?: string;
  type: number;
  serverId: string;
}

export function mapChannelResponse(response: ChannelResponse[]): Channel[] {
  return response.map((r) => ({
    id: r.id,
    name: r.name,
    description: r.description,
    type: r.channelType,
    serverId: "",
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
