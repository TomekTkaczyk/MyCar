import { defineStore } from 'pinia';

import router from '@/router'

import { httpApiClient, axiosErrorHandler } from '@/infrastructure/httpApiClient';

import type ISignInCommand from '@/modules/auth/requests/signin-command';
import type ISignUpCommand from '@/modules/auth/requests/signup-command';
import type IChangePasswordCommand from '@/modules/auth/requests/changepassword-command';
import type IUpdateProfileCommand from '@/modules/auth/requests/updateprofile-command';

import type IUser from '@/types/IUser';
import type IAuthState from '@/types/IAuthState';

export const useAuthStore = defineStore('auth', {
  state: (): IAuthState => ({}),
  actions: {
    async logout() {
      try {
        await httpApiClient.post('/users-module/Account/logout');
        this.$reset;
        router.push('/signin');
      } catch (error) {
        await axiosErrorHandler(error, 'logout');
      }
    },

    async signInUser(command: ISignInCommand) {
      try {
        const response = await httpApiClient.post('/users-module/Account/sign-in', command);
        this.token = response.data;
        this.isAuthenticated = true;
        await this.getUser();
        router.push('/');
      } catch (error) {
        await axiosErrorHandler(error, 'signIn');
      }
    },

    async signUpUser(command: ISignUpCommand) {
      try {
        await httpApiClient.post('/users-module/Account/sign-up', command);
        router.push('/signin');
        alert('Wysłaliśmy aktywację na twój adres email. Zaloguj się po potwierdzeniu adresu email.')
      } catch (error) {
        await axiosErrorHandler(error, 'signUp');
      }
    },

    async forgotPassword(command: string) {
      try {
        await httpApiClient.post('/users-module/Account/forgot-password', command);
        router.push('/signin');
        alert('Wysłaliśmy link do zmiany hasła na twój adres email.')
      } catch (error) {
        await axiosErrorHandler(error, 'forgotPassword');
      }
    },

    async changePassword(command: IChangePasswordCommand) {
      try {
        await httpApiClient.post('/users-module/change-password', command);
        alert('Hasło zostało zmienione.');
      } catch (error) {
        await axiosErrorHandler(error, 'changePassword');
      }
    },

    async changeEmail(command: string) {
      try {
        await httpApiClient.put('/users-module/Account/change-email', command);
      } catch (error) {
        axiosErrorHandler(error, 'changeEmail');
      }
    },

    async updateProfile(command: IUpdateProfileCommand) {
      try {
        await httpApiClient.put('/users-module/Account/update-profile', command);
        await this.getUser();
      } catch (error) {
        console.log(error);
        axiosErrorHandler(error, 'updateProfile');
      }
    },

    async getUser() {
      try {
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
      } catch (error) {
        await axiosErrorHandler(error, 'getUser');
      }
    },
  },
});
