export default interface IChangeEmailCommand {
  email: string,
}


export function isChangeEmailCommand(obj: any): obj is IChangeEmailCommand {
    return (
        obj !== null &&
        typeof obj === "object" &&
        typeof obj.email === "string"
      );
}
