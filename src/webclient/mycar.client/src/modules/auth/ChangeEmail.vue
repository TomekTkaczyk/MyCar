<template>
  <div class="changeemail-form">
      <h2>Zmiana adresu email</h2>
      <form @submit.prevent="changeEmail(formData)">
        <div class="form-group">
              <TextInput v-model="formData.email"
                type="text"
                id="email"
                label="Email"
                hint='Wymagany prawidłowy adres email'
                :showHint="emailHintFlag"
                @input="onChangeEmail"/>
            </div>
          <button type="submit" v-if="isFormValid">Wyślij</button>
          <p></p>
          <div><RouterLink to="signin">Chcę się zalogować</RouterLink></div>
          <div><RouterLink to="signup">Chcę się zarejestrować</RouterLink></div>
      </form>
  </div>
</template>

<script setup lang="ts">
    import { ref, watchEffect } from 'vue';
    import type IChangeEmailCommand from './requests/changeemail-command.ts';
    import TextInput from "@/components/TextInput.vue";
    import { useAuthStore } from '@/stores/AuthStore';
    import { isValidEmail } from '@/helpers/email-validator'

    const emailHintFlag = ref(false);

    const formData = ref<IChangeEmailCommand>({
        email: '',
    });

    const isFormValid = ref(false);

    const authStore = useAuthStore();

    async function changeEmail(data: {email: string}) {
      // Tutaj można wywołać funkcję do rejestracji użytkownika
      // np. poprzez wywołanie API, przekazując formData.value
      const { email } = data;
      try {
        await authStore.changeEmail(email);
      } catch (error) {
          console.error('Change email failed', error);
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
    }
</style>
