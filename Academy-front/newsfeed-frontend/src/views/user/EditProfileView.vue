<template>
  <div class="min-h-screen bg-gray-50 py-8">
    <div class="container-custom max-w-2xl">
      <div class="card card-body">
        <!-- Header -->
        <div class="mb-8">
          <h1 class="text-3xl font-bold text-gray-900 mb-2">Edit Profile</h1>
          <p class="text-gray-600">Update your personal information</p>
        </div>

        <!-- Form -->
        <form @submit.prevent="handleSubmit" class="space-y-6">
          <!-- Avatar Preview -->
          <div class="flex items-center gap-6">
            <div class="relative">
              <img
                v-if="avatarPreview"
                :src="avatarPreview"
                alt="Avatar"
                class="w-24 h-24 rounded-full object-cover border-4 border-primary-100"
              />
              <div
                v-else
                class="w-24 h-24 rounded-full bg-primary-100 flex items-center justify-center border-4 border-primary-200"
              >
                <span class="text-3xl font-bold text-primary-600">
                  {{ form.name.charAt(0).toUpperCase() || 'U' }}
                </span>
              </div>
            </div>

            <div class="flex-1">
              <label class="label">Profile Picture</label>
              <div class="flex gap-2">
                <input
                  type="file"
                  accept="image/*"
                  @change="handleAvatarUpload"
                  class="hidden"
                  ref="fileInput"
                />
                <button
                  type="button"
                  @click="fileInput?.click()"
                  class="btn-secondary"
                  :disabled="uploading"
                >
                  {{ uploading ? 'Uploading...' : 'Upload New' }}
                </button>
                <button
                  v-if="form.avatar"
                  type="button"
                  @click="removeAvatar"
                  class="btn-secondary text-red-600"
                >
                  Remove
                </button>
              </div>
            </div>
          </div>

          <!-- Name -->
          <div>
            <label for="name" class="label">Full Name *</label>
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
            <label for="email" class="label">Email Address *</label>
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

          <!-- Action Buttons -->
          <div class="flex gap-4 pt-4">
            <button
              type="submit"
              :disabled="loading"
              class="btn-primary flex-1"
            >
              <span v-if="!loading">Save Changes</span>
              <span v-else class="flex items-center justify-center">
                <svg class="animate-spin -ml-1 mr-3 h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                  <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                  <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                </svg>
                Saving...
              </span>
            </button>
            <router-link to="/profile" class="btn-secondary flex-1 text-center">
              Cancel
            </router-link>
          </div>
        </form>

        <!-- Danger Zone -->
        <div class="mt-8 pt-8 border-t border-red-200">
          <h3 class="text-lg font-semibold text-red-600 mb-4">Danger Zone</h3>
          <button
            @click="handleDeleteAccount"
            class="btn-danger"
          >
            Delete Account
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { reactive, ref, computed, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '../../stores/auth.store';
import { userApi } from '../../api/user.api';
import { fileApi } from '../../api/file.api';
import type { PutUserCommand } from '../../types/user.types';
import { useToast } from '../../composables/useToast';

const router = useRouter();
const authStore = useAuthStore();
const toast = useToast();

const form = reactive<PutUserCommand>({
  name: '',
  email: '',
  avatar: '',
});

const errors = ref<Partial<Record<keyof PutUserCommand, string>>>({});
const loading = ref(false);
const uploading = ref(false);
const fileInput = ref<HTMLInputElement | null>(null);

const avatarPreview = computed(() => {
  if (!form.avatar) return '';
  return form.avatar.startsWith('http') ? form.avatar : fileApi.getFileUrl(form.avatar);
});

const loadUserData = () => {
  if (authStore.user) {
    form.name = authStore.user.name;
    form.email = authStore.user.email;
    form.avatar = authStore.user.avatar || '';
  }
};

const validateForm = (): boolean => {
  errors.value = {};

  if (!form.name || form.name.trim().length < 2) {
    errors.value.name = 'Name must be at least 2 characters';
  }

  if (!form.email) {
    errors.value.email = 'Email is required';
  } else if (!/\S+@\S+\.\S+/.test(form.email)) {
    errors.value.email = 'Email is invalid';
  }

  return Object.keys(errors.value).length === 0;
};

const handleAvatarUpload = async (event: Event) => {
  const target = event.target as HTMLInputElement;
  const file = target.files?.[0];

  if (!file) return;

  if (!file.type.startsWith('image/')) {
    toast.error('Please select an image file');
    return;
  }

  if (file.size > 5 * 1024 * 1024) {
    toast.error('Image size must be less than 5MB');
    return;
  }

  try {
    uploading.value = true;
    const response = await fileApi.upload(file);
    
    if (response.data) {
      form.avatar = response.data;
      toast.success('Avatar uploaded successfully');
    }
  } catch (error) {
    toast.error('Failed to upload avatar');
    console.error('Upload error:', error);
  } finally {
    uploading.value = false;
  }
};

const removeAvatar = () => {
  form.avatar = '';
};

const handleSubmit = async () => {
  if (!validateForm()) return;

  try {
    loading.value = true;
    const response = await userApi.update(form);
    
    if (response.data) {
      // Обновляем пользователя в store
      if (authStore.user) {
        authStore.user.name = response.data.name;
        authStore.user.email = response.data.email;
        authStore.user.avatar = response.data.avatar || '';
      }
      
      toast.success('Profile updated successfully');
      router.push('/profile');
    }
  } catch (error) {
    console.error('Failed to update profile:', error);
    toast.error('Failed to update profile');
  } finally {
    loading.value = false;
  }
};

const handleDeleteAccount = async () => {
  const confirmed = confirm(
    'Are you sure you want to delete your account? This action cannot be undone.'
  );

  if (!confirmed) return;

  const doubleConfirm = confirm(
    'This will permanently delete all your data. Are you absolutely sure?'
  );

  if (!doubleConfirm) return;

  try {
    await userApi.delete();
    toast.info('Account deleted successfully');
    authStore.logout();
  } catch (error) {
    console.error('Failed to delete account:', error);
    toast.error('Failed to delete account');
  }
};

onMounted(() => {
  loadUserData();
});
</script>