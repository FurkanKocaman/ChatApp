import { EntityDto } from "../entities";

export interface RoleDetailsResponse extends EntityDto {
  name: string;
  userCount: number;
  level: number;
  accessibleChannelCount: number;
  permissions: string[];
}
