export interface RoleUppdateRequest {
  id: string;
  name: string;
  level: number;
  colorHex?: string;
  claims: string[];
}
