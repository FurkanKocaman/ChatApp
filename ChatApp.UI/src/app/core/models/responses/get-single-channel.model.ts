import { EntityDto } from "../entities";

export interface SingleChannelModel extends EntityDto {
  name: string;
  description?: string;
  isPublic: boolean;
  roleIds: string[];
  iconUrl?: string;
  channelType: number;
}
