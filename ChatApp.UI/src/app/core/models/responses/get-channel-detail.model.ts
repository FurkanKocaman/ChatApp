import { EntityDto } from "../entities";

export interface GetChannelDetail extends EntityDto {
  name: string;
  access: string;
  messageCount: number;
  mostActiveUser: string;
  status: string;
}
