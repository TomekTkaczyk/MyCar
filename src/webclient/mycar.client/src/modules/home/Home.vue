<template>
    <div class="text-center">
        <h2 class="text-info">MyCar</h2>
        <p>v.0.00.001 (alfa)</p>
        <br />
        <HintList :messages="messages"/>
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';

import HintList from '@/components/HintList.vue';

const messages = ref<string[]>([]);

</script>
