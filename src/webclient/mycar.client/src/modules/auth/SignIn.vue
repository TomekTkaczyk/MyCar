<!-- ***************************************************  -->
<!-- * Script section                                  *  -->
<!-- ***************************************************  -->

<script setup lang="ts">
  import { ref } from 'vue';
  import type ISignInCommand from './requests/signin-command';
  import TextInput from "@/components/TextInput.vue";
  import HintList from '@/components/HintList.vue';
  import MessageProvider from '@/infrastructure/messageProvider';
  import { isApiError } from '@/types/IApiError';
  import { useAuthStore } from '@/stores/AuthStore';

  const errors = ref<string[]>([]);
  const identifierMessages = ref<string[]>([]);
  const passwordMessages = ref<string[]>([]);

  const authStore = useAuthStore();

  const touchedFields = ref({
    identifier: false,
    password: false,
  });

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
      if(validateForm()) {
        await authStore.signInUser(body);
      }
    } catch (error: any) {
        if(isApiError(error)){
          error.validationErrors.forEach((value) => {
            const {code, message} = value;
            errors.value.push(messageProvider.GetMessage({field, code, message}));
          });
          const {code, message} = error;
          errors.value = [messageProvider.GetMessage({code, message})];
        } else {
          errors.value = ["Nierozpoznany błąd systemowy."];
        };
    };
  }

  const onChangeIdentifier = (value: string) => {
    formData.value.identifier = value;
    touchedFields.value.identifier = true;
    identifierMessages.value = [];
    if(!(value.length > 0)){
      identifierMessages.value.push("Wymagany jest identyfikator lub adres email.");
    }
  };

  const onChangePassword = (value: string) => {
    formData.value.password = value;
    touchedFields.value.password = true;
    passwordMessages.value = [];
    if(!(value.length > 0)) passwordMessages.value.push("Hasło jest wymagane.");
  };

  const identifierValid = (value: string) => {
    identifierMessages.value = [];
    if (!value) identifierMessages.value.push("Identyfikator jest wymagany.");

    return identifierMessages.value.length === 0;
  }

  const passwordValid = (value: string) => {
    passwordMessages.value = [];
    if ((!value) && touchedFields.value.identifier) passwordMessages.value.push("Hasło jest wymagane.");

    return passwordMessages.value.length === 0;
  }

  const validateForm = () => {
    const { identifier, password } = formData.value;
    return (
      (identifierValid(identifier)) &&
      (passwordValid(password))
    );
  };
</script>

<!-- ***************************************************  -->
<!-- * Template section                                *  -->
<!-- ***************************************************  -->

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
        <button type="submit">Zaloguj</button>
        <p></p>
        <div><RouterLink to="signup">Chcę się zarejestrować</RouterLink></div>
        <div><RouterLink to="remindpassword">Nie pamiętam hasła</RouterLink></div>
        <HintList style="margin-top: 10px;" :messages="errors"/>
      </form>
  </div>
</template>

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
