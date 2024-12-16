<template>
  <div>
      <h4>Zmiana hasła</h4>
      <form @submit.prevent="changePassword(formData)">
        <div class="form-group">
          <TextInput v-model="formData.currentPassword"
            type="password"
            id="currentpassword"
            label="Aktualne hasło"
            @input="onChangeCurrentPassword"/>
        </div>
        <div class="form-group">
          <TextInput v-model="formData.password"
            type="password"
            id="password"
            label="Nowe hasło"
            @input="onChangePassword"/>
        </div>
        <div class="form-group">
            <TextInput v-model="formData.retypePassword"
            type="password"
            id="retypepassword"
            label="Powtórz hasło"
            :messages="retypeMessages"
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

const formData = ref<IChangePasswordCommand & {retypePassword: string}>({
  currentPassword: '',
  password: '',
  retypePassword: '',
});

const retypeMessages = ref<string[]>(["aaa"]);

async function changePassword(data: IChangePasswordCommand) {
  const {currentPassword, password} = data;
  const body: IChangePasswordCommand = {currentPassword, password};
  try {
    await authStore.changePassword(body);
    retypeMessages.value = ["dfd"];
  } catch (error) {
    retypeMessages.value = ["dfd"];
    console.log("Change password failed", error);
  }
}

const isFormValid = ref(false);

const authStore = useAuthStore();

const onChangeCurrentPassword = (value:string ) => {
  formData.value.currentPassword = value;
}

const onChangePassword = (value: string) => {
  formData.value.password = value;
};

const onChangeRetypePassword = (value: string) => {
  formData.value.retypePassword = value;
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
