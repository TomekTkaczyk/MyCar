export default interface IRemaindPasswordCommand {
  email: string,
}

export function isRemaindPasswordCommand(obj: any): obj is IRemaindPasswordCommand {
    return (
        obj !== null &&
        typeof obj === "object" &&
        typeof obj.email === "string"
      );
}
