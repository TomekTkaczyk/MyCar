<template>
  <div class="updateprofile-form">
    <h4>Zmiana nazwy</h4>
    <form @submit.prevent="updateProfile(formData)">
        <div class="form-group">
              <TextInput v-model="formData.firstName"
                type="text"
                id="firstname"
                label="Imię"
                @input="onChangeFirstName"/>
            </div>
            <div class="form-group">
              <TextInput v-model="formData.lastName"
                type="text"
                id="lastname"
                label="Nazwisko"
                @input="onChangeLastName"/>
            </div>
            <button class="btn btn-outline-primary" type="submit">Wyślij</button>
      </form>
  </div>
</template>

<script setup lang="ts">
    import { ref, watchEffect } from 'vue';
    import type IUpdateProfileCommand from './requests/updateprofile-command.ts';
    import TextInput from "@/components/TextInput.vue";
    import { useAuthStore } from '@/stores/AuthStore';

    const authStore = useAuthStore();

    const formData = ref<IUpdateProfileCommand>({
        firstName: authStore.firstName as string || '',
        lastName: authStore.lastName as string || '',
    });

    const isFormValid = ref(false);

    async function updateProfile(data: IUpdateProfileCommand) {
      // Tutaj można wywołać funkcję do rejestracji użytkownika
      // np. poprzez wywołanie API, przekazując formData.value
      const { firstName, lastName } = data;
      const body: IUpdateProfileCommand = {firstName, lastName};
      await authStore.updateProfile(body);
    };

    const onChangeFirstName = (value: string) => {
      formData.value.firstName = value;
    }

    const onChangeLastName = (value: string) => {
      formData.value.lastName = value;
    }
</script>



<style scoped>
    .form-group {
        margin-bottom: 10px;
    }

    label {
        display: block;
        margin-bottom: 5px;
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
