<!-- ***************************************************  -->
<!-- * Script section                                  *  -->
<!-- ***************************************************  -->

<script setup lang="ts">
  import { computed, reactive, ref, watchEffect } from 'vue';
  import TextInput from "@/components/TextInput.vue";
  import HintList from '@/components/HintList.vue';
  import { useAuthStore } from '@/stores/AuthStore';
  import { isValidEmail } from '@/helpers/email-validator'
  import { FormErrors } from '@/types/FormErrors.ts';

  const authStore = useAuthStore();

  const isFormValid = ref(false);

  const touchedFields = ref({
    email: false,
  });

  const formData = ref(authStore.email as string);

  const errors = reactive<FormErrors>(new FormErrors);

  const emailLabel = computed(() => {return authStore.isConfirmed ? "Email" : "Email (nie potwierdzony)"});

  async function changeEmail(email: string) {
    touchedFields.value.email = false;
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
    touchedFields.value.email = true;
    emailValidate(value);
  }

  const emailValidate = (value: string): boolean => {
    errors.Clear("Email");
    errors.messages.length = 0;
    if((touchedFields.value.email) && !isValidEmail(value)) {
      errors.Add("Email","Wymagany prawidłowy adres email.");
    }
    return errors.Get("Email").length === 0;
  };

  watchEffect(() => {
    isFormValid.value = errors.Count === 0 && touchedFields.value.email;
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
                :label="emailLabel"
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
      margin-bottom: 1px;
      margin-top: 5px;
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

  .small-text {
    font-size: 0.85em;
    margin-top: 2px;
  }

</style>
