<template>
    <div class="signin-form">
        <h2>Logowanie użytkownika</h2>
        <form @submit.prevent="signInUser(formData)">
            <div class="form-group">
              <TextInput v-model="formData.userName"
                type="text"
                id="username"
                label="Nazwa użytkownika"
                hint="Nazwa użytkownika jest wymagana"
                :showHint="hints.userNameHintFlag"
                @input="onChangeUsername"/>
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
      userNameHintFlag: boolean,
      passwordHintFlag: boolean,
    }

    const hints = ref<Hints>({
      userNameHintFlag: false,
      passwordHintFlag: false,
    });

    const formData = ref<ISignInCommand>({
        userName: '',
        password: '',
    });

    let isFormValid = ref(false);

    const authStore = useAuthStore();

    async function signInUser(data: ISignInCommand) {
      const { userName, password } = data;
      const body: ISignInCommand = {userName, password};
      try {
          await authStore.signInUser(body);
      } catch (error) {
          console.error('Login failed', error);
      }
    };

    const onChangeUsername = (value: string) => {
      formData.value.userName = value;
      hints.value.userNameHintFlag = formData.value.userName === '';
    };

    const onChangePassword = (value: string) => {
      formData.value.password = value;
      hints.value.passwordHintFlag = formData.value.password === '';
    };

    const validateForm = () => {
      const { userName, password } = formData.value;
      return userName !== '' && password !== '';
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
