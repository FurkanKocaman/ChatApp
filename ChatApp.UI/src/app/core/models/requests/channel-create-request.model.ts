export interface ChannelCreateRequest {
  serverId: string;
  name: string;
  description?: string;
  iconUrl?: string;
  isPublic: boolean;
  roleIds: string[];
  channeltype: number;
}
