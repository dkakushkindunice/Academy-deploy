import { defineStore } from 'pinia';
import { ref } from 'vue';
import { newsApi } from '../api/news.api';
import type { NewsOutDto, CreateNewsCommand, PutNewsCommand } from '../types/news.types';
import { useToast } from '../composables/useToast';

export const useNewsStore = defineStore('news', () => {
  const toast = useToast();
  
  const newsList = ref<NewsOutDto[]>([]);
  const currentNews = ref<NewsOutDto | null>(null);
  const totalNews = ref(0);
  const loading = ref(false);

  async function fetchNews(limit = 10, offset = 0) {
    try {
      loading.value = true;
      const response = await newsApi.getAll({ limit, offset });

      if (response.data) {
        newsList.value = response.data.newsOutDtos;
        totalNews.value = response.data.numberOfElements;
      }
    } catch (error) {
      console.error('Error fetching news:', error);
      toast.error('Failed to load news');
    } finally {
      loading.value = false;
    }
  }

  async function createNews(data: CreateNewsCommand) {
    try {
      loading.value = true;
      const response = await newsApi.create(data);

      if (response.data) {
        newsList.value.unshift(response.data);
        toast.success('News created successfully!');
        return response.data;
      }
    } catch (error) {
      console.error('Error creating news:', error);
      toast.error('Failed to create news');
      throw error;
    } finally {
      loading.value = false;
    }
  }

  async function updateNews(id: number, data: PutNewsCommand) {
    try {
      loading.value = true;
      await newsApi.update(id, data);

      const index = newsList.value.findIndex((news) => news.id === id);
      if (index !== -1) {
        const existing = newsList.value[index];
                if (!existing) return;
        // Явно создаем новый объект со всеми обязательными полями
        const updated: NewsOutDto = {
          id: existing.id,
          userId: existing.userId,
          username: existing.username,
          title: data.title,
          description: data.description,
          image: data.image,
          tags: data.tags,
          createdAt: existing.createdAt,
          updatedAt: existing.updatedAt,
        };
        
        newsList.value[index] = updated;
      }

      toast.success('News updated successfully!');
    } catch (error) {
      console.error('Error updating news:', error);
      toast.error('Failed to update news');
      throw error;
    } finally {
      loading.value = false;
    }
  }

  async function deleteNews(id: number) {
    try {
      loading.value = true;
      await newsApi.delete(id);

      newsList.value = newsList.value.filter((news) => news.id !== id);
      toast.success('News deleted successfully!');
    } catch (error) {
      console.error('Error deleting news:', error);
      toast.error('Failed to delete news');
      throw error;
    } finally {
      loading.value = false;
    }
  }

  return {
    newsList,
    currentNews,
    totalNews,
    loading,
    fetchNews,
    createNews,
    updateNews,
    deleteNews,
  };
});