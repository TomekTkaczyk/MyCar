import type { RouteRecordRaw } from 'vue-router';

import Home from '@/modules/home/Home.vue'
import About from '@/modules/home/About.vue'

const routes: RouteRecordRaw[] = [
    {
        path: '/',
        name: '',
        component: Home,
    },
    {
        path: '/Error403',
        name: 'Error403',
        component: () => import('@/modules/errors/Error403.vue'),
    },  
    {
        path: '/Home',
        name: 'Home',
        component: () => import('@/modules/home/Home.vue'),
    },    
    {
        path: '/About',
        name: 'About',
        component: () => import('@/modules/home/About.vue'),
    },
    {
        path: '/SignUp',
        name: 'SignUp',
        component: () => import('@/modules/auth/SignUp.vue'),
    },
    {
        path: '/SignIn',
        name: 'SignIn',
        component: () => import('@/modules/auth/SignIn.vue'),
    },
];

export default routes;