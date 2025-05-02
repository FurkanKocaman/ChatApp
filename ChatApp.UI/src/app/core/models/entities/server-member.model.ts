import { ServerMemberGetAllResponse } from "../responses";
import { EntityDto } from "./entity.model";

export interface ServerMember extends EntityDto {
  serverId: string;
  userId: string;
  nickname?: string;
  fullName: string;
  displayName?: string;
  avatarUrl?: string;
  roles: string[];
}

export function mapServerMemberResponse(response: ServerMemberGetAllResponse[]): ServerMember[] {
  return response.map((p) => ({
    id: p.id,
    serverId: p.serverId,
    userId: p.userId,
    fullName: p.fullName,
    displayName: p.displayName,
    avatarUrl: p.avatarUrl,
    roles: p.roles,
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
