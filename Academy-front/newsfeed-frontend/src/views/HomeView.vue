<template>
  <div class="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100">
    <!-- Hero Section -->
    <section class="container-custom py-20">
      <div class="text-center">
        <h1 class="text-5xl font-bold text-gray-900 mb-6">
          Welcome to <span class="text-primary-600">NewsFeed</span>
        </h1>
        <p class="text-xl text-gray-600 mb-8 max-w-2xl mx-auto">
          Discover, share, and stay updated with the latest news from around the world.
          Join our community today!
        </p>
        
        <div class="flex gap-4 justify-center">
          <router-link
            v-if="!authStore.isAuthenticated"
            to="/register"
            class="btn-primary px-8 py-3 text-lg"
          >
            Get Started
          </router-link>
          <router-link
            to="/news"
            class="btn-outline px-8 py-3 text-lg"
          >
            Browse News
          </router-link>
        </div>
      </div>
    </section>

    <!-- Features Section -->
    <section class="container-custom py-16">
      <div class="grid md:grid-cols-3 gap-8">
        <div class="card card-body text-center">
          <div class="w-16 h-16 bg-primary-100 rounded-full flex items-center justify-center mx-auto mb-4">
            <svg class="w-8 h-8 text-primary-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 20H5a2 2 0 01-2-2V6a2 2 0 012-2h10a2 2 0 012 2v1m2 13a2 2 0 01-2-2V7m2 13a2 2 0 002-2V9a2 2 0 00-2-2h-2m-4-3H9M7 16h6M7 8h6v4H7V8z" />
            </svg>
          </div>
          <h3 class="text-xl font-semibold mb-2">Latest News</h3>
          <p class="text-gray-600">Stay updated with real-time news from trusted sources</p>
        </div>

        <div class="card card-body text-center">
          <div class="w-16 h-16 bg-primary-100 rounded-full flex items-center justify-center mx-auto mb-4">
            <svg class="w-8 h-8 text-primary-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
            </svg>
          </div>
          <h3 class="text-xl font-semibold mb-2">Create & Share</h3>
          <p class="text-gray-600">Share your stories and insights with the community</p>
        </div>

        <div class="card card-body text-center">
          <div class="w-16 h-16 bg-primary-100 rounded-full flex items-center justify-center mx-auto mb-4">
            <svg class="w-8 h-8 text-primary-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 7h.01M7 3h5c.512 0 1.024.195 1.414.586l7 7a2 2 0 010 2.828l-7 7a2 2 0 01-2.828 0l-7-7A1.994 1.994 0 013 12V7a4 4 0 014-4z" />
            </svg>
          </div>
          <h3 class="text-xl font-semibold mb-2">Organize with Tags</h3>
          <p class="text-gray-600">Filter and find news by topics that matter to you</p>
        </div>
      </div>
    </section>

    <!-- CTA Section -->
    <section class="bg-primary-600 text-white py-16">
      <div class="container-custom text-center">
        <h2 class="text-3xl font-bold mb-4">Ready to get started?</h2>
        <p class="text-xl mb-8 opacity-90">Join thousands of readers and writers today</p>
        <router-link
          v-if="!authStore.isAuthenticated"
          to="/register"
          class="inline-block px-8 py-3 bg-white text-primary-600 rounded-lg font-medium hover:bg-gray-100 transition-colors"
        >
          Sign Up Now
        </router-link>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import { useAuthStore } from '../stores/auth.store';

const authStore = useAuthStore();
</script>