export interface IErrorMessage {
  field: string;
  code: string;
  message: string;
}

export function isErrorMessage(obj: any): obj is IErrorMessage {
    return (
        obj !== null &&
        typeof obj === "object" &&
        typeof obj.field === "string" &&
        typeof obj.code === "string" &&
        typeof obj.message === "string"
      );
}
