export default interface AuthState {
    userName: string | null;
    role: string;
    claims: string[];
    isAuthenticated: boolean;
    token: string | null;
    refreshToken: string | null;
    returnUrl: string;
}
