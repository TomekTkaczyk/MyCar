

import '@/assets/main.css';

import { createApp } from 'vue';
import App from './MyCar.vue';
import { createPinia } from 'pinia';

import router from "@/router";

const pinia = createPinia();

const mycar = createApp(App);

mycar
    .use(pinia)
    .use(router)
    .mount('#mycar');
