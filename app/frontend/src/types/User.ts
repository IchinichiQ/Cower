export enum UserRole {
  USER = 'User',
  ADMIN = 'Admin'
}

export interface User {
  id: number;
  name: string;
  surname: string;
  email: string;
  phone: string;
  role: UserRole;
}
