import {isValidationError,  type IValidationError } from "./IValidationError";

export interface IApiError {
  code: string;
  detail: string;
  instance: string;
  message: string;
  status: number;
  title: string;
  type: string;
  validationErrors: IValidationError[];
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
          (error: any) => isValidationError(error)
        )
      );
}
