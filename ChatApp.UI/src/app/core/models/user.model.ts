export interface User {
  id: string;
  firstname: string;
  lastname: string;
  userName: string;
  about: string;
  email: string;
  isOnline: boolean;
  profileImageUrl?: string;
}
