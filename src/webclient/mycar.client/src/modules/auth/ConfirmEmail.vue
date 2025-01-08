<!-- ***************************************************  -->
<!-- * Script section                                  *  -->
<!-- ***************************************************  -->

<script setup lang='ts'>
import { ref } from 'vue';
import { useRoute } from 'vue-router';
import { useAuthStore } from '@/stores/AuthStore';
import type { ApiError } from '@/infrastructure/errors/ApiError';

const route = useRoute();
const email = route.query.Email as string;
const confirmToken = route.query.ConfirmToken as string;
const authStore = useAuthStore();

const isLoading = ref(false);
const isConfirmed = ref(false);
const errorMessage = ref<string | null>(null);
const buttonVisible = ref<boolean>(false);

const clickFunction = ref<() => Promise<void>>(() => Promise.resolve());

const confirmEmail = async () => {
  if (!email || !confirmToken) {
    errorMessage.value = 'Brak wymaganych danych do potwierdzenia e-maila.';
    return;
  }
  isLoading.value = true;
  errorMessage.value = null;
  try{
    await authStore.confirmEmail({ email, confirmToken });
    isConfirmed.value = true;
  } catch (error: any) {
    switch((error as ApiError).message){
      case "email_already_confirmed": {
        errorMessage.value = 'Adres email jest już potwierdzony.';
        break;
      }
      case "": {
        errorMessage.value = 'Adres email jest już potwierdzony.';
        clickFunction.value = resendConfirmEmail;
        break;
      }
      default:{
        errorMessage.value = error || 'Wystąpił błąd podczas potwierdzania e-maila.';
      }
    }
  } finally {
    isLoading.value = false;
  }
};

const resendConfirmEmail = async () => {
  if(!email) {
    errorMessage.value = 'Brak wymaganych danych do potwierdzenia e-maila.';
    return;
  }
  isLoading.value = true;
  errorMessage.value = null;
  try{
    await authStore.resendConfirmEmail(email);
    isLoading.value = true;
  } catch (error: any) {
    console.error(error);
    errorMessage.value = error || 'Wystąpił błąd podczas wysyłania żądania.';
  } finally {
    isLoading.value = false;
  }
}

clickFunction.value = confirmEmail;

confirmEmail();

</script>

<!-- ***************************************************  -->
<!-- * Template section                                *  -->
<!-- ***************************************************  -->

<template>
  <div class="confirm-form" >
    <div class="confirm-email">
      <div v-if="isLoading">Potwierdzanie e-maila...</div>
      <div v-else-if="isConfirmed">E-mail został pomyślnie potwierdzony!</div>
      <div v-else-if="errorMessage">
        <div class="error">
          {{ errorMessage }}
        </div>
        <br/>
        <button v-if="buttonVisible" v-on:click="clickFunction">Wyślij ponownie</button>
      </div>
      <div v-else>Oczekiwanie na potwierdzenie e-maila...</div>
    </div>
  </div>
</template>

<!-- ***************************************************  -->
<!-- * Style section                                   *  -->
<!-- ***************************************************  -->

<style scoped>
.confirm-form {
  max-width: 400px;
    margin: 0 auto;
    padding: 20px;
    border: 1px solid #ccc;
    border-radius: 5px;
}
.confirm-email {
  text-align: center;
  margin: 20px;
}

.error {
  color: red;
  font-weight: bold;
}
</style>
