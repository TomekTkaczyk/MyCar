<!-- ***************************************************  -->
<!-- * Script section                                  *  -->
<!-- ***************************************************  -->

<script setup lang="ts">
    import { reactive, ref, watchEffect } from 'vue';
    import type IChangeEmailCommand from './requests/changeemail-command.ts';
    import TextInput from "@/components/TextInput.vue";
    import HintList from '@/components/HintList.vue';
    import { useAuthStore } from '@/stores/AuthStore';
    import { isValidEmail } from '@/helpers/email-validator'
    import MessageProvider from '@/infrastructure/messageProvider';
    import { isAxiosError, type AxiosError, type AxiosResponse } from 'axios';
    import {isApiError, type IApiError} from '@/types/IApiError';
import { FormErrors } from '@/types/FormErrors.ts';

    const authStore = useAuthStore();

    const isFormValid = ref(false);

    const errors = reactive<FormErrors>(new FormErrors);

    const emailMessages = ref<string[]>([]);

    const formData = ref(authStore.email as string);

    async function changeEmail(email: string) {
      try {
        if(isFormValid) {
          await authStore.changeEmail(email);
        }
      } catch (error) {
        await errors.CatchApiError(error);
      }
    };

    const onChangeEmail = (value: string) => {
      formData.value = value;
      if(!isValidEmail(formData.value)){
        emailMessages.value.push("Wymagany jest prawidłowy adres email.");
      }
    }

    const emailValidate = (value: string): boolean => {
      errors.Clear("Email");
      errors.messages.length = 0;
      if(!isValidEmail(formData.value)) {
        errors.Add("Email", "Wymagany prawidłowy adres eamil.");
      };
      return errors.Get("Email").length === 0;
    };

    watchEffect(() => {
        isFormValid.value = errors.Count === 0 &&
          formData.value.length > 0;
    });

</script>

<!-- ***************************************************  -->
<!-- * Template section                                *  -->
<!-- ***************************************************  -->

<template>
  <div>
      <h4>Zmiana adresu email</h4>
      <form @submit.prevent="changeEmail(formData)">
        <div class="form-group">
              <TextInput v-model="formData"
                type="text"
                id="email"
                label="Email"
                :messages="errors.Get('Email')"
                @input="onChangeEmail"/>
            </div>
          <button v-if="isFormValid" type="submit">Wyślij</button>
          <HintList :messages="errors.messages"/>
        </form>
  </div>
</template>

<!-- ***************************************************  -->
<!-- * Style section                                   *  -->
<!-- ***************************************************  -->

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

    button {
        padding: 10px 20px;
        background-color: #007bff;
        color: #fff;
        border: none;
        border-radius: 5px;
        cursor: pointer;
        margin-top: 10px;
        margin-bottom: 5px;
    }
</style>
