import { isApiError } from "@/types/IApiError";
import MessageProvider from '@/infrastructure/messageProvider';

async function catchFunction(action: string, error: any): Promise<string[]> {

  const messageProvider = new MessageProvider(action);
  await messageProvider.Initialize();
  let errors: IErrorMessage[] = [];

  if(isApiError(error)){
    error.validationErrors.forEach((value) => {
      const {code, message} = value;
      errors.push(messageProvider.GetMessage({code,message}));
    });
    const {field, code, message} = error;
    errors = [messageProvider.GetMessage({field,code,message})];
  } else {
    errors = ["Nierozpoznany błąd systemowy"];
  }

  return errors;
}

export default catchFunction;
