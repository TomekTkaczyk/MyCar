<template>
</template>

<script setup lang="ts">
import {useAuthStore} from "@/stores/AuthStore";
import router from "@/router"
import { onMounted } from "vue";

const authStore = useAuthStore();

onMounted(async () => {
  try{
    await authStore.logout();
  } catch(error) {
    console.error("My Error: ",error);
  };
});

</script>
