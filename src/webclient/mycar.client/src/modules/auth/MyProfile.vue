<template>
  <div class="container-fluid">
    <div class="row justify-content-center">
      <div class="myprofile-form col-12 col-sm-12 col-md-4 col-lg-3 col-xxl-2">
          <h4>Moje konto</h4>
          <div class="btn-group-vertical" role="group" aria-label="toggle button group">
            <input v-model="selectedOption" value="name" type="radio" class="btn-check" name="btnradio" id="option1" autocomplete="off">
            <label class="btn text-start" for="option1">Nazwa</label>
            <input v-model="selectedOption" value="email" type="radio" class="btn-check" :class="{ 'is-invalid': !authStore.isConfirmed }" name="btnradio" id="option2" autocomplete="off">
            <label class="btn text-start" :class="{ 'text-danger': !authStore.isConfirmed }" for="option2">Email</label>
            <input v-model="selectedOption" value="pass" type="radio" class="btn-check" name="btnradio" id="option3" autocomplete="off">
            <label class="btn text-start" for="option3">Hasło</label>
          </div>
      </div>
      <div class="myprofile-content col-12 col-sm-12 col-md-7 col-lg-5 col-xl-4 col-xxl-3">
        <component :is="currentComponent" />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import {computed, ref} from 'vue';
  import UpdateProfile from './UpdateProfile.vue';
  import ChangeEmail from './ChangeEmail.vue';
  import ChangePassword from './ChangePassword.vue';
import { useAuthStore } from '@/stores/AuthStore';

  type OptionKey = "name" | "email" | "pass";
  const componentsMap: Record<OptionKey,any> = {
    name: UpdateProfile,
    email: ChangeEmail,
    pass: ChangePassword,
  };
  const selectedOption = ref<OptionKey>("name");

  const authStore = useAuthStore();

  const currentComponent = computed(() => componentsMap[selectedOption.value]);

</script>

<style scoped>
    .myprofile-form {
      padding: 10px;
      border: 1px solid #ccc;
      border-radius: 5px;
      margin: 5px;
    }

    .myprofile-content {
      padding: 10px;
      border: 1px solid #ccc;
      border-radius: 5px;
      margin: 5px;
    }

    .btn-check:checked + .btn {
      border-radius: 0.5rem; /* Lub dowolna wartość, np. 4px */
      border: 1px solid #ccc;
    }
</style>
