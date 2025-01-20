import { defineStore } from 'pinia';
import { httpApiClient } from '@/infrastructure/httpApiClient';
import ErrorHandler from './ErrorHandler';

import { AlerMessage } from '@/infrastructure/AlertMessage';
import type IUpdatePrivilegeCommand from '@/modules/users/requests/updateprivilege-command';

const errorHandle = ErrorHandler;

export const useUserStore = defineStore('users',{
  actions: {
    async getUsers() {
      try{
        return await httpApiClient.get('/users-module');
      } catch (error) {
        errorHandle(error);
      }
    },

    async getUser(id: string) {
      try{
        return await httpApiClient.get(`/users-module/${id}`);
      } catch (error) {
        errorHandle(error);
      }
    },

    async updatePrivilege(command: IUpdatePrivilegeCommand) {
      try{
        await httpApiClient.post('/users-module/update-privilege', command);
        const alertMessage = new AlerMessage();
        alertMessage.Show('Uprawnienia zostały zaktualizowane.');
      } catch (error) {
        errorHandle(error);
      }
    },
  }
});
