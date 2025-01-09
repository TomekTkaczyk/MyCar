<!-- ***************************************************  -->
<!-- * Script section                                  *  -->
<!-- ***************************************************  -->

<script setup lang="ts">
    import { ref, watchEffect } from 'vue';
    import type ISignUpCommand from './requests/signup-command';
    import TextInput from "@/components/TextInput.vue";
    import HintList from '@/components/HintList.vue';
    import { useAuthStore } from '@/stores/AuthStore';
    import MessageProvider from '@/infrastructure/messageProvider';
    import { isApiError } from '@/types/IApiError';

    const errors = ref<string[]>([]);
    const userNameMessages = ref<string[]>([]);
    const emailMessages  = ref<string[]>([]);
    const passwordMessages = ref<string[]>([]);
    const retypePasswordMessages = ref<string[]>([]);

    const authStore = useAuthStore();
    const isFormValid = ref(false);

    const touchedFields = ref({
      userName: false,
      email: false,
      password: false,
      retypePassword: false
    });

    const formData = ref<ISignUpCommand & {retypePassword: string}>({
      userName: '',
      email: '',
      password: '',
      retypePassword: '',
    });

    async function signUpUser(data: ISignUpCommand) {
      const { userName, email, password } = data;
      const body: ISignUpCommand = {userName, email, password};
      const messageProvider = new MessageProvider("signUp");
      await messageProvider.Initialize();
      errors.value = [];
      userNameMessages.value = [];
      emailMessages.value = [];
      passwordMessages.value = [];
      retypePasswordMessages.value = [];

      try {
          await authStore.signUpUser(body);
      } catch (error: any) {
        if(isApiError(error)){
          error.validationErrors.forEach((value) => {
            const {code, message} = value;
            errors.value.push(messageProvider.GetMessage({code,message}));
          });
          const {code, message} = error;
          errors.value = [messageProvider.GetMessage({code,message})];
        } else {
          errors.value = ["Nierozpoznany błąd systemowy"];
        }
      }
    }

    const onChangeUsername = (value: string) => {
      formData.value.userName = value;
      touchedFields.value.userName = true;
      if(!(value.length > 0)){
        userNameMessages.value.push("Identyfikator jest wymagany.");
      }
    };

    const onChangeEmail = (value: string) => {
      formData.value.email = value;
      touchedFields.value.email = true;
      if(!(value.length > 0)){
        emailMessages.value.push("Email jest wymagany.");
      }
    };

    const onChangePassword = (value: string) => {
      formData.value.password = value;
      touchedFields.value.password = true;
      if(!(value.length > 0)){
        passwordMessages.value.push("Hasło jest wymagane.");
      }
    };

    const onChangeRetypePassword = (value: string) => {
        formData.value.retypePassword = value;
        touchedFields.value.email = true;
    };

    const usernameValid = (value: string): boolean => {
      userNameMessages.value = [];
      const invalidCharsRegex = /[^a-zA-Z0-9_-]/;

      if ((touchedFields.value.userName) && (!value)) userNameMessages.value.push("Nazwa użytkownika jest wymagana.");
      if ((touchedFields.value.userName) && (value.length < 3)) userNameMessages.value.push("Nazwa użytkownika musi mieć co najmniej 3 znaki.");
      if ((touchedFields.value.userName) && (value.length > 20)) userNameMessages.value.push("Nazwa użytkownika może mieć maksymalnie 20 znaków.");
      if ((touchedFields.value.userName) && (invalidCharsRegex.test(value))) userNameMessages.value.push("Nazwa użytkownika może zawierać tylko litery, cyfry, myślniki i podkreślniki.");

      return userNameMessages.value.length === 0;
    }

    const emailValid = (value: string): boolean => {
      emailMessages.value = [];
      const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

      if((touchedFields.value.email) && !emailRegex.test(value)) emailMessages.value.push("Wymagany prawidłowy adres email.");

      return emailMessages.value.length === 0;
    }

    const passwordValid = (value: string): boolean => {
        passwordMessages.value = [];

        if((touchedFields.value.password) && (!value)) passwordMessages.value.push("Hasło jest wymagane");

        return passwordMessages.value.length === 0;
    }

    const retypePasswordValid = (value: string): boolean => {
        retypePasswordMessages.value = [];

        if((touchedFields.value.retypePassword || touchedFields.value.password) && (value !== formData.value.password)) retypePasswordMessages.value.push("Powtórzone hasło musi być identyczne.");

        return retypePasswordMessages.value.length === 0;
    }

    const validateForm = () => {
        const { userName, email, password, retypePassword } = formData.value;
        return (
          usernameValid(userName) &&
          emailValid(email) &&
          passwordValid(password) &&
          retypePasswordValid(retypePassword)
        );
    };

    watchEffect(() => {
        isFormValid.value = validateForm() && (touchedFields.value.userName || touchedFields.value.email || touchedFields.value.password || touchedFields.value.retypePassword);
    });

</script>

<!-- ***************************************************  -->
<!-- * Template section                                *  -->
<!-- ***************************************************  -->

<template>
  <div class="signup-form">
      <h2>Rejestracja użytkownika</h2>
      <form @submit.prevent="signUpUser(formData)">
         <div class="form-group">
              <TextInput v-model="formData.userName"
              type="text"
              id="username"
              label="Nazwa użytkownika"
              :messages="userNameMessages"
              @input="onChangeUsername"/>
          </div>
          <div class="form-group">
              <TextInput v-model="formData.email"
              type="text"
              id="email"
              label="Adres email"
              :messages="emailMessages"
              @input="onChangeEmail"/>
          </div>
          <div class="form-group">
              <TextInput v-model="formData.password"
              type="password"
              id="password"
              label="Hasło"
              :messages="passwordMessages"
              @input="onChangePassword"/>
          </div>
          <div class="form-group">
              <TextInput v-model="formData.retypePassword"
              type="password"
              id="retypepassword"
              label="Powtórz hasło"
              :messages="retypePasswordMessages"
              @input="onChangeRetypePassword"/>
          </div>
          <button type="submit" v-if="isFormValid">Zarejestruj</button>
          <p></p>
          <div><RouterLink to="signin">Chcę się zalogować</RouterLink></div>
          <HintList style="margin-top: 10px;" :messages="errors"/>
      </form>
  </div>
</template>

<!-- ***************************************************  -->
<!-- * Style section                                   *  -->
<!-- ***************************************************  -->

<style scoped>
    .signup-form {
        max-width: 400px;
        margin: 0 auto;
        padding: 20px;
        border: 1px solid #ccc;
        border-radius: 5px;
    }

    .form-group {
        margin-bottom: 15px;
    }

    label {
        display: block;
        margin-bottom: 5px;
    }

    input {
        width: 100%;
        padding: 8px;
        border: 1px solid #ccc;
        border-radius: 5px;
    }
</style>
