import { createRouter, createWebHistory } from 'vue-router';
import type { RouteRecordRaw } from 'vue-router';
import { useAuthStore } from '../stores/auth.store';

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    name: 'Home',
    component: () => import('../views/HomeView.vue'),
    meta: { requiresAuth: false },
  },
  {
    path: '/login',
    name: 'Login',
    component: () => import('../views/auth/LoginView.vue'),
    meta: { requiresAuth: false, guestOnly: true },
  },
  {
    path: '/register',
    name: 'Register',
    component: () => import('../views/auth/RegisterView.vue'),
    meta: { requiresAuth: false, guestOnly: true },
  },
  {
    path: '/news',
    name: 'NewsList',
    component: () => import('../views/news/NewsListView.vue'),
    meta: { requiresAuth: false },
  },
  {
    path: '/news/create',
    name: 'CreateNews',
    component: () => import('../views/news/CreateNewsView.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/news/:id/edit',
    name: 'EditNews',
    component: () => import('../views/news/EditNewsView.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/profile',
    name: 'Profile',
    component: () => import('../views/user/ProfileView.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/profile/edit',
    name: 'EditProfile',
    component: () => import('../views/user/EditProfileView.vue'),
    meta: { requiresAuth: true },
  },
];

const router = createRouter({
  history: createWebHistory(), 
  routes,
});

// Navigation Guard
router.beforeEach((to, from, next) => {
  const token = localStorage.getItem('token');

  const requiresAuth = to.matched.some(r => r.meta.requiresAuth);
  const guestOnly = to.matched.some(r => r.meta.guestOnly);

  const isAuth = !!token;

  if (requiresAuth && !isAuth) {
    next('/login');
  } else if (guestOnly && isAuth) {
    next('/');
  } else {
    next();
  }
});

export default router;