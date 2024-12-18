import { useAuthStore } from '@/stores/AuthStore';
import axios from 'axios';

const httpApiClient = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

httpApiClient.interceptors.request.use(config => {
  const authStore = useAuthStore();
  const accessToken = authStore.accessToken;

  if (accessToken) {
    config.headers.Authorization = `Bearer ${accessToken}`;
  }
  return config;
  }, error => {
    return Promise.reject(error);
  }
);

export { httpApiClient };
