<!-- ***************************************************  -->
<!-- * Script section                                  *  -->
<!-- ***************************************************  -->

<script setup lang="ts">
  import {computed, ref } from 'vue';
  import { useRoute, useRouter } from 'vue-router';
  import { useAuthStore } from '@/stores/AuthStore';
  import UpdateProfile from './UpdateProfile.vue';
  import ChangeEmail from './ChangeEmail.vue';
  import ChangePassword from './ChangePassword.vue';

  const componentsMap: Record<OptionKey,any> = {
    name: UpdateProfile,
    email: ChangeEmail,
    pass: ChangePassword,
  };

  const authStore = useAuthStore();
  const route = useRoute();

  type OptionKey = "name" | "email" | "pass";
  const selectedOption = ref<OptionKey>((route.params.option as OptionKey));

  const currentComponent = computed(() => {
    if(privilege.value[selectedOption.value]){
      return componentsMap[selectedOption.value]
    } else {
      const isApprove = Object.keys(privilege.value).find((key) =>
          privilege.value[key as OptionKey]) as OptionKey;
      return componentsMap[isApprove];
    }
  });

  interface IPrivilege {
    name: boolean,
    email: boolean,
    pass: boolean,
  };

  const privilege = ref<IPrivilege>({
    name: authStore.role !== "admin",
    email: authStore.role !== "admin",
    pass: true,
  } as IPrivilege);

</script>

<!-- ***************************************************  -->
<!-- * Template section                                *  -->
<!-- ***************************************************  -->

<template>
  <div class="container-fluid">
    <div class="row justify-content-center">
      <div class="myprofile-form col-12 col-sm-12 col-md-4 col-lg-3 col-xxl-2">
          <h4>Moje konto</h4>
          <div class="btn-group-vertical" role="group" aria-label="toggle button group">
            <input v-model="selectedOption" value="name" type="radio" class="btn-check" name="btnradio" id="option1" autocomplete="off">
            <label v-if="privilege.name" class="btn text-start" for="option1">Nazwa</label>
            <input v-model="selectedOption" value="email" type="radio" class="btn-check" :class="{ 'is-invalid': !authStore.isConfirmed }" name="btnradio" id="option2" autocomplete="off">
            <label v-if="privilege.email" class="btn text-start" :class="{ 'text-danger': !authStore.isConfirmed }" for="option2">Email</label>
            <input v-model="selectedOption" value="pass" type="radio" class="btn-check" name="btnradio" id="option3" autocomplete="off">
            <label v-if="privilege.pass" class="btn text-start" for="option3">Hasło</label>
          </div>
      </div>
      <div class="myprofile-content col-12 col-sm-12 col-md-7 col-lg-5 col-xl-4 col-xxl-3">
        <component :is="currentComponent" />
      </div>
    </div>
  </div>
</template>

<!-- ***************************************************  -->
<!-- * Style section                                   *  -->
<!-- ***************************************************  -->

<style scoped>
    .myprofile-form {
      padding: 10px;
      border: 1px solid #ccc;
      border-radius: 5px;
      margin: 5px;
    }

    .myprofile-content {
      padding: 10px;
      border: 1px solid #ccc;
      border-radius: 5px;
      margin: 5px;
    }

    .btn-check:checked + .btn {
      border-radius: 0.5rem; /* Lub dowolna wartość, np. 4px */
      border: 1px solid #ccc;
    }
</style>
