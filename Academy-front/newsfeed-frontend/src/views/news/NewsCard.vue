<template>
  <article class="card hover:shadow-xl transition-all duration-300 overflow-hidden group">
    <!-- Image -->
    <div class="relative h-48 overflow-hidden bg-gray-200">
      <img
        v-if="news.image"
        :src="getImageUrl(news.image)"
        :alt="news.title"
        class="w-full h-full object-cover group-hover:scale-110 transition-transform duration-300"
      />
      <div
        v-else
        class="w-full h-full flex items-center justify-center bg-gradient-to-br from-primary-400 to-primary-600"
      >
        <svg class="w-20 h-20 text-white opacity-50" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 20H5a2 2 0 01-2-2V6a2 2 0 012-2h10a2 2 0 012 2v1m2 13a2 2 0 01-2-2V7m2 13a2 2 0 002-2V9a2 2 0 00-2-2h-2m-4-3H9M7 16h6M7 8h6v4H7V8z" />
        </svg>
      </div>
      
      <!-- Tags Overlay -->
      <div v-if="news.tags && news.tags.length > 0" class="absolute top-2 left-2 flex flex-wrap gap-1">
        <span
          v-for="(tag, index) in news.tags.slice(0, 3)"
          :key="index"
          class="px-2 py-1 bg-black bg-opacity-60 text-white text-xs rounded-full"
        >
          {{ tag.title }}
        </span>
        <span
          v-if="news.tags.length > 3"
          class="px-2 py-1 bg-black bg-opacity-60 text-white text-xs rounded-full"
        >
          +{{ news.tags.length - 3 }}
        </span>
      </div>
    </div>

    <!-- Content -->
    <div class="card-body">
      <!-- Title -->
      <h3 class="text-xl font-bold text-gray-900 mb-2 line-clamp-2 group-hover:text-primary-600 transition-colors">
        {{ news.title }}
      </h3>

      <!-- Description -->
      <p class="text-gray-600 text-sm mb-4 line-clamp-3">
        {{ news.description }}
      </p>

      <!-- Author & Actions -->
      <div class="flex items-center justify-between pt-4 border-t">
        <div class="flex items-center gap-2">
          <div class="w-8 h-8 rounded-full bg-primary-100 flex items-center justify-center">
            <span class="text-sm font-semibold text-primary-600">
              {{ news.username.charAt(0).toUpperCase() }}
            </span>
          </div>
          <div class="text-sm">
            <p class="font-medium text-gray-900">{{ news.username }}</p>
          </div>
        </div>

        <!-- Action Buttons (если это новость текущего пользователя) -->
        <div v-if="showActions && isOwner" class="flex gap-2">
          <router-link
            :to="`/news/${news.id}/edit`"
            class="p-2 text-gray-600 hover:text-primary-600 hover:bg-primary-50 rounded-lg transition-colors"
            title="Edit"
          >
            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
            </svg>
          </router-link>
          <button
            @click="handleDelete"
            class="p-2 text-gray-600 hover:text-red-600 hover:bg-red-50 rounded-lg transition-colors"
            title="Delete"
          >
            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
            </svg>
          </button>
        </div>
      </div>
    </div>
  </article>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useAuthStore } from '../../stores/auth.store';
import { fileApi } from '../../api/file.api';
import type { NewsOutDto } from '../../types/news.types';

interface Props {
  news: NewsOutDto;
  showActions?: boolean;
}

const props = withDefaults(defineProps<Props>(), {
  showActions: false,
});

const emit = defineEmits<{
  delete: [id: number];
}>();

const authStore = useAuthStore();

const isOwner = computed(() => {
  return authStore.user?.id === props.news.userId;
});

const getImageUrl = (image: string) => {
  if (image.startsWith('http')) {
    return image;
  }
  return fileApi.getFileUrl(image);
};

const handleDelete = () => {
  emit('delete', props.news.id);
};
</script>

<style scoped>
.line-clamp-2 {
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

.line-clamp-3 {
  display: -webkit-box;
  -webkit-line-clamp: 3;
  -webkit-box-orient: vertical;
  overflow: hidden;
}
</style>