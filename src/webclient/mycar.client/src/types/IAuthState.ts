export default interface AuthState {
    userName: string;
    firstName: string | null;
    lastName: string | null;
    email: string | null;
    role: string;
    claims: string[];
    isAuthenticated: boolean;
    token: string | null;
    refreshToken: string | null;
    returnUrl: string;
}
