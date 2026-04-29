<template>
  <div class="min-h-screen bg-gray-50 py-8">
    <div class="container-custom">
      <!-- Header -->
      <div class="flex justify-between items-center mb-8">
        <div>
          <h1 class="text-4xl font-bold text-gray-900 mb-2">News Feed</h1>
          <p class="text-gray-600">Discover the latest stories</p>
        </div>
        <router-link
          v-if="authStore.isAuthenticated"
          to="/news/create"
          class="btn-primary"
        >
          <svg class="w-5 h-5 mr-2 inline" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
          </svg>
          Create News
        </router-link>
      </div>

      <!-- Loading State -->
      <div v-if="newsStore.loading" class="flex justify-center items-center py-20">
        <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-primary-600"></div>
      </div>

      <!-- News Grid -->
      <div v-else-if="newsStore.newsList.length > 0" class="grid md:grid-cols-2 lg:grid-cols-3 gap-6">
        <NewsCard
          v-for="news in newsStore.newsList"
          :key="news.id"
          :news="news"
          @delete="handleDelete"
        />
      </div>

      <!-- Empty State -->
      <div v-else class="text-center py-20">
        <svg class="mx-auto h-24 w-24 text-gray-400 mb-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 20H5a2 2 0 01-2-2V6a2 2 0 012-2h10a2 2 0 012 2v1m2 13a2 2 0 01-2-2V7m2 13a2 2 0 002-2V9a2 2 0 00-2-2h-2m-4-3H9M7 16h6M7 8h6v4H7V8z" />
        </svg>
        <h3 class="text-xl font-semibold text-gray-900 mb-2">No news yet</h3>
        <p class="text-gray-600 mb-6">Be the first to share a story!</p>
        <router-link
          v-if="authStore.isAuthenticated"
          to="/news/create"
          class="btn-primary"
        >
          Create First News
        </router-link>
      </div>

      <!-- Pagination -->
      <div v-if="newsStore.totalNews > limit" class="mt-8 flex justify-center">
        <div class="flex gap-2">
          <button
            :disabled="currentPage === 1"
            @click="changePage(currentPage - 1)"
            class="px-4 py-2 border rounded-lg disabled:opacity-50 disabled:cursor-not-allowed hover:bg-gray-50"
          >
            Previous
          </button>
          <span class="px-4 py-2">
            Page {{ currentPage }} of {{ totalPages }}
          </span>
          <button
            :disabled="currentPage === totalPages"
            @click="changePage(currentPage + 1)"
            class="px-4 py-2 border rounded-lg disabled:opacity-50 disabled:cursor-not-allowed hover:bg-gray-50"
          >
            Next
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useNewsStore } from '../../stores/news.store';
import { useAuthStore } from '../../stores/auth.store';
import NewsCard from './NewsCard.vue';

const newsStore = useNewsStore();
const authStore = useAuthStore();

const currentPage = ref(1);
const limit = 9;

const totalPages = computed(() => Math.ceil(newsStore.totalNews / limit));
console.log('AUTH:', authStore.isAuthenticated);
const loadNews = async () => {
  const offset = (currentPage.value - 1) * limit;
  await newsStore.fetchNews(limit, offset);
};

const changePage = (page: number) => {
  currentPage.value = page;
  loadNews();
  window.scrollTo({ top: 0, behavior: 'smooth' });
};

const handleDelete = async (id: number) => {
  if (confirm('Are you sure you want to delete this news?')) {
    await newsStore.deleteNews(id);
  }
};

onMounted(() => {
  authStore.checkAuth();
  loadNews();
});
</script>