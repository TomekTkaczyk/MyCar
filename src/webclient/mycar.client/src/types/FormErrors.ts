import { Dictionary } from "./Dictionary";

export class FormErrors {
  public readonly _dictionary: Dictionary<string[]> = new Dictionary<string[]>();
  public readonly messages: string[] = [];

  public Add(field: string, message: string): void {
    const item = this._dictionary.Get(field);
    if (item !== undefined) {
      this._dictionary.Set(field, [...item, message]);
    } else {
      this._dictionary.Set(field, [message]);
    }
    console.log(this._dictionary);
  }

  public ClearAll(): void {
    this._dictionary.Clear();
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
}
