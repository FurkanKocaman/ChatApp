export interface EntityDto {
  id: string;
  isActive: boolean;
  createdAt: Date;
  createUserId: string;
  createUserName: string;
  updateAt: Date | undefined;
  updateUserId: string | undefined;
  updateUserName: string | undefined;
  isDeleted: boolean;
  deleteAt: Date | undefined;
}
