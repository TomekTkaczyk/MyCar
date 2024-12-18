<template>
  <div>
      <h4>Zmiana adresu email</h4>
      <form @submit.prevent="changeEmail(formData)">
        <div class="form-group">
              <TextInput v-model="formData.email"
                type="text"
                id="email"
                label="Email"
                :messages="emailMessages"
                @input="onChangeEmail"/>
            </div>
          <button class="btn btn-outline-primary" type="submit" v-if="isFormValid">Wyślij</button>
          <HintList :messages="errors"/>
        </form>
  </div>
</template>

<script setup lang="ts">
    import { ref, watchEffect } from 'vue';
    import type IChangeEmailCommand from './requests/changeemail-command.ts';
    import TextInput from "@/components/TextInput.vue";
    import { useAuthStore } from '@/stores/AuthStore';
    import HintList from '@/components/HintList.vue';
    import { isValidEmail } from '@/helpers/email-validator'
    import MessageProvider from '@/infrastructure/messageProvider';
    import { isAxiosError, type AxiosError, type AxiosResponse } from 'axios';
    import {isApiError, type IApiError} from '@/types/IApiError';

    const errors = ref<string[]>([]);
    const emailMessages = ref<string[]>([]);
    const authStore = useAuthStore();
    const isFormValid = ref(false);

    const formData = ref<IChangeEmailCommand>({
      email: authStore.email as string,
    });

    interface errordata {
      code:string,
      message:string,
    }

    async function changeEmail(data: IChangeEmailCommand) {
      const {email} = data;
      const body: IChangeEmailCommand = { email };
      const messageProvider = new MessageProvider("changeEmail");
      await messageProvider.Initialize();
      errors.value = [];
      emailMessages.value = [];
      try {
        await authStore.changeEmail(body);
      } catch (error) {
        if(isAxiosError(error)){
          if(error.response && isApiError(error.response.data)){
            error.response.data.errors.forEach((value) => {
              emailMessages.value.push(messageProvider.GetMessage(value.code),);
            });
          } else {
            errors.value = [error.message];
            console.error(error);
          };
        } else {
          errors.value = ["Nierozpoznany błąd systemowy."];
          console.error(error);
        }
      }
    };

    const onChangeEmail = (value: string) => {
      formData.value.email = value;
      emailMessages.value = [];
      if(!isValidEmail(formData.value.email)){
        emailMessages.value.push("Wymagany jest prawidłowy adres email.");
      }
    }

    const validateForm = () => {
      return isValidEmail(formData.value.email);
    };

    watchEffect(() => {
        isFormValid.value = validateForm();
    });

</script>



<style scoped>
    .changeemail-form {
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
