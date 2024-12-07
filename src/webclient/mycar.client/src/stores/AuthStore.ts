import { defineStore } from 'pinia';

import { fetchWrapper } from '@/helpers/fetch-wrapper';
import router from '@/router'
import { useAlertStore } from '@/stores/AlertStore';

import type ISignInCommand from '@/modules/auth/requests/signin-command';
import type ISignUpCommand from '@/modules/auth/requests/signup-command';
import type IUser from '@/types/IUser';
import type IAuthState from '@/types/IAuthState';

const baseUrl = `${import.meta.env.VITE_API_URL}/api`

console.log(baseUrl);

export const useAuthStore = defineStore('auth', {
  state: (): IAuthState => ({
      userName: null,
      isAuthenticated: false,
      token: null,
      refreshToken: null,
      role: '',
      claims: [],
      returnUrl: '',
  }),
  actions: {
      async signInUser(command: ISignInCommand) {
        console.log("Strzał do API/Login", command);
        const response = await fetchWrapper.post(`${baseUrl}/signin`, command)
            .then((resp) => resp.json());
        console.log(`user: `,response);
      },
      async signUpUser(command: ISignUpCommand) {
        console.log("Strzał do API/Rejestracja", command);
        const response = await fetchWrapper.post(`${baseUrl}/signup`, command)
            .then((resp) => resp.json());
        console.log(`user: `,response);
      },
      async remindPassword(command: string) {
        console.log("Strzał do API/PrzypomnijHasło", command);
        const response = await fetchWrapper.post(`${baseUrl}/remind`, command)
            .then((resp) => resp.json());
        console.log(`user: `,response);
      },
      async changePassword(command: string) {
        console.log("Strzał do API/ZmienHasło", command);
        const response = await fetchWrapper.post(`${baseUrl}/changepassword`, command)
            .then((resp) => resp.json());
        console.log(`user: `,response);
      },
      async changeEmail(command: string) {
        console.log("Strzał do API/ZmienEmail", command);
        const response = await fetchWrapper.post(`${baseUrl}/changeemail`, command)
            .then((resp) => resp.json());
        console.log(`user: `,response);
      },  },
});
