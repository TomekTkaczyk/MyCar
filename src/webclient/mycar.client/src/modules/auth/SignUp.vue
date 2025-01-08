<!-- ***************************************************  -->
<!-- * Script section                                  *  -->
<!-- ***************************************************  -->

<script setup lang="ts">
    import { ref, watchEffect } from 'vue';
    import type ISignUpCommand from './requests/signup-command';
    import TextInput from "@/components/TextInput.vue";
    import { useAuthStore } from '@/stores/AuthStore';
    import MessageProvider from '@/infrastructure/messageProvider';
import { isApiError, type IApiError } from '@/types/IApiError';

    interface Hints {
      userNameHints: string[],
      emailHints: string[],
      passwordHints: string[],
      retypePasswordHints: string[]
    }

    const hints = ref<Hints>({
      userNameHints: [],
      emailHints: [],
      passwordHints: [],
      retypePasswordHints: []
    });

    const formData = ref<ISignUpCommand & {retypePassword: string}>({
      userName: '',
      email: '',
      password: '',
      retypePassword: '',
    });

    const touchedFields = ref({
      userName: false,
      email: false,
      password: false,
      retypePassword: false
    });

    const authStore = useAuthStore();

    async function signUpUser(data: ISignUpCommand) {
      const { userName, email, password } = formData.value;
      const body: ISignUpCommand = {userName, email, password};
      const messageProvider = new MessageProvider("signIn");
      await messageProvider.Initialize();

      try {
          await authStore.signUpUser(body);
      } catch (error) {
        if(isApiError(error)){
          const {errors} = (error as IApiError).errors;

          UserName.forEach(element => {
            hints.value.userNameHints.push(messageProvider.GetMessage(element));
          });
          Email.forEach(element => {
            hints.value.emailHints.push(element);
          });
          Password.forEach(element => {
            hints.value.passwordHints.push(element);
          });
        }
      }
    }

    const isFormValid = ref(false);

    const onChangeUsername = (value: string) => {
      formData.value.userName = value;
      touchedFields.value.userName = true;
    };

    const onChangeEmail = (value: string) => {
        formData.value.email = value;
        touchedFields.value.email = true;
    };

    const onChangePassword = (value: string) => {
        formData.value.password = value;
        touchedFields.value.password = true;
    };

    const onChangeRetypePassword = (value: string) => {
        formData.value.retypePassword = value;
        touchedFields.value.retypePassword = true;
    };

    const validateForm = () => {
        const { userName, email, password, retypePassword } = formData.value;
        hints.value.userNameHints = [];
        hints.value.emailHints = [];
        hints.value.passwordHints = [];
        hints.value.retypePasswordHints = [];
        return userNameValid(userName) && emailValid(email) && passwordValid(password) && retypePasswordValid(retypePassword);
    };

    const userNameValid = (value: string): boolean => {
      const errors: string[] = [];
      const invalidCharsRegex = /[^a-zA-Z0-9_-]/;

      if ((touchedFields.value.userName) && (!value)) errors.push("Nazwa użytkownika jest wymagana.");
      if ((touchedFields.value.userName) && (value.length < 3)) errors.push("Nazwa użytkownika musi mieć co najmniej 3 znaki.");
      if ((touchedFields.value.userName) && (value.length > 20)) errors.push("Nazwa użytkownika może mieć maksymalnie 20 znaków.");
      if ((touchedFields.value.userName) && (invalidCharsRegex.test(value))) errors.push("Nazwa użytkownika może zawierać tylko litery, cyfry, myślniki i podkreślniki.");
      hints.value.userNameHints = errors;

      return errors.length === 0;
    }

    const emailValid = (value: string): boolean => {
      const errors: string[] = [];
      const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

      if((touchedFields.value.email) && !emailRegex.test(value)) errors.push("Wymagany prawidłowy adres email.");
      hints.value.emailHints = errors;

      return errors.length === 0;
    }

    const passwordValid = (value: string): boolean => {
        const errors: string[] = [];

        if(touchedFields.value.password) {
            hints.value.passwordHints = errors;
        }

        return errors.length === 0;
    }

    const retypePasswordValid = (value: string): boolean => {
        const errors: string[] = [];

        if((touchedFields.value.retypePassword || touchedFields.value.password) && (value !== formData.value.password)) {
            errors.push("Powtórzone hasło musi być identyczne.");
            hints.value.retypePasswordHints = errors;
        }

        return errors.length === 0;
    }

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
              :messages="hints.userNameHints"
              @input="onChangeUsername"/>
          </div>
          <div class="form-group">
              <TextInput v-model="formData.email"
              type="text"
              id="email"
              label="Adres email"
              :messages="hints.emailHints"
              @input="onChangeEmail"/>
          </div>
          <div class="form-group">
              <TextInput v-model="formData.password"
              type="password"
              id="password"
              label="Hasło"
              :messages="hints.passwordHints"
              @input="onChangePassword"/>
          </div>
          <div class="form-group">
              <TextInput v-model="formData.retypePassword"
              type="password"
              id="retypepassword"
              label="Powtórz hasło"
              :messages="hints.retypePasswordHints"
              @input="onChangeRetypePassword"/>
          </div>
          <button type="submit" v-if="isFormValid">Zarejestruj</button>
          <p></p>
          <div><RouterLink to="signin">Chcę się zalogować</RouterLink></div>
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
