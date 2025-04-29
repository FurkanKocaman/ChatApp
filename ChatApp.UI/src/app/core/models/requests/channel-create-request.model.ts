export interface ChannelCreateRequest {
  serverId: string;
  name: string;
  description?: string;
  iconUrl?: string;
  channeltype: number;
}
