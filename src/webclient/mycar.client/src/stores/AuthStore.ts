import { defineStore } from 'pinia';

import router from '@/router'

import { httpApiClient } from '@/infrastructure/httpApiClient';

import type ISignInCommand from '@/modules/auth/requests/signin-command';
import type ISignUpCommand from '@/modules/auth/requests/signup-command';
import type IConfirmEmailCommand from '@/modules/auth/requests/confirmemail-command';
import type IChangePasswordCommand from '@/modules/auth/requests/changepassword-command';
import type IUpdateProfileCommand from '@/modules/auth/requests/updateprofile-command';

import type IUser from '@/types/IUser';
import type IAuthState from '@/types/IAuthState';
import type IChangeEmailCommand from '@/modules/auth/requests/changeemail-command';
import type IRemaindPasswordCommand from '@/modules/auth/requests/remaindpassword-command';
import { isAxiosError, type AxiosError } from 'axios';
import { ApiError } from '@/infrastructure/errors/ApiError';

const errorHanle = (error: any) => {
  if(isAxiosError(error)){
    switch(error.code) {
      case "ERR_NETWORK": {
        router.push("/Error503");
        break;
      };
      case "ERR_BAD_REQUEST": {
        throw error.response?.data;
      };
      default: {
        throw error;
      };
    }
  } else {
    throw new ApiError('Wystąpił nieoczekiwany błąd.', "ApiError");
  }
}

export const useAuthStore = defineStore('auth', {

  state: (): IAuthState => ({
    accessToken: null,
    refreshToken: null,
    isAuthenticated: false,
    isRefreshing: false,
  }),

  actions: {
    async logout() {
      try {
        await httpApiClient.post('/users-module/Account/logout');
        this.$reset();
        router.push('/signin');
      } catch (error) {
        errorHanle(error);
      }
    },

    async signInUser(command: ISignInCommand) {
      try{
        await httpApiClient.post('/users-module/Account/sign-in', command);
        this.isAuthenticated = true;
        await this.getUser();
        router.push('/');
      } catch (error) {
        errorHanle(error);
      }
    },

    async signUpUser(command: ISignUpCommand) {
      try {
        await httpApiClient.post('/users-module/Account/sign-up', command, {headers: {
          'X-Frontend-Url': window.location.origin+"/ConfirmEmail",
        }});
        router.push('/signin');
        alert('Wysłaliśmy aktywację na twój adres email. Zaloguj się po potwierdzeniu adresu email.')
      } catch (error) {
        errorHanle(error);
      }
    },

    async reamindPassword(command: IRemaindPasswordCommand) {
      try {
        await httpApiClient.post('/users-module/Account/remaind-password', command);
        router.push('/signin');
        alert('Wysłaliśmy link do zmiany hasła na twój adres email.\nZaloguj się do aplikacji.')
      } catch (error) {
        errorHanle(error);
      }
    },

    async changePassword(command: IChangePasswordCommand) {
      try{
        await httpApiClient.post('/users-module/Account/change-password', command);
        alert('Hasło zostało zmienione.');
      } catch (error) {
        errorHanle(error);
      }
    },

    async changeEmail(command: IChangeEmailCommand) {
      try{
        await httpApiClient.post('/users-module/Account/change-email', command);
        alert('Adres email został zmieniony.');
      } catch (error) {
        errorHanle(error);
      }
    },

    async confirmEmail(command: IConfirmEmailCommand) : Promise<void> {
      try{
        await httpApiClient.post('users-module/Account/confirm-email', command)
        router.push("/signin");
        alert('Adres email został potwierdzony.\nZaloguj się do aplikacji.');
      } catch (error) {
        errorHanle(error);
      }
    },

    async resendConfirmEmail(email: string) : Promise<void> {
      try{
        await httpApiClient.post('users-module/Account/resend-confirm-email', email)
      } catch (error) {
        errorHanle(error);
      }
    },

    async updateProfile(command: IUpdateProfileCommand) {
      try{
        await httpApiClient.put('/users-module/Account/update-profile', command);
        await this.getUser();
      } catch (error) {
        errorHanle(error);
      }
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
    }
  },
});
