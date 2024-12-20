export default interface IMessageProvider {
  GetMessage(messageCode: string): string;
}
