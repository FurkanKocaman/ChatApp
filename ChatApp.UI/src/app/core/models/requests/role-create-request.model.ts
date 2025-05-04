export interface RoleCreateRequest {
  name: string;
  serverId: string;
  level: number;
  colorHex?: string;
  claims: string[];
}
