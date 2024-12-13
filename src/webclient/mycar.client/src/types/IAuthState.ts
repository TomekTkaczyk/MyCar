import type IUser from "./IUser";

export default interface AuthState extends IUser {
  token?: string;
  refreshToken?: string;
  returnUrl?: string;
  isAuthenticated?: boolean;
}
