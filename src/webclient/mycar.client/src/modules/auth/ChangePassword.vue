<template>
  <div class="changepassword-form">
      <h2>Zmiana hasła</h2>
      <form @submit.prevent="changePassword(formData)">
        <div class="form-group">
          <TextInput v-model="formData.password"
            type="password"
            id="pass1"
            label="Hasło"
            hint="Hasło jest wymagane"
            :show-hint="hints.passwordHintFlag"
            @input="onChangePassword"/>
        </div>
        <div class="form-group">
                <TextInput v-model="formData.retypePassword"
                type="password"
                id="retypepassword"
                label="Powtórz hasło"
                hint="Hasła nie są zgodne"
                :showHint="hints.retypePasswordHintFlag"
                @input="onChangeRetypePassword"/>
            </div>
            <button type="submit" v-if="isFormValid">Zmień hasło</button>
      </form>
  </div>
</template>

<script setup lang="ts">

import {ref, watchEffect} from 'vue';
import type IChangePasswordCommand from './requests/changepassword-command.ts';
import TextInput from '@/components/TextInput.vue';
import { useAuthStore } from '@/stores/AuthStore';

interface Hints {
  passwordHintFlag: boolean,
  retypePasswordHintFlag: boolean,
}

const hints = ref<Hints>({
  passwordHintFlag: false,
  retypePasswordHintFlag: false,
});

const formData = ref<IChangePasswordCommand & {retypePassword: string}>({
  password: '',
  retypePassword: '',
});

async function changePassword(data: IChangePasswordCommand) {
  const {password} = data;
  const body: IChangePasswordCommand = {password};
  try{
    await authStore.changePassword(body.password);
  } catch (error) {
    console.log("Change password failed", error);
  }
}

const isFormValid = ref(false);

const authStore = useAuthStore();

const onChangePassword = (value: string) => {
  formData.value.password = value;
      hints.value.passwordHintFlag = formData.value.password === '';
      hints.value.retypePasswordHintFlag = formData.value.password !== formData.value.retypePassword;
};

const onChangeRetypePassword = (value: string) => {
  formData.value.retypePassword = value;
      hints.value.retypePasswordHintFlag = formData.value.password !== formData.value.retypePassword;
};

const validateForm = () => {
      const { password, retypePassword } = formData.value;
      return password !== '' && password === retypePassword;
    };

watchEffect(() => {
  isFormValid.value = validateForm();
});

</script>

<style scoped>
    .changepassword-form {
        max-width: 400px;
        margin: 0 auto;
        padding: 20px;
        border: 1px solid #ccc;
        border-radius: 5px;
    }

    .form-group {
    margin-bottom: 15px;
}
</style>
