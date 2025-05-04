export interface ChannelUpdateRequest {
  id: string;
  name: string;
  description?: string;
  isPublic: boolean;
  roleIds: string[];
}
