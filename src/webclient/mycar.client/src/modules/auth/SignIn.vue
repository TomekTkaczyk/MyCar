<template>
    <div class="signin-form">
        <h2>Logowanie użytkownika</h2>
        <form @submit.prevent="signInUser(formData)">
            <div class="form-group">
              <TextInput v-model="formData.identifier"
                type="text"
                id="identifier"
                label="Nazwa użytkownika (login/email)"
                :messages="identifierMessages"
                @input="onChangeIdentifier"/>
            </div>
            <div class="form-group">
              <TextInput v-model="formData.password"
                type="password"
                id="password"
                label="Hasło"
                :messages="passwordMessages"
                @input="onChangePassword"/>
            </div>
            <button type="submit" v-if="isFormValid">Zaloguj</button>
            <p></p>
            <div><RouterLink to="signup">Chcę się zarejestrować</RouterLink></div>
            <div><RouterLink to="remindpassword">Nie pamiętam hasła</RouterLink></div>
            <HintList :messages="errors"/>
        </form>
    </div>
</template>

<script setup lang="ts">
    import { ref, watchEffect } from 'vue';
    import type ISignInCommand from './requests/signin-command';
    import TextInput from "@/components/TextInput.vue";
    import HintList from '@/components/HintList.vue';
    import { useAuthStore } from '@/stores/AuthStore';
    import MessageProvider from '@/infrastructure/messageProvider';
    import { AxiosError, isAxiosError, type AxiosResponse } from 'axios';
    import {isApiError, type IApiError} from '@/types/IApiError';


    const errors = ref<string[]>([]);
    const identifierMessages= ref<string[]>([]);
    const passwordMessages = ref<string[]>([]);
    const authStore = useAuthStore();
    const isFormValid = ref(false);

    const formData = ref<ISignInCommand>({
        identifier: '',
        password: '',
    });

    async function signInUser(data: ISignInCommand) {
      const { identifier, password } = data;
      const body: ISignInCommand = {identifier, password};
      const messageProvider = new MessageProvider("signIn");
      await messageProvider.Initialize();
      errors.value = [];
      identifierMessages.value = [];
      passwordMessages.value = [];

      try {
          await authStore.signInUser(body);
      } catch (error) {
        if(isAxiosError(error)){
          if(error.response && isApiError(error.response.data)){
            error.response.data.errors.forEach((value) => {
              errors.value.push(messageProvider.GetMessage(value.code),);
            });
          } else {
            errors.value = [error.message];
            console.error(error);
          };
        } else {
          errors.value = ["Nierozpoznany błąd systemowy."];
          console.error(error);
        }
    };
  }

    const onChangeIdentifier = (value: string) => {
      formData.value.identifier = value;
      identifierMessages.value = [];
      if(!(value.length > 0)){
        identifierMessages.value.push("Wymagany jest identyfikator lub adres email.");
      }
    };

    const onChangePassword = (value: string) => {
      formData.value.password = value;
      passwordMessages.value = [];
      if(!(value.length > 0)){
        passwordMessages.value.push("Hasło jest wymagane.");
      }
    };

    const validateForm = () => {
      const { identifier, password } = formData.value;
      return identifier !== '' && password !== '';
    };

    watchEffect(() => {
      isFormValid.value = validateForm();
    });

</script>

<style scoped>

.signin-form {
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

button {
    padding: 10px 20px;
    background-color: #007bff;
    color: #fff;
    border: none;
    border-radius: 5px;
    cursor: pointer;
}

</style>
