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
    import { FormErrors } from '@/types/FormErrors';

    const authStore = useAuthStore();

    const errors = ref<FormErrors>(new FormErrors());
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
      errors.value.Add("UserName", "Identyfikator jest wymagany.");
      try {
        if(isFormValid){
          await authStore.signUpUser(data);
        }
      } catch (error: any) {
        const messageProvider = new MessageProvider("signUp");
        await messageProvider.Initialize();
        errors.value.ClearAll();
        if(isApiError(error)){
          error.validationErrors.forEach((value) => {
            const {code, message} = value;
            const translateMessage = messageProvider.GetMessage({code, message});
            errors.value.Add(value.field, translateMessage);
          });
          const message = messageProvider.GetMessage({code: error.code, message: error.message});
          errors.value.messages.push(message);
        } else {
          errors.value.messages.push("Nierozpoznany błąd systemowy");
        }
      }
    }

    const onChangeUsername = (value: string) => {
      formData.value.userName = value;
      touchedFields.value.userName = true;
      if(!(value.length > 0)){
        errors.value.Add("UserName", "Identyfikator jest wymagany.");
      }
    };

    const onChangeEmail = (value: string) => {
      formData.value.email = value;
      touchedFields.value.email = true;
      errors.value.Clear("Email");
      if(!(value.length > 0)){
        errors.value.Add("Email", "Email jest wymagany.");
      }
    };

    const onChangePassword = (value: string) => {
      formData.value.password = value;
      touchedFields.value.password = true;
      if(!(value.length > 0)){
        errors.value.Add("Password", "Hasło jest wymagane.");
      }
    };

    const onChangeRetypePassword = (value: string) => {
        formData.value.retypePassword = value;
        touchedFields.value.email = true;
    };

    const usernameValid = (value: string): boolean => {
      errors.value.Clear("UserName");
      const invalidCharsRegex = /[^a-zA-Z0-9_-]/;
      if ((touchedFields.value.userName) && (!value)) errors.value.Add("UserName", "Nazwa użytkownika jest wymagana.");
      if ((touchedFields.value.userName) && (value.length < 3)) errors.value.Add("UserName", "Nazwa użytkownika musi mieć co najmniej 3 znaki.");
      if ((touchedFields.value.userName) && (value.length > 20)) errors.value.Add("UserName", "Nazwa użytkownika może mieć maksymalnie 20 znaków.");
      if ((touchedFields.value.userName) && (invalidCharsRegex.test(value))) errors.value.Add("UserName", "Nazwa użytkownika może zawierać tylko litery, cyfry, myślniki i podkreślniki.");

      return errors.value.Get("UserName").length === 0;
    }

    const emailValid = (value: string): boolean => {
      errors.value.Clear("Email");
      const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
      if((touchedFields.value.email) && !emailRegex.test(value)) {
        errors.value.Add("Email","Wymagany prawidłowy adres email.");
      }
      return errors.value.Get("Email").length === 0;
    }

    const passwordValid = (value: string): boolean => {
      errors.value.Clear("Password");
      if((touchedFields.value.password) && (!value)) {
        errors.value.Add("Password", "Hasło jest wymagane");
      }
      return errors.value.Get("Password").length === 0;
    }

    const retypePasswordValid = (value: string): boolean => {
      errors.value.Clear("RetypePassword");
      if((touchedFields.value.retypePassword || touchedFields.value.password) && (value !== formData.value.password)) {
        errors.value.Add("RetypePassword","Powtórzone hasło musi być identyczne.");
      }

      return errors.value.Get("RetypePassword").length === 0;
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
        isFormValid.value = validateForm() &&
          touchedFields.value.userName &&
          touchedFields.value.email
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
              :messages="errors.Get('UserName')"
              @input="onChangeUsername"/>
          </div>
          <div class="form-group">
              <TextInput v-model="formData.email"
              type="text"
              id="email"
              label="Adres email"
              :messages="errors.Get('Email')"
              @input="onChangeEmail"/>
          </div>
          <div class="form-group">
              <TextInput v-model="formData.password"
              type="password"
              id="password"
              label="Hasło"
              :messages="errors.Get('Password')"
              @input="onChangePassword"/>
          </div>
          <div class="form-group">
              <TextInput v-model="formData.retypePassword"
              type="password"
              id="retypepassword"
              label="Powtórz hasło"
              :messages="errors.Get('RetypePassword')"
              @input="onChangeRetypePassword"/>
          </div>
          <button v-if="isFormValid" type="submit">Zarejestruj</button>
          <p></p>
          <div><RouterLink to="signin">Chcę się zalogować</RouterLink></div>
          <HintList style="margin-top: 10px;" :messages="errors.Get('Common')"/>
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
