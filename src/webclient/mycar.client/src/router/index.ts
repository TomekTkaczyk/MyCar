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
      if (to.name === 'SignUp') {
        return next({ name: 'SignUp' });
      }
      return next({ name: 'SignIn' });
    }
  }

  const requiredRoles = to.meta.requiredRoles as string[] | undefined;
  const requiredClaims = to.meta.requiredClaims as string[] | undefined;

  const hasRequiredRole = requiredRoles?.some(role => authStore.role === role) ?? true;
  const hasRequiredClaim = requiredClaims?.some(claim => authStore.claims?.includes(claim)) ?? true;

  if (!hasRequiredRole && !hasRequiredClaim) {
    return next({ name: 'Error403' });
  }

  next();
});

export default router;
