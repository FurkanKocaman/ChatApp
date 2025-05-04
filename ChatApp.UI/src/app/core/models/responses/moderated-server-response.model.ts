import { EntityDto } from "../entities";

export interface ModeratedServerResponse extends EntityDto {
  name: string;
  iconUrl?: string;
  channelCount: number;
  memberCount: number;
  roleCount: number;
  accessType: string;
  status: string;
}
