import { EntityDto } from "../entities";

export interface UserResponse extends EntityDto {
  firstName: string;
  lastName: string;
  fullName: string;
  displayName: string | undefined;
  avatarUrl: string | undefined;
  status: string | undefined;
  customStatus: string | undefined;
  birthOfDate: string | undefined;
  gender: string | undefined;
  lastActive: string | undefined;
}
