export interface IApiError {
  errors: { code: string, message: string }[];
}

export function isApiError(obj: any): obj is IApiError {
    return (
        obj !== null &&
        typeof obj === "object" &&
        Array.isArray(obj.errors) &&
        obj.errors.every(
          (error: any) =>
            typeof error === "object" &&
            typeof error.code === "string" &&
            typeof error.message === "string"
        )
      );
}
