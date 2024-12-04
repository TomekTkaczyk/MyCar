import { createRouter, createWebHistory } from 'vue-router'
import routes from '@/router/routes'
import { useAuthStore } from '@/stores/AuthStore';

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes,
});

router.beforeEach((to, from, next) => {
    const authStore = useAuthStore();
    if (to.matched.some(record => record.meta.requiresAuth)) {
        if (!authStore.isAuthenticated) {
            if (to.name === 'SignOut') {
                next({ name: 'SignOut' });
            }
            next({ name: 'SignIn' });
        } else {
            next();
        }
    } else {
        next();
    }
});

export default router;
