import type { IErrorMessage } from "./IErrorMessage";

export interface IApiError {
  code: string;
  detail: string;
  instance: string;
  message: string;
  status: number;
  title: string;
  validationErrors: IErrorMessage[];
}

export function isApiError(obj: any): obj is IApiError {
    return (
        obj !== null &&
        typeof obj === "object" &&
        typeof obj.code === "string" &&
        typeof obj.message === "string" &&
        typeof obj.status === "number" &&
        Array.isArray(obj.validationErrors) &&
        obj.validationErrors.every(
          (error: any) => error.isErrorMessage(error)
        )
      );
}
