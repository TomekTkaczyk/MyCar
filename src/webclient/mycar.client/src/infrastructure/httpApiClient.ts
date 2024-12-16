import { useAuthStore } from '@/stores/AuthStore';
import axios, { AxiosError } from 'axios';
import type ApiError from '@/types/IApiError';
import MessageProvider from './messageProvider';
import type { ValidationErrorResponse } from '../types/IValidationErrorResponse';

const httpApiClient = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  headers: {
    'Content-Type': 'application/json',
  }
});

httpApiClient.interceptors.request.use(config => {
  const authStore = useAuthStore();
  const token = authStore?.token;
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
  }, error => {
    return Promise.reject(error);
  }
);

function isAxiosError(error: unknown): error is AxiosError {
  return axios.isAxiosError(error);
};

function isApiError(data: any): data is ApiError {
  return data && Array.isArray(data.errors) && data.errors.every((e: any) => 'code' in e && 'message' in e);
};

async function axiosErrorHandler(error: any, messageGroup: string) {
  if (isAxiosError(error) && error.response) {
    const errorBody = error.response.data;

    var messageError: string = '';

    if (isApiError(errorBody)) {
      const messageProvider = new MessageProvider(messageGroup);
      await messageProvider.Initialize();
      errorBody.errors.forEach((err) => {
        messageError += messageProvider.GetMessage(err.code) || `Error: ${err.message}`;
      });
      alert(messageError);
    } else {
      const axiosError = error as AxiosError<ValidationErrorResponse>;
      if (axiosError.response && axiosError.response.status === 400) {
        const validationErrors = axiosError.response.data.errors;
        console.log('Validation Errors:', validationErrors);

        // Example: Process errors for displaying on a form
        for (const [field, messages] of Object.entries(validationErrors)) {
          console.log(`Field: ${field}, Messages: ${messages.join(', ')}`);
        }
      } else {
        console.error('Unhandled error:', error);
      }
    }
  } else {
    console.error('Non-Axios error:', error);
  }
}

export { httpApiClient, isAxiosError, isApiError, axiosErrorHandler };
