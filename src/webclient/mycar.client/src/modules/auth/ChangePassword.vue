<template>
  <div>
      <h4>Zmiana hasła</h4>
      <form @submit.prevent="changePassword(formData)">
        <div class="form-group">
          <TextInput v-model="formData.currentPassword"
            type="password"
            id="currentpassword"
            label="Aktualne hasło"
            hint="Hasło jest wymagane"
            :show-hint="hints.currentPasswordHintFlag"
            @input="onChangeCurrentPassword"/>
        </div>
        <div class="form-group">
          <TextInput v-model="formData.password"
            type="password"
            id="password"
            label="Nowe hasło"
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
        <button class="btn btn-outline-primary" type="submit" v-if="isFormValid">Zmień hasło</button>
      </form>
  </div>
</template>

<script setup lang="ts">

import {ref, watchEffect} from 'vue';
import type IChangePasswordCommand from './requests/changepassword-command';
import TextInput from '@/components/TextInput.vue';
import { useAuthStore } from '@/stores/AuthStore';

interface Hints {
  currentPasswordHintFlag: boolean,
  passwordHintFlag: boolean,
  retypePasswordHintFlag: boolean,
}

const hints = ref<Hints>({
  currentPasswordHintFlag: false,
  passwordHintFlag: false,
  retypePasswordHintFlag: false,
});

const formData = ref<IChangePasswordCommand & {retypePassword: string}>({
  currentPassword: '',
  password: '',
  retypePassword: '',
});


async function changePassword(data: IChangePasswordCommand) {
  const {currentPassword, password} = data;
  const body: IChangePasswordCommand = {currentPassword, password};
  try{
    await authStore.changePassword(body);
  } catch (error) {
    console.log("Change password failed", error);
  }
}

const isFormValid = ref(false);

const authStore = useAuthStore();

const onChangeCurrentPassword = (value:string ) => {
  formData.value.currentPassword = value;
  hints.value.currentPasswordHintFlag = formData.value.currentPassword === '';
}

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
