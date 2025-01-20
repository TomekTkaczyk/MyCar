<!-- ***************************************************  -->
<!-- * Script section                                  *  -->
<!-- ***************************************************  -->

<script setup lang='ts'>
import type userDto from './responses/userDto';

defineProps<{
  user: userDto
}>();

</script>

<!-- ***************************************************  -->
<!-- * Template section                                *  -->
<!-- ***************************************************  -->

<template>
  <div class="user-item">
    <h3>{{ user.name }}</h3>
    <p>Email: {{ user.email }}</p>
    <p>Rola: {{ user.role }}</p>
  </div>
</template>

<!-- ***************************************************  -->
<!-- * Style section                                   *  -->
<!-- ***************************************************  -->

<style scoped>
.user-item {
  border: 1px solid #ddd;
  padding: 10px;
  margin-bottom: 10px;
  border-radius: 5px;
  cursor: pointer;
  transition: background-color 0.2s, box-shadow  0.2s;
}

.user-item:hover {
  background-color: #f9f9f9;
  box-shadow: 0 4px 8px rgba(0,0,0,0.1);
}
</style>
