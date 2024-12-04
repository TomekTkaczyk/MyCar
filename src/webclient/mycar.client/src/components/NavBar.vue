<script setup lang="ts">
    import { useAuthStore } from '@/stores/AuthStore';

    const authStore = useAuthStore();

    //const logout = () => {
    //    authStore.logout();
    //}

</script>

<template>
    <nav class="navbar navbar-dark bg-dark navbar-expand-md" role="navigation">
        <div class="container-fluid">
            <RouterLink class="navbar-brand" to="/">AGK</RouterLink>
            <button class="navbar-toggler" v-if="authStore.isAuthenticated" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarSupportedContent">
                <ul class="navbar-nav me-auto mb-lg-0">
                    <li class="nav-item" v-if="authStore.isAuthenticated">
                        <!-- <RouterLink class="nav-link" :to="{name:'/Home', query: { pageNumber: 1, pageSize: 15, textSearch: '' }}">Klienci</RouterLink> -->
                    </li>
                    <!-- <li class="dropdown nav-item" v-if="authStore.isAuthenticated">
                        <a class="nav-link dropdown-toggle" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                            Agenci
                        </a>
                        <ul class="dropdown-menu">
                            <li><RouterLink class="dropdown-item" :to="{name:'/Home'}">Kartoteka</RouterLink></li>
                            <li><RouterLink class="dropdown-item" :to="{name:'/Home'}">Struktura</RouterLink></li>
                        </ul>
                    </li> -->
                    <li class="nav-item" v-if="authStore.isAuthenticated">
                        <RouterLink class="nav-link" to="/Home">Polisy</RouterLink>
                    </li>
                    <li class="nav-item" v-if="authStore.isAuthenticated">
                        <RouterLink to="/About" class="nav-link" exact="false">Gotówka</RouterLink>
                    </li>
                    <li class="dropdown nav-item" v-if="authStore.isAuthenticated">
                        <a class="nav-link dropdown-toggle" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                            Rozliczenia
                        </a>
                        <ul class="dropdown-menu">
                            <li><RouterLink class="dropdown-item" to="/Home">Banki</RouterLink></li>
                            <li><RouterLink class="dropdown-item" to="/Home">Prowizje</RouterLink></li>
                        </ul>
                    </li>
                    <li class="nav-item" v-if="authStore.isAuthenticated">
                        <RouterLink class="nav-link" to="/Home">Użytkownicy</RouterLink>
                    </li>
                    <li class="nav-item" v-if="authStore.isAuthenticated">
                        <RouterLink class="nav-link" to="/Home">Ustawienia</RouterLink>
                    </li>
                </ul>
                <ul class="navbar-nav ms-auto mb-lg-0">
                    <li class="nav-item dropdown" v-if="authStore.isAuthenticated">
                        <a class="nav-link dropdown-toggle" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                            Witaj User
                        </a>
                        <ul class="dropdown-menu">
                            <li><RouterLink class="dropdown-item" to="/Home">Mój profil</RouterLink></li>
                            <li><RouterLink class="dropdown-item" to="/Home">Wyloguj</RouterLink></li>
                        </ul>
                    </li>
                </ul>
            </div>
        </div>
    </nav>
</template>


<style>
    a[class*=dropdown-item] {
        cursor: pointer;
    }
</style>