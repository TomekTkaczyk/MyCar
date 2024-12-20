import type { RouteRecordRaw } from 'vue-router';

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    name: 'Home',
    component: () => import('@/modules/home/Home.vue'),
  },
  {
    path: '/MyProfile',
    name: 'MyProfile',
    component: () => import('@/modules/auth/MyProfile.vue'),
    meta: {
      requiresAuth: true,
      role: 'user',
    }
  },
  {
    path: '/Logout',
    name: 'Logout',
    component: () => import('@/modules/auth/Logout.vue'),
    meta: {
      requiresAuth: true,
    }
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
    component: () => import('@/modules/auth/RemindPassword.vue'),
  },
  {
    path: '/Error403',
    name: 'Error403',
    component: () => import('@/modules/errors/Error403.vue'),
  },
  {
    path: '/:pathMatch(.*)*',
    name: 'Error404',
    component: () => import('@/modules/errors/Error404.vue')
  },
];

export default routes;
