import { defineStore } from 'pinia';

import router from '@/router'

import { httpApiClient } from '@/infrastructure/httpApiClient';

import type ISignInCommand from '@/modules/auth/requests/signin-command';
import type ISignUpCommand from '@/modules/auth/requests/signup-command';
import type IChangePasswordCommand from '@/modules/auth/requests/changepassword-command';
import type IUpdateProfileCommand from '@/modules/auth/requests/updateprofile-command';

import type IUser from '@/types/IUser';
import type IAuthState from '@/types/IAuthState';
import type IChangeEmailCommand from '@/modules/auth/requests/changeemail-command';
import type IRemaindPasswordCommand from '@/modules/auth/requests/remaindpassword-command';

export const useAuthStore = defineStore('auth', {
  state: (): IAuthState => ({
    accessToken: null,
    refreshToken: null,
    isAuthenticated: false,
    isRefreshing: false,
  }),
  actions: {
    async logout() {
      await httpApiClient.put('/users-module/Account/logout');
      this.$reset();
      router.push('/signin');
    },

    async signInUser(command: ISignInCommand) {
      await httpApiClient.post('/users-module/Account/sign-in', command);
      this.isAuthenticated = true;
      await this.getUser();
      router.push('/');
    },

    async signUpUser(command: ISignUpCommand) {
      await httpApiClient.post('/users-module/Account/sign-up', command);
      router.push('/signin');
      alert('Wysłaliśmy aktywację na twój adres email. Zaloguj się po potwierdzeniu adresu email.')
    },

    async reamindPassword(command: IRemaindPasswordCommand) {
      await httpApiClient.post('/users-module/Account/remaind-password', command);
      router.push('/signin');
      alert('Wysłaliśmy link do zmiany hasła na twój adres email.')
    },

    async changePassword(command: IChangePasswordCommand) {
      await httpApiClient.post('/users-module/Account/change-password', command);
      alert('Hasło zostało zmienione.');
    },

    async changeEmail(command: IChangeEmailCommand) {
      await httpApiClient.post('/users-module/Account/change-email', command);
      alert('Adres email został zmieniony.');
    },

    async updateProfile(command: IUpdateProfileCommand) {
      await httpApiClient.put('/users-module/Account/update-profile', command);
      await this.getUser();
    },

    async getUser() {
      const userReponse = await httpApiClient.get('/users-module/Account');
      const user: IUser = userReponse.data;
      this.id = user.id;
      this.name = user.name;
      this.email = user.email;
      this.firstName = user.firstName;
      this.lastName = user.lastName;
      this.role = user.role;
      this.claims = user.claims;
      this.isConfirmed = user.isConfirmed;
    },

    setTokens(accessToken: string, refreshToken: string) {
      this.accessToken = accessToken;
      this.refreshToken = refreshToken;
      this.isAuthenticated = !!accessToken;
      console.log("Setting tokens:", accessToken, refreshToken);
      console.log("this tokens:", this.accessToken, this.refreshToken);
    }
  },
});
