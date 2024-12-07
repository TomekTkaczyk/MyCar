
<template>
    <div class="signup-form">
        <h2>Rejestracja użytkownika</h2>
        <form @submit.prevent="signUpUser(formData)">
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
                <TextInput v-model="formData.email"
                type="text"
                id="email"
                label="Adres email"
                hint="Adres email jest wymagany"
                :showHint="hints.emailHintFlag"
                @input="onChangeEmail"/>
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
            <div class="form-group">
                <TextInput v-model="formData.retypePassword"
                type="password"
                id="retypepassword"
                label="Powtórz hasło"
                hint="Hasła nie są zgodne"
                :showHint="hints.retypePasswordHintFlag"
                @input="onChangeRetypePassword"/>
            </div>
            <button type="submit" v-if="isFormValid">Zarejestruj</button>
            <p></p>
            <div><RouterLink to="signin">Chcę się zalogować</RouterLink></div>
        </form>
    </div>
</template>

<script setup lang="ts">
    import { ref, watchEffect } from 'vue';
    import type ISignUpCommand from './requests/signup-command';
    import TextInput from "@/components/TextInput.vue";
    import { useAuthStore } from '@/stores/AuthStore';

    interface Hints {
      userNameHintFlag: boolean,
      emailHintFlag: boolean,
      passwordHintFlag: boolean,
      retypePasswordHintFlag: boolean
    }

    const hints = ref<Hints>({
      userNameHintFlag: false,
      emailHintFlag: false,
      passwordHintFlag: false,
      retypePasswordHintFlag: false
    });

    const formData = ref<ISignUpCommand & {retypePassword: string}>({
      userName: '',
      email: '',
      password: '',
      retypePassword: '',
    });

    async function signUpUser(data: ISignUpCommand) {
      const { userName, email, password } = formData.value;
      const body: ISignUpCommand = {userName, email, password};
      try {
          await authStore.signUpUser(body);
      } catch (error) {
          console.error('Register failed', error);
      }
    }

    const isFormValid = ref(false);

    const authStore = useAuthStore();

    const onChangeUsername = (value: string) => {
      formData.value.userName = value;
      hints.value.userNameHintFlag = formData.value.userName === '';
    };

    const onChangeEmail = (value: string) => {
      formData.value.email = value;
      hints.value.emailHintFlag = formData.value.email === '';
    };

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
      const { userName, email, password, retypePassword } = formData.value;
      return userName !== '' && email !== '' && password !== '' && password === retypePassword;
    };

    watchEffect(() => {
      isFormValid.value = validateForm();
    });

</script>

<style scoped>
    .signup-form {
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

</style>
