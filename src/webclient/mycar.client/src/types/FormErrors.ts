import { Dictionary } from "./Dictionary";
import { isApiError, type IApiError } from "./IApiError";
import MessageProvider from "@/infrastructure/MessageProvider";

class FormErrors {
  public readonly _dictionary: Dictionary<string[]> = new Dictionary<string[]>();
  public readonly messages: string[] = [];

  public Add(field: string, message: string): void {
    const item = this._dictionary.Get(field);
    if (item !== undefined) {
      this._dictionary.Set(field, [...item, message]);
    } else {
      this._dictionary.Set(field, [message]);
    }
  }

  public ClearAll(): void {
    this._dictionary.Clear();
    this.messages.length = 0;
  }

  public Clear(field: string): void {
    if (this._dictionary.Get(field) !== undefined) {
      this._dictionary.Remove(field);
    }
  }

  public Get(field: string): string[] {
    const items = this._dictionary.Get(field);
    if (items !== undefined) {
      return items;
    }
    return [];
  }

  public get Count(): number {
    return this.messages.length + this._dictionary.Count;
  }

  public async CatchApiError(formName: string, error: any) {
    if(isApiError(error)){
      const messageProvider = new MessageProvider(formName);
      await messageProvider.Initialize();
      this.ClearAll();
      error.validationErrors.forEach((value) => {
        const {code, message} = value;
        this.Add(value.field, messageProvider.GetMessage({code, message}));
      });
      if(error.code){
        const message = messageProvider.GetMessage({code: error.code, message: error.message});
        this.messages.push(message);
      }
    } else {
      this.messages.push("Nierozpoznany błąd systemowy.");
    };
  }
}

export { FormErrors };
