<script setup lang="ts">
    import { ref, watchEffect } from 'vue';
    import type ISignInCommand from './requests/signin-command';
    import { useAuthStore } from '@/stores/AuthStore';

    const formData = ref<ISignInCommand>({
        userName: '',
        password: '',
    });

    const authStore = useAuthStore();

    async function signInUser(body: ISignInCommand) {
        try {
            await authStore.signin(body);
        } catch (error) {
            console.error('Login failed', error);
        }
    };

</script>

<template>
    <div class="signin-form">
        <h2>Logowanie użytkownika</h2>
        <form @submit.prevent="signInUser(formData)">
            <div class="form-group">
                <label for="userName">Nazwa użytkownika (login):</label>
                <input id="userName" 
                       v-model="formData.userName" 
                       type="text" required />
            </div>
            <div class="form-group">
                <label for="password">Hasło:</label>
                <input id="password" 
                       v-model="formData.password"
                       type="password" 
                       required />
            </div>
            <button type="submit">Zaloguj</button>
            <div>Jeżeli nie posiadasz konta, <RouterLink to="signup">Zarejestruj się</RouterLink></div>
        </form>
    </div>
</template>

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

    .password-mismatch {
        color: red;
        margin-top: 5px;
    }
</style>
