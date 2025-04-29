import { UserResponse } from "../responses";
import { EntityDto } from "./entity.model";

export interface User extends EntityDto {
  firstName: string;
  lastName: string;
  fullName: string;
  displayName: string | undefined;
  avatarUrl: string | undefined;
  status: string | undefined;
  customStatus: string | undefined;
  birthOfDate: Date | undefined;
  gender: string | undefined;
  lastActive: Date | undefined;
}

export function mapUserResponse(response: UserResponse): User {
  return {
    id: response.id,
    firstName: response.firstName,
    lastName: response.lastName,
    fullName: response.fullName,
    displayName: response.displayName,
    avatarUrl: response.avatarUrl,
    status: response.status,
    customStatus: response.customStatus,
    birthOfDate: response.birthOfDate ? new Date(response.birthOfDate) : undefined,
    gender: response.gender,
    lastActive: response.lastActive ? new Date(response.lastActive) : undefined,
    isActive: response.isActive,
    createdAt: new Date(response.createdAt),
    createUserId: response.createUserId,
    createUserName: response.createUserName,
    updateAt: response.updateAt,
    updateUserId: response.updateUserId,
    updateUserName: response.updateUserName,
    isDeleted: response.isDeleted,
    deleteAt: response.deleteAt,
  };
}
