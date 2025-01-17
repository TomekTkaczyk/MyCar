import { defineStore } from 'pinia';

import router from '@/router'

import { httpApiClient } from '@/infrastructure/httpApiClient';

import type ISignInCommand from '@/modules/auth/requests/signin-command';
import type ISignUpCommand from '@/modules/auth/requests/signup-command';
import type IConfirmEmailCommand from '@/modules/auth/requests/confirmemail-command';
import type IChangePasswordCommand from '@/modules/auth/requests/changepassword-command';
import type IUpdateProfileCommand from '@/modules/auth/requests/updateprofile-command';
import { AlerMessage } from '@/infrastructure/AlertMessage';
import type IAuthState from '@/types/IAuthState';
import { isAxiosError } from 'axios';
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
        this.isAuthenticated = false;
        this.accessToken = null;
        this.refreshToken = null;
        sessionStorage.removeItem('auth');
        await httpApiClient.post('/users-module/Account/logout');
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
        await httpApiClient.post('/users-module/Account/sign-up', command,
          {headers:
            {'X-Frontend-Url': window.location.origin+"/ConfirmEmail"}
          }
        );
        router.push('/signin');
        const alertMessage = new AlerMessage();
        alertMessage.Show('Wysłaliśmy aktywację na twój adres email. Zaloguj się po potwierdzeniu adresu email.')
      } catch (error) {
        errorHanle(error);
      }
    },

    async remindPassword(email: string) {
      try {
        await httpApiClient.post('/users-module/Account/remaind-password', null, { params: { email: email}});
        router.push('/signin');
        const alertMessage = new AlerMessage();
        alertMessage.Show('Wysłaliśmy link do zmiany hasła na twój adres email.\nZaloguj się do aplikacji.')
      } catch (error) {
        errorHanle(error);
      }
    },

    async changePassword(command: IChangePasswordCommand) {
      try{
        await httpApiClient.post('/users-module/Account/change-password', command);
        const alertMessage = new AlerMessage();
        alertMessage.Show('Hasło zostało zmienione.');
      } catch (error) {
        errorHanle(error);
      }
    },

    async changeEmail(email: string) {
      try{
        await httpApiClient.post('/users-module/Account/change-email', null,
          {
            params: {email: email},
            headers: {'X-Frontend-Url': window.location.origin+"/ConfirmEmail"}
          }
        );
        const alertMessage = new AlerMessage();
        alertMessage.Show('Na podany adres został wysłany link aktywacyjny.');
      } catch (error) {
        errorHanle(error);
      }
    },

    async confirmEmail(token: string) {
      try{
        await httpApiClient.post('users-module/Account/confirm-email', null,
          {
            params: {token: token},
          }
        )
        router.push("/signin");
        const alertMessage = new AlerMessage();
        alertMessage.Show('Adres email został potwierdzony.\nZaloguj się do aplikacji.');
      } catch (error) {
        console.error();
        errorHanle(error);
      }
    },

    async resendConfirmEmail(email: string) {
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
      this.id = userReponse.data.id;
      this.name = userReponse.data.name;
      this.email = userReponse.data.email;
      this.firstName = userReponse.data.firstName;
      this.lastName = userReponse.data.lastName;
      this.role = userReponse.data.role;
      this.claims = userReponse.data.claims;
      this.isConfirmed = userReponse.data.isConfirmed;

      sessionStorage.setItem('auth', JSON.stringify({
        accessToken: this.accessToken,
        refreshToken: this.refreshToken,
        isAuthenticated: this.isAuthenticated
      }))
    },

    async initialize() {
      const authData = sessionStorage.getItem('auth');
      if(authData) {
        const {accessToken, refreshToken, isAuthenticated} = JSON.parse(authData);
        this.accessToken = accessToken;
        this.refreshToken = refreshToken;
        this.isAuthenticated = isAuthenticated;
        await this.getUser();
      };
    },

    setTokens(accessToken: string, refreshToken: string) {
      this.accessToken = accessToken;
      this.refreshToken = refreshToken;
      this.isAuthenticated = !!accessToken;
    }
  },
});

