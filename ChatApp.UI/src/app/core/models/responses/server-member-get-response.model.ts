export interface ServerMemberGetAllResponse {
  id: string;
  serverId: string;
  userId: string;
  fullName: string;
  displayName?: string;
  avatarUrl?: string;
}
