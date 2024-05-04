export enum UserRole {
  USER = 'USER',
  ADMIN = 'ADMIN'
}

export interface User {
  id: number;
  name: string;
  surname: string;
  email: string;
  phone: string;
  role: UserRole;
}
