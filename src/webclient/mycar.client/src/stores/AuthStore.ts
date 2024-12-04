import { defineStore } from 'pinia';

// import { fetchWrapper } from '@/helpers/fetch-wrapper';
import router from '@/router'
import { useAlertStore } from '@/stores/AlertStore';

import type ISignInCommand from '@/modules/auth/requests/signin-command';
import type IUser from '@/types/IUser';

const baseUrl = `${import.meta.env.VITE_API_URL}/api`

interface AuthState {
    userName: string | null;
    isAuthenticated: boolean;
    token: string | null;
    refreshToken: string | null;
    claims: string[];
    returnUrl: string;
}

export const useAuthStore = defineStore('auth', {
    state: (): AuthState => ({
        userName: null,
        isAuthenticated: true,
        token: null,
        refreshToken: null,
        claims: [],
        returnUrl: '',
    }),
    actions: {
        async signin(command: ISignInCommand) {
            // const response = await fetchWrapper.post(`${baseUrl}/signin`, command)
            //     .then((resp) => resp.json());
            const response = "signIn";

            console.log(`user: `,response);
        },
    },
});