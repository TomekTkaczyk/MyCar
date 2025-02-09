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

const formData = ref<IUpdatePrivilegeCommand & {name: string, claims: Record<string,string[]>}>({
  id: "",
  name: "",
  role: "",
  isActive: false,
  claims: {},
});
const allClaims = ref<Record<string, string[]>>({});

const errors = new FormErrors();

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
    formData.value.claims = response?.data.claims || {};

    const claims = await userStore.getAllClaims();
    allClaims.value = claims?.data || {};
  } catch (error) {
    errorHandle(error);
  }
}

const categoryTranslations: Record<string, string> = {
  "user": "Moduł: Użytkownik",
  "employee": "Moduł: Pracownik"
};

const translateClaimCategory = (category: string): string => {
  return categoryTranslations[category] || category;
};

const onClaimChange = (category: string, claim: string, event: Event) => {
  const checked = (event.target as HTMLInputElement).checked;

  if (!formData.value.claims[category]) {
    formData.value.claims[category] = [];
  }

  if (checked) {
    if (!formData.value.claims[category].includes(claim)) {
      formData.value.claims[category].push(claim);
    }
  } else {
    formData.value.claims[category] = formData.value.claims[category].filter(c => c !== claim);

    if (formData.value.claims[category].length === 0) {
      delete formData.value.claims[category];
    }
  }
};

onMounted(fetchUser);

</script>

<!-- ***************************************************  -->
<!-- * Template section                                *  -->
<!-- ***************************************************  -->

<template>
  <div class="updateuser-form">
    <a href="javascript:history.back()" class="back-link">&#8592; Powrót</a>
    <br/>
    <br/>
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

      <div class="claims-section">
        <h5>Uprawnienia</h5>
        <div v-for="(claims, category) in allClaims" :key="category" class="claim-group">
          <h6 class="claim-category">{{ translateClaimCategory(category) }}</h6>
          <div v-for="claim in claims" :key="claim" class="claim-item">
            <label>
              <input type="checkbox"
                    :value="claim"
                    :checked="formData.claims[category]?.includes(claim)"
                    @change="onClaimChange(category, claim, $event)" />
              {{ claim }}
            </label>
          </div>
        </div>
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

.claims-section {
  margin-top: 20px;
}

.claim-group {
  margin-bottom: 15px;
}

.claim-category {
  text-align: center;
  font-weight: bold;
  margin-bottom: 10px;
}

.claim-item label {
  display: flex;
  align-items: center;
  gap: 10px;
}

input[type="checkbox"] {
  margin: 0;
}
</style>
