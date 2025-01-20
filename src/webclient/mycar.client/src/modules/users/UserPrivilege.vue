<!-- ***************************************************  -->
<!-- * Script section                                  *  -->
<!-- ***************************************************  -->

<script setup lang='ts'>

import { onMounted, reactive, ref, watchEffect } from 'vue';
import type IUpdatePrivilegeCommand from './requests/updateprivilege-command.ts';
import ComboBox from '@/components/ComboBox.vue';
import { FormErrors } from '@/types/FormErrors.ts';
import { useRoute, useRouter } from 'vue-router';
import { useUserStore } from '@/stores/UserStore.ts';
import ErrorHandler from '@/stores/ErrorHandler.ts';

const userStore = useUserStore();
const route = useRoute();
const router = useRouter();
const errorHandle = ErrorHandler;

const props = ref(route.params.id);
const isFormValid = ref(false);

const touchedFields = ref({
  role: false,
  isActive: false,
});

const formData = ref<IUpdatePrivilegeCommand & {name: string}>({
  id: "",
  name: "",
  role: "",
  isActive: false,
  claims: null,
});

const errors = reactive<FormErrors>(new FormErrors());

const updatePrivilege = async (data: IUpdatePrivilegeCommand) => {
  try{
    if(isFormValid){
      await userStore.updatePrivilege(data);
      router.push("/UserManager");
    }
  } catch (error) {
    errorHandle(error);
  }
}

const onChangeRole = (value: string)=> {
  touchedFields.value.role = true;
  roleValidate(value);
};

const onChangeIsActive = (value: string) => {
  touchedFields.value.isActive = true;
};

const roleValidate = (value: string): boolean => {
  errors.Clear("Role");
  errors.messages.length = 0;
  if(!value) {
    errors.Add("Role","Rola nie może być pusta.")
  }
  return errors.Get("Role").length === 0;
};

const roleOptions = [
  { label: 'Administrator', value: 'Admin' },
  { label: 'Użytkownik', value: 'User' },
];

const isActiveOptions = [
  { label: 'Tak', value: true },
  { label: 'Nie', value: false },
];

watchEffect(() => {
  isFormValid.value = errors.Count === 0 &&
    (touchedFields.value.role || touchedFields.value.isActive);
});

const fetchUser = async () => {
  try{
    const response = await userStore.getUser(props.value as string);
    formData.value.id = response?.data.id;
    formData.value.name = response?.data.name;
    formData.value.role = response?.data.role;
    formData.value.isActive = response?.data.isActive;
    formData.value.claims = response?.data.claims;
  } catch (error) {
    errorHandle(error);
  }
}

onMounted(fetchUser);

</script>

<!-- ***************************************************  -->
<!-- * Template section                                *  -->
<!-- ***************************************************  -->

<template>
  <div class="updateuser-form">
      <h4>Edycja użytkownika</h4>
      <div class="user-name">{{formData.name}}</div>
      <form @submit.prevent="updatePrivilege(formData)">
        <div class="form-group">
          <ComboBox v-model="formData.role"
            label="Rola w systemie"
            :options="roleOptions"
            :messages="errors.Get('Role')"
          />
        </div>
        <div class="form-group">
          <ComboBox v-model="formData.isActive"
            label="Aktywny"
            :options="isActiveOptions"
            :messages="errors.Get('IsActive')"
          />
        </div>
        <button v-if="true" type="submit">Zapisz</button>
      </form>
  </div>
  </template>

<!-- ***************************************************  -->
<!-- * Style section                                   *  -->
<!-- ***************************************************  -->

<style scoped>
.updateuser-form {
  max-width: 400px;
  margin: 0 auto;
  padding: 20px;
  border: 1px solid #ccc;
  border-radius: 5px;
}

.form-group {
  margin-bottom: 15px;
}

.user-name {
  font-size: 25px;
  color: blue;
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
