<script setup lang="ts">
    import { ref, watchEffect } from 'vue';

    const formData = ref({
        username: '',
        email: '',
        password: '',
        retypePassword: ''
    });

    const passwordsMatch = ref(true);

    const isFormValid = ref(false);

    const signOutUser = () => {
        if (formData.value.password !== formData.value.retypePassword) {
            passwordsMatch.value = false;
            return;
        }

        // Tutaj można wywołać funkcję do rejestracji użytkownika
        // np. poprzez wywołanie API, przekazując formData.value
        console.log("register");

        // Czyść dane formularza po rejestracji
        clearForm();
    };

    const clearForm = () => {
        formData.value.username = '';
        formData.value.email = '';
        formData.value.password = '';
        formData.value.retypePassword = '';
        passwordsMatch.value = true;
    };

    // Funkcja sprawdzająca, czy formularz jest poprawnie wypełniony
    const validateForm = () => {
        const { username, email, password, retypePassword } = formData.value;
        return username !== '' && email !== '' && password !== '' && retypePassword !== '' && passwordsMatch.value;
    };

    // Obserwacja zmian w danych formularza
    watchEffect(() => {
        isFormValid.value = validateForm();
    });
</script>

<template>
    <div class="signout-form">
        <h2>Rejestracja użytkownika</h2>
        <form @submit.prevent="signOutUser">
            <div class="form-group">
                <label for="username">Nazwa użytkownika (login):</label>
                <input type="text" id="username" v-model="formData.username" required />
            </div>
            <div class="form-group">
                <label for="email">Email:</label>
                <input type="text" id="email" v-model="formData.email" required />
            </div>
            <div class="form-group">
                <label for="password">Hasło:</label>
                <input type="password" id="password" v-model="formData.password" required />
            </div>
            <div class="form-group">
                <label for="retypePassword">Powtórz hasło:</label>
                <input type="password" id="retypePassword" v-model="formData.retypePassword" required />
            </div>
            <div v-if="!passwordsMatch" class="password-mismatch">Hasła nie pasują do siebie.</div>
            <button type="submit" v-if="isFormValid">Zarejestruj</button>
            <button type="submit" v-else disabled>Zarejestruj</button>
            <div>Jeżeli posiadasz już konto, <RouterLink to="signin">Zaloguj się</RouterLink></div>
        </form>
    </div>
</template>


<style scoped>
    .signout-form {
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
