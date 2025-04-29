import { GetUserJoinedServersQueryResponse } from "../responses";
import { EntityDto } from "./entity.model";

export interface Server extends EntityDto {
  name: string;
  description?: string;
  iconUrl?: string;
  ownerId?: string;
  ownerName?: string;
}

export function mapServerResponse(response: GetUserJoinedServersQueryResponse[]): Server[] {
  return response.map((r) => ({
    id: r.id,
    name: r.name,
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
