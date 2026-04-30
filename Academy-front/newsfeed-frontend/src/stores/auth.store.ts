import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import { authApi } from '../api/auth.api';
import type { RegisterRequest, LoginRequest, UserResponse } from '../types/auth.types';
import { useToast } from '../composables/useToast';
import router from '../router';

export const useAuthStore = defineStore('auth', () => {
  const toast = useToast();
const user = ref<UserResponse | null>(null);
  const loading = ref(false);
  const isAuthenticated = ref(false)
 const authChecked = ref(false);

async function checkAuth() {
if (authChecked.value) return;

    try {
      const me = await authApi.getCurrentUser();
      user.value = me;
      isAuthenticated.value = true;
    } catch {
      user.value = null;
      isAuthenticated.value = false;
    }
    finally {
    authChecked.value = true;
  }
  }

  async function register(data: RegisterRequest) {
    try {
      loading.value = true;
      const response = await authApi.register(data);
      if (response) {
          await checkAuth();

          toast.success('Registration successful!');
          router.push('/');
      }
    } catch (error) {
      console.error('Registration error:', error);
      throw error;
    } finally {
      loading.value = false;
    }
  }

  async function login(data: LoginRequest) {
    try {
      loading.value = true;
      const response = await authApi.login(data);
      console.log('auth ',response);
  
      if (response) {
        authChecked.value = false; 
        await checkAuth();
        toast.success('Login successful!');
        router.push('/');
      } 
    } catch (error) {
      console.error('Login error:', error);
      throw error;
    } finally {
      loading.value = false;
    }
  }

  function logout() {
    user.value = null;
    isAuthenticated.value = false;
    router.push('/login');
    toast.info('Logged out successfully');
  }

  return {
    user,
    loading,
    isAuthenticated,
    authChecked,
    checkAuth,
    register,
    login,
    logout,
  };
});