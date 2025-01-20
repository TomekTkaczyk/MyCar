<template>
  <nav class="navbar navbar-dark bg-dark navbar-expand-md" role="navigation">
      <div class="container-fluid">
          <RouterLink class="navbar-brand" to="/">MyCar</RouterLink>
          <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent"
                  aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
              <span class="navbar-toggler-icon"></span>
          </button>
          <div class="collapse navbar-collapse" id="navbarSupportedContent">
              <ul class="navbar-nav me-auto mb-lg-0">
                <li class="nav-item" v-if="authStore.isAuthenticated">
                      <RouterLink class="nav-link" to="/UserManager">Użytkownicy</RouterLink>
                </li>
                <li class="dropdown nav-item" v-if="false">
                      <a class="nav-link dropdown-toggle" v-if="privilege.anotherAction" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                          Agenci
                      </a>
                      <ul class="dropdown-menu">
                          <li><RouterLink class="dropdown-item" :to="{name:'Home'}">Kartoteka</RouterLink></li>
                          <li><RouterLink class="dropdown-item" :to="{name:'Home'}">Struktura</RouterLink></li>
                      </ul>
                </li>
              </ul>
              <ul class="navbar-nav ms-auto mb-lg-0" v-if="authStore.isAuthenticated">
                <li class="dropdown nav-item">
                      <a class="nav-link dropdown-toggle" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                          Witaj &nbsp
                          <span class="navbar-brand">
                            {{ authStore.name }}
                          </span>
                          <span class="text-danger" v-if="!authStore.isConfirmed">
                            (nie potwierdzony)
                          </span>
                      </a>
                      <ul class="dropdown-menu">
                          <li><RouterLink class="dropdown-item" to="/MyProfile">Mój profil</RouterLink></li>
                          <a href="#" class="dropdown-item" @click="logout">Wyloguj</a>
                      </ul>
                  </li>
              </ul>
              <ul class="navbar-nav ms-auto mb-lg-0" v-if="!authStore.isAuthenticated">
                  <li class="nav-item">
                      <RouterLink class="nav-link" to="signIn">Zaloguj</RouterLink>
                  </li>
              </ul>
          </div>
      </div>
  </nav>
</template>

<script setup lang="ts">
    import { useAuthStore } from '@/stores/AuthStore';
    import { ref } from 'vue';

    const authStore = useAuthStore();

    interface IPrivilege {
      users: boolean,
      agents: boolean;
      anotherAction: boolean;
    }

    const privilege = ref<IPrivilege>(
      {
        users: authStore.isAuthenticated && authStore.role === "admin",
        agents: authStore.isAuthenticated,
        anotherAction: true
      }
    );

    const logout = () => authStore.logout();
</script>

<style>
    a[class*=dropdown-item] {
        cursor: pointer;
    }

    .navbar-brand {
      padding: auto;
    }
</style>
