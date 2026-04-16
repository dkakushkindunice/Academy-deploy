import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import { authApi } from '../api/auth.api';
import type { RegisterRequest, LoginRequest, UserResponse } from '../types/auth.types';
import { useToast } from '../composables/useToast';
import router from '../router';

const TOKEN_KEY = 'auth_token';
const USER_KEY = 'auth_user';

export const useAuthStore = defineStore('auth', () => {
  const toast = useToast();
  
  const token = ref<string | null>(localStorage.getItem(TOKEN_KEY));
  const user = ref<UserResponse | null>(
    JSON.parse(localStorage.getItem(USER_KEY) || 'null')
  );
  const loading = ref(false);

  const isAuthenticated = computed(() => !!token.value);

  async function register(data: RegisterRequest) {
    try {
      loading.value = true;
      const response = await authApi.register(data);
      if (response) {
        //setAuth(response.data?);

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

      if (response.data) {
          console.log("INSIDE SUCCESS BLOCK");
        setAuth(response.data);
  alert("INSIDE SUCCESS");

        alert('Login successful!');
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

  function setAuth(userData: UserResponse) {
    token.value = userData.token;
    user.value = userData;
    localStorage.setItem(TOKEN_KEY, userData.token);
    localStorage.setItem(USER_KEY, JSON.stringify(userData));
  }

  function logout() {
    token.value = null;
    user.value = null;
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
    router.push('/login');
    toast.info('Logged out successfully');
  }

  return {
    token,
    user,
    loading,
    isAuthenticated,
    register,
    login,
    logout,
  };
});