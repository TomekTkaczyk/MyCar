<template>
    <div class="signin-form">
        <h2>Logowanie użytkownika</h2>
        <form @submit.prevent="signInUser(formData)">
            <div class="form-group">
              <TextInput v-model="formData.identifier"
                type="text"
                id="identifier"
                label="Nazwa użytkownika (login/email)"
                hint="Nazwa użytkownika jest wymagana"
                :showHint="hints.identifierHintFlag"
                @input="onChangeIdentifier"/>
            </div>
            <div class="form-group">
              <TextInput v-model="formData.password"
                type="password"
                id="password"
                label="Hasło"
                hint="Hasło jest wymagane"
                :showHint="hints.passwordHintFlag"
                @input="onChangePassword"/>
            </div>
            <button type="submit" v-if="isFormValid">Zaloguj</button>
            <p></p>
            <div><RouterLink to="signup">Chcę się zarejestrować</RouterLink></div>
            <div><RouterLink to="remindpassword">Nie pamiętam hasła</RouterLink></div>
        </form>
    </div>
</template>

<script setup lang="ts">
    import { ref, watchEffect } from 'vue';
    import type ISignInCommand from './requests/signin-command';
    import TextInput from "@/components/TextInput.vue";
    import { useAuthStore } from '@/stores/AuthStore';

    interface Hints {
      identifierHintFlag: boolean,
      passwordHintFlag: boolean,
    }

    const hints = ref<Hints>({
      identifierHintFlag: false,
      passwordHintFlag: false,
    });

    const formData = ref<ISignInCommand>({
        identifier: '',
        password: '',
    });

    let isFormValid = ref(false);

    const authStore = useAuthStore();

    async function signInUser(data: ISignInCommand) {
      const { identifier, password } = data;
      const body: ISignInCommand = {identifier, password};
      try {
          await authStore.signInUser(body);
      } catch (error) {
          console.error('Login failed', error);
      }
    };

    const onChangeIdentifier = (value: string) => {
      formData.value.identifier = value;
      hints.value.identifierHintFlag = formData.value.identifier === '';
    };

    const onChangePassword = (value: string) => {
      formData.value.password = value;
      hints.value.passwordHintFlag = formData.value.password === '';
    };

    const validateForm = () => {
      const { identifier, password } = formData.value;
      return identifier !== '' && password !== '';
    };

    watchEffect(() => {
      isFormValid.value = validateForm();
    });

</script>

<style scoped>

.signin-form {
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
