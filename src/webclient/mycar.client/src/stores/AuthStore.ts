import { defineStore } from 'pinia';

import { fetchWrapper } from '@/helpers/fetch-wrapper';
import router from '@/router'
import { useAlertStore } from '@/stores/AlertStore';

import type ISignInCommand from '@/modules/auth/requests/signin-command';
import type ISignUpCommand from '@/modules/auth/requests/signup-command';
import type IChangePasswordCommand from '@/modules/auth/requests/changepassword-command';
import type IUser from '@/types/IUser';
import type IAuthState from '@/types/IAuthState';
import type Logout from '@/modules/auth/Logout.vue';
import type IChangeProfileCommand from '@/modules/auth/requests/changeprofile-command';

const baseUrl = `${import.meta.env.VITE_API_URL}/api`

// console.log(baseUrl);

export const useAuthStore = defineStore('auth', {
  state: (): IAuthState => ({
    userName: "jk",
    firstName: "Jan",
    lastName: "Kowalski",
    email: 'jakis@email.eu',
    isAuthenticated: false,
    token: null,
    refreshToken: null,
    role: '',
    claims: [],
    returnUrl: '',
  }),
  actions: {
    async logout(command: string) {
      console.log("Strzał do API/Logout ale co wysłać?");
      const response = await fetchWrapper.post(`${baseUrl}/logout`, command)
      const user = await response.json();
      console.log(`user: `, user);
    },
    async signInUser(command: ISignInCommand) {
      console.log("Strzał do API/Login", command);
      const response = await fetchWrapper.post(`${baseUrl}/signin`, command)
        .then((resp) => resp.json());
      console.log(`user: `, response);
      router.push("/About");
    },
    async signUpUser(command: ISignUpCommand) {
      console.log("Strzał do API/Rejestracja", command);
      const response = await fetchWrapper.post(`${baseUrl}/signup`, command)
        .then((resp) => resp.json());
      console.log(`user: `, response);
    },
    async remindPassword(command: string) {
      console.log("Strzał do API/PrzypomnijHasło", command);
      const response = await fetchWrapper.post(`${baseUrl}/remind`, command)
        .then((resp) => resp.json());
      console.log(`user: `, response);
    },
    async changePassword(command: IChangePasswordCommand) {
      console.log("Strzał do API/ZmienHasło", command);
      const response = await fetchWrapper.post(`${baseUrl}/changepassword`, command)
        .then((resp) => resp.json());
      console.log(`user: `, response);
    },
    async changeEmail(command: string) {
      console.log("Strzał do API/ZmienEmail", command);
      const response = await fetchWrapper.post(`${baseUrl}/changeemail`, command)
        .then((resp) => resp.json());
      console.log(`user: `, response);
    },
    async changeProfile(command: IChangeProfileCommand) {
      console.log("Strzał do API/UpdateUser", command);
      const response = await fetchWrapper.put(`${baseUrl}/user`, command)
        .then((resp) => resp.json());
      console.log(`user: `, response);
    },
  },
});
