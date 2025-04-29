export interface RegisterRequest {
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
  password: string;
  birthOfDate: Date | undefined;
  gender: boolean | undefined;
}
