import type { RouteRecordRaw } from 'vue-router';

import Home from '@/modules/home/Home.vue'

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
        path: '/SignUp',
        name: 'SignUp',
        component: () => import('@/modules/auth/SignUp.vue'),
    },
    {
        path: '/SignIn',
        name: 'SignIn',
        component: () => import('@/modules/auth/SignIn.vue'),
    },
    {
      path: '/RemindPassword',
      name: 'ReminPassword',
      component: () => import('@/modules/auth/RemindPassword.vue')
    },
    {
      path: '/ChangePassword',
      name: 'ChangePassword',
      component: () => import('@/modules/auth/ChangePassword.vue')
    },];

export default routes;
