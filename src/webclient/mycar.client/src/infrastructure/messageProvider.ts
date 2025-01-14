import type IMessageProvider from '@/types/IMessageProvider';

class MessageProvider implements IMessageProvider {

  private messageDictionary: { [code: string]: string } = {};
  private messageGroup: string;

  messageModules: { [key: string]: () => Promise<{ default: { [code: string]: string } }> } = {
    signIn: () => import('@/infrastructure/messages/signInMessages'),
    signUp: () => import('@/infrastructure/messages/signUpMessages'),
    getUser: () => import('@/infrastructure/messages/getUserMessages'),
    forgotPassword: () => import('@/infrastructure/messages/remaindPasswordMessages'),
    logout: () => import('@/infrastructure/messages/logoutMessages'),
    updateProfile: () => import('@/infrastructure/messages/updateProfileMessages'),
    changeEmail: () => import('@/infrastructure/messages/changeEmailMessages'),
    changePassword: () => import('@/infrastructure/messages/changePasswordMessages'),
  };

  constructor(messageGroup: string) {
    this.messageGroup = messageGroup;
  }

  GetMessage(error: {code: string, message: string}): string {
    return this.messageDictionary[error.code] ?? error.message;
  }

  async Initialize(): Promise<void> {
    const loader = this.messageModules[this.messageGroup];
    if (!loader) {
      throw new Error(`Message group "${this.messageGroup}" not found`);
    }

    try {
      var messages = await loader();
      this.messageDictionary = messages.default;
    } catch (error) {
      console.error(`Error loading message group "@/infrastructure/${this.messageGroup}Messages":`, error);
    }
  }
}

export default MessageProvider;
