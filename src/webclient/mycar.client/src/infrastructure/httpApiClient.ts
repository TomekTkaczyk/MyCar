import { useAuthStore } from '@/stores/AuthStore';
import axios, { AxiosError, type AxiosRequestConfig, type AxiosResponse } from 'axios';

const httpApiClient = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
  withCredentials: true,
});

interface FailedQueueItem {
  resolve: (value?: any) => void;
  reject: (reason?: any) => void;
}

let failedQueue: FailedQueueItem[] = [];

const processQueue = (error: any, token: string | null = null): void => {
  failedQueue.forEach(prom => {
    if (error) {
      prom.reject(error);
    } else {
      prom.resolve(token);
    }
  });
  failedQueue = [];
};

httpApiClient.interceptors.response.use(
  response => response,
  async error => {
    const originalRequest = error.config as AxiosRequestConfig;
    if (error.response && error.response.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
      const authStore = useAuthStore();
      if (authStore.isRefreshing) {
        return new Promise<any>((resolve, reject) => {
          failedQueue.push({ resolve, reject });
        }).then(() => httpApiClient(originalRequest));
      };
      authStore.isRefreshing = true;
      try {
        await httpApiClient.post('/users-module/Account/refresh-token');
        return axios(originalRequest);
      } catch (refreshError) {
        processQueue(refreshError);
        authStore.logout();
        return Promise.reject(refreshError);
      } finally {
        authStore.isRefreshing = false;
      }
    };
    return Promise.reject(error);
  });

export { httpApiClient };
