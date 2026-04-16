<template>
  <div class="min-h-screen bg-gray-50 py-8">
    <div class="container-custom max-w-4xl">
      <!-- User Info Card -->
      <div class="card card-body mb-8">
        <div class="flex items-start justify-between mb-6">
          <div class="flex items-center gap-6">
            <!-- Avatar -->
            <div class="relative">
              <img
                v-if="user?.avatar"
                :src="user.avatar"
                :alt="user.name"
                class="w-24 h-24 rounded-full object-cover border-4 border-primary-100"
              />
              <div
                v-else
                class="w-24 h-24 rounded-full bg-primary-100 flex items-center justify-center border-4 border-primary-200"
              >
                <span class="text-3xl font-bold text-primary-600">
                  {{ user?.name.charAt(0).toUpperCase() }}
                </span>
              </div>
            </div>

            <!-- User Details -->
            <div>
              <h1 class="text-3xl font-bold text-gray-900 mb-1">{{ user?.name }}</h1>
              <p class="text-gray-600 mb-2">{{ user?.email }}</p>
              <span class="inline-block px-3 py-1 bg-green-100 text-green-700 rounded-full text-sm font-medium">
                Active Member
              </span>
            </div>
          </div>

          <!-- Edit Button -->
          <router-link to="/profile/edit" class="btn-primary">
            <svg class="w-5 h-5 mr-2 inline" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
            </svg>
            Edit Profile
          </router-link>
        </div>

        <!-- Stats -->
        <div class="grid grid-cols-3 gap-4 pt-6 border-t">
          <div class="text-center">
            <div class="text-2xl font-bold text-gray-900">{{ userNewsCount }}</div>
            <div class="text-sm text-gray-600">News Posted</div>
          </div>
          <div class="text-center">
            <div class="text-2xl font-bold text-gray-900">0</div>
            <div class="text-sm text-gray-600">Followers</div>
          </div>
          <div class="text-center">
            <div class="text-2xl font-bold text-gray-900">0</div>
            <div class="text-sm text-gray-600">Following</div>
          </div>
        </div>
      </div>

      <!-- User's News -->
      <div class="mb-8">
        <h2 class="text-2xl font-bold text-gray-900 mb-6">My News</h2>

        <!-- Loading State -->
        <div v-if="loading" class="flex justify-center items-center py-20">
          <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-primary-600"></div>
        </div>

        <!-- News Grid -->
        <div v-else-if="userNews.length > 0" class="grid md:grid-cols-2 gap-6">
          <NewsCard
            v-for="news in userNews"
            :key="news.id"
            :news="news"
            :show-actions="true"
            @delete="handleDelete"
          />
        </div>

        <!-- Empty State -->
        <div v-else class="text-center py-20 card card-body">
          <svg class="mx-auto h-24 w-24 text-gray-400 mb-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 20H5a2 2 0 01-2-2V6a2 2 0 012-2h10a2 2 0 012 2v1m2 13a2 2 0 01-2-2V7m2 13a2 2 0 002-2V9a2 2 0 00-2-2h-2m-4-3H9M7 16h6M7 8h6v4H7V8z" />
          </svg>
          <h3 class="text-xl font-semibold text-gray-900 mb-2">No news yet</h3>
          <p class="text-gray-600 mb-6">Start sharing your stories with the community!</p>
          <router-link to="/news/create" class="btn-primary inline-block">
            Create Your First News
          </router-link>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useAuthStore } from '../../stores/auth.store';
import { useNewsStore } from '../../stores/news.store';
import { newsApi } from '../../api/news.api';
import NewsCard from '../news/NewsCard.vue';
import type { NewsOutDto } from '../../types/news.types';

const authStore = useAuthStore();
const newsStore = useNewsStore();

const user = computed(() => authStore.user);
const loading = ref(true);
const userNews = ref<NewsOutDto[]>([]);

const userNewsCount = computed(() => userNews.value.length);

const loadUserNews = async () => {
  if (!user.value) return;

  try {
    loading.value = true;
    const response = await newsApi.getByUser(user.value.id, { limit: 100, offset: 0 });
    
    // API возвращает данные в разном формате, нужно проверить
    if (response.data) {
      userNews.value = Array.isArray(response.data) ? response.data : [];
    }
  } catch (error) {
    console.error('Failed to load user news:', error);
  } finally {
    loading.value = false;
  }
};

const handleDelete = async (id: number) => {
  if (confirm('Are you sure you want to delete this news?')) {
    await newsStore.deleteNews(id);
    userNews.value = userNews.value.filter(n => n.id !== id);
  }
};

onMounted(() => {
  loadUserNews();
});
</script>