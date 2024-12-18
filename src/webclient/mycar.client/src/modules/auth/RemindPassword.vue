<template>
  <div class="remindpassword-form">
      <h2>Reset hasła</h2>
      <form @submit.prevent="remindPassword(formData)">
        <div class="form-group">
              <TextInput v-model="formData.email"
                type="text"
                id="email"
                label="Email"
                :messages="emailMessages"
                @input="onChangeEmail"/>
            </div>
          <button type="submit" v-if="isFormValid">Wyślij</button>
          <p></p>
          <div><RouterLink to="signin">Chcę się zalogować</RouterLink></div>
          <div><RouterLink to="signup">Chcę się zarejestrować</RouterLink></div>
          <HintList :messages="errors"/>

      </form>
  </div>
</template>

<script setup lang="ts">
    import { ref, watchEffect } from 'vue';
    import type IRemindPasswordCommand from './requests/remaindpassword-command';
    import TextInput from "@/components/TextInput.vue";
    import HintList from '@/components/HintList.vue';
    import { useAuthStore } from '@/stores/AuthStore';
    import MessageProvider from '@/infrastructure/messageProvider';
    import { AxiosError, isAxiosError, type AxiosResponse } from 'axios';
    import {isApiError, type IApiError} from '@/types/IApiError';
    import { isValidEmail } from '@/helpers/email-validator'


    const errors = ref<string[]>([]);
    const emailMessages= ref<string[]>([]);
    const authStore = useAuthStore();
    const isFormValid = ref(false);

    const formData = ref<IRemindPasswordCommand>({
        email: '',
    });

    async function remindPassword(data: {email: string}) {
      const { email } = data;
      const body: IRemindPasswordCommand = {email};
      const messageProvider = new MessageProvider("forgotPassword");
      await messageProvider.Initialize();
      errors.value = [];
      emailMessages.value = [];

      try {
        await authStore.forgotPassword(body);
      } catch (error) {
          console.error('Remind failed', error);
      }
    };

    const onChangeEmail = (value: string) => {
      formData.value.email = value;
      emailHintFlag.value = !isValidEmail(value);
    }

    watchEffect(() => {
        isFormValid.value = isValidEmail(formData.value.email);
    });

</script>



<style scoped>
    .remindpassword-form {
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
