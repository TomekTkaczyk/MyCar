<template>
  <div>
    <p>Wylogowanie...</p>
  </div>
</template>

<script setup lang="ts">
import {useAuthStore} from "@/stores/AuthStore";
import router from "@/router"
import { onMounted } from "vue";

const authStore = useAuthStore();

onMounted(async () => {
  try{
    // await authStore.logout("");
    router.push({path: "/SignIn"});
  } catch(error) {
    console.error("My Error: ",error);
  };
});

</script>
