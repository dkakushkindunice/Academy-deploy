<template>
  <div class="min-h-screen bg-gray-50 py-8">
    <div class="container-custom max-w-3xl">
      <div class="card card-body">
        <!-- Header -->
        <div class="mb-8">
          <h1 class="text-3xl font-bold text-gray-900 mb-2">Create News</h1>
          <p class="text-gray-600">Share your story with the community</p>
        </div>

        <!-- Form -->
        <form @submit.prevent="handleSubmit" class="space-y-6">
          <!-- Title -->
          <div>
            <label for="title" class="label">Title *</label>
            <input
              id="title"
              v-model="form.title"
              type="text"
              required
              class="input-field"
              :class="{ 'input-error': errors.title }"
              placeholder="Enter news title"
            />
            <p v-if="errors.title" class="error-message">{{ errors.title }}</p>
          </div>

          <!-- Description -->
          <div>
            <label for="description" class="label">Description *</label>
            <textarea
              id="description"
              v-model="form.description"
              required
              rows="6"
              class="input-field resize-none"
              :class="{ 'input-error': errors.description }"
              placeholder="Write your news content here..."
            ></textarea>
            <p v-if="errors.description" class="error-message">{{ errors.description }}</p>
          </div>

          <!-- Image Upload -->
          <div>
            <label class="label">Cover Image</label>
            <div class="flex items-center gap-4">
              <input
                type="file"
                accept="image/*"
                @change="handleImageUpload"
                class="hidden"
                ref="fileInput"
              />
              <button
                type="button"
                @click="$refs.fileInput.click()"
                class="btn-secondary"
                :disabled="uploading"
              >
                <svg class="w-5 h-5 mr-2 inline" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z" />
                </svg>
                {{ uploading ? 'Uploading...' : 'Choose Image' }}
              </button>
              <span v-if="form.image" class="text-sm text-gray-600 truncate max-w-xs">
                {{ form.image }}
              </span>
            </div>
            
            <!-- Image Preview -->
            <div v-if="imagePreview" class="mt-4">
              <img
                :src="imagePreview"
                alt="Preview"
                class="max-w-md rounded-lg shadow-md"
              />
            </div>
          </div>

          <!-- Tags -->
          <div>
            <label class="label">Tags</label>
            <div class="flex gap-2 mb-2">
              <input
                v-model="tagInput"
                type="text"
                class="input-field"
                placeholder="Add a tag..."
                @keydown.enter.prevent="addTag"
              />
              <button
                type="button"
                @click="addTag"
                class="btn-secondary"
              >
                Add
              </button>
            </div>
            
            <!-- Tags Display -->
            <div v-if="form.tags.length > 0" class="flex flex-wrap gap-2">
              <span
                v-for="(tag, index) in form.tags"
                :key="index"
                class="inline-flex items-center gap-1 px-3 py-1 bg-primary-100 text-primary-700 rounded-full text-sm"
              >
                {{ tag.title }}
                <button
                  type="button"
                  @click="removeTag(index)"
                  class="hover:text-primary-900"
                >
                  <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
                  </svg>
                </button>
              </span>
            </div>
          </div>

          <!-- Action Buttons -->
          <div class="flex gap-4 pt-4">
            <button
              type="submit"
              :disabled="newsStore.loading"
              class="btn-primary flex-1"
            >
              <span v-if="!newsStore.loading">Publish News</span>
              <span v-else class="flex items-center justify-center">
                <svg class="animate-spin -ml-1 mr-3 h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                  <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                  <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                </svg>
                Publishing...
              </span>
            </button>
            <router-link to="/news" class="btn-secondary flex-1 text-center">
              Cancel
            </router-link>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { reactive, ref } from 'vue';
import { useRouter } from 'vue-router';
import { useNewsStore } from '../../stores/news.store';
import { fileApi } from '../../api/file.api';
import type { CreateNewsCommand, TagDto } from '../../types/news.types';
import { useToast } from '../../composables/useToast';

const router = useRouter();
const newsStore = useNewsStore();
const toast = useToast();

const form = reactive<CreateNewsCommand>({
  title: '',
  description: '',
  tags: [],
  image: '',
});

const errors = ref<Partial<Record<keyof CreateNewsCommand, string>>>({});
const tagInput = ref('');
const uploading = ref(false);
const imagePreview = ref('');
const fileInput = ref<HTMLInputElement>();

const validateForm = (): boolean => {
  errors.value = {};

  if (!form.title || form.title.trim().length < 3) {
    errors.value.title = 'Title must be at least 3 characters';
  }

  if (!form.description || form.description.trim().length < 10) {
    errors.value.description = 'Description must be at least 10 characters';
  }

  return Object.keys(errors.value).length === 0;
};

const handleImageUpload = async (event: Event) => {
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
      form.image = response.data;
      imagePreview.value = fileApi.getFileUrl(response.data);
      toast.success('Image uploaded successfully');
    }
  } catch (error) {
    toast.error('Failed to upload image');
    console.error('Upload error:', error);
  } finally {
    uploading.value = false;
  }
};

const addTag = () => {
  const tag = tagInput.value.trim();
  
  if (!tag) return;
  
  if (form.tags.some(t => t.title.toLowerCase() === tag.toLowerCase())) {
    toast.warning('Tag already exists');
    return;
  }

  form.tags.push({ title: tag });
  tagInput.value = '';
};

const removeTag = (index: number) => {
  form.tags.splice(index, 1);
};

const handleSubmit = async () => {
  if (!validateForm()) return;

  try {
    const createdNews = await newsStore.createNews(form);
    if (createdNews) {
      router.push('/news');
    }
  } catch (error) {
    console.error('Failed to create news:', error);
  }
};
</script>