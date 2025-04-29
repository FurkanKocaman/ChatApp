import { EntityDto } from "../entities";

export interface ChannelResponse extends EntityDto {
  name: string;
  description?: string;
  iconUrl?: string;
  channelType: number;
}
