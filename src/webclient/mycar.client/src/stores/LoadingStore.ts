import { defineStore } from 'pinia';

interface ILoadingState {
  isLoading: boolean;
}

export const useLoadingStore = defineStore("loading",
{
  state: (): ILoadingState => ({
    isLoading: false,
  }),
});
