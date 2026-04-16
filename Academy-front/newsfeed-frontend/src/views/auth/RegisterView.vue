<template>
  <div class="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100 flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8">
    <div class="max-w-md w-full">
      <div class="card card-body">
        <!-- Header -->
        <div class="text-center mb-8">
          <h2 class="text-3xl font-bold text-gray-900 mb-2">Create Account!</h2>
          <p class="text-gray-600">Join our community today</p>
        </div>

        <!-- Register Form -->
        <form @submit.prevent="handleRegister" class="space-y-6">
          <!-- Name -->
          <div>
            <label for="name" class="label">Full Name</label>
            <input
              id="name"
              v-model="form.name"
              type="text"
              required
              class="input-field"
              :class="{ 'input-error': errors.name }"
              placeholder="John Doe"
            />
            <p v-if="errors.name" class="error-message">{{ errors.name }}</p>
          </div>

          <!-- Email -->
          <div>
            <label for="email" class="label">Email Address</label>
            <input
              id="email"
              v-model="form.email"
              type="email"
              required
              class="input-field"
              :class="{ 'input-error': errors.email }"
              placeholder="you@example.com"
            />
            <p v-if="errors.email" class="error-message">{{ errors.email }}</p>
          </div>

          <!-- Password -->
          <div>
            <label for="password" class="label">Password</label>
            <input
              id="password"
              v-model="form.password"
              type="password"
              required
              class="input-field"
              :class="{ 'input-error': errors.password }"
              placeholder="••••••••"
            />
            <p v-if="errors.password" class="error-message">{{ errors.password }}</p>
            <p class="text-xs text-gray-500 mt-1">Must be at least 6 characters</p>
          </div>

          <!-- Avatar URL (Optional) -->
          <div>
            <label for="avatar" class="label">Avatar URL (Optional)</label>
            <input
              id="avatar"
              v-model="form.avatar"
              type="url"
              class="input-field"
              placeholder="https://example.com/avatar.jpg"
            />
          </div>

          <!-- Submit Button -->
          <button
            type="submit"
            :disabled="authStore.loading"
            class="w-full btn-primary"
          >
            <span v-if="!authStore.loading">Create Account</span>
            <span v-else class="flex items-center justify-center">
              <svg class="animate-spin -ml-1 mr-3 h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
              </svg>
              Creating account...
            </span>
          </button>
        </form>

        <!-- Divider -->
        <div class="mt-6">
          <div class="relative">
            <div class="absolute inset-0 flex items-center">
              <div class="w-full border-t border-gray-300"></div>
            </div>
            <div class="relative flex justify-center text-sm">
              <span class="px-2 bg-white text-gray-500">Already have an account?</span>
            </div>
          </div>
        </div>

        <!-- Login Link -->
        <div class="mt-6 text-center">
          <router-link
            to="/login"
            class="text-primary-600 hover:text-primary-700 font-medium"
          >
            Sign in instead
          </router-link>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { reactive, ref } from 'vue';
import { useAuthStore } from '../../stores/auth.store';
import type { RegisterRequest } from '../../types/auth.types';

const authStore = useAuthStore();

const form = reactive<RegisterRequest>({
  name: '',
  email: '',
  password: '',
  avatar: '',
});

const errors = ref<Partial<Record<keyof RegisterRequest, string>>>({});

const validateForm = (): boolean => {
  errors.value = {};

  if (!form.name) {
    errors.value.name = 'Name is required';
  } else if (form.name.length < 2) {
    errors.value.name = 'Name must be at least 2 characters';
  }

  if (!form.email) {
    errors.value.email = 'Email is required';
  } else if (!/\S+@\S+\.\S+/.test(form.email)) {
    errors.value.email = 'Email is invalid';
  }

  if (!form.password) {
    errors.value.password = 'Password is required';
  } else if (form.password.length < 6) {
    errors.value.password = 'Password must be at least 6 characters';
  }

  return Object.keys(errors.value).length === 0;
};

const handleRegister = async () => {
  if (!validateForm()) return;

  try {
    await authStore.register(form);
  } catch (error) {
    console.error('Registration failed:', error);
  }
};
</script>