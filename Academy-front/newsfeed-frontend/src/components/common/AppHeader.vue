<template>
  <header class="bg-white shadow-sm sticky top-0 z-50">
    <nav class="container-custom">
      <div class="flex items-center justify-between h-16">
        <!-- Logo -->
        <router-link to="/" class="flex items-center gap-2 group">
          <div class="w-10 h-10 bg-gradient-to-br from-primary-500 to-primary-700 rounded-lg flex items-center justify-center group-hover:scale-110 transition-transform">
            <svg class="w-6 h-6 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 20H5a2 2 0 01-2-2V6a2 2 0 012-2h10a2 2 0 012 2v1m2 13a2 2 0 01-2-2V7m2 13a2 2 0 002-2V9a2 2 0 00-2-2h-2m-4-3H9M7 16h6M7 8h6v4H7V8z" />
            </svg>
          </div>
          <span class="text-xl font-bold text-gray-900 group-hover:text-primary-600 transition-colors">
            NewsFeed
          </span>
        </router-link>

        <!-- Desktop Navigation -->
        <div class="hidden md:flex items-center gap-6">
          <router-link
            to="/"
            class="text-gray-600 hover:text-primary-600 font-medium transition-colors"
            active-class="text-primary-600"
          >
            Home
          </router-link>
          <router-link
            to="/news"
            class="text-gray-600 hover:text-primary-600 font-medium transition-colors"
            active-class="text-primary-600"
          >
            News
          </router-link>

          <!-- Authenticated User Menu -->
          <template v-if="authStore.isAuthenticated">
            <router-link
              to="/news/create"
              class="text-gray-600 hover:text-primary-600 font-medium transition-colors"
              active-class="text-primary-600"
            >
              Create
            </router-link>

            <!-- User Dropdown -->
            <div class="relative" ref="dropdownRef">
              <button
                @click="toggleDropdown"
                class="flex items-center gap-2 text-gray-600 hover:text-primary-600 font-medium transition-colors"
              >
                <div class="w-8 h-8 rounded-full bg-primary-100 flex items-center justify-center">
                  <span class="text-sm font-semibold text-primary-600">
                    {{ authStore.user?.name.charAt(0).toUpperCase() }}
                  </span>
                </div>
                <svg class="w-4 h-4" :class="{ 'rotate-180': isDropdownOpen }" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
                </svg>
              </button>

              <!-- Dropdown Menu -->
              <transition
                enter-active-class="transition ease-out duration-100"
                enter-from-class="transform opacity-0 scale-95"
                enter-to-class="transform opacity-100 scale-100"
                leave-active-class="transition ease-in duration-75"
                leave-from-class="transform opacity-100 scale-100"
                leave-to-class="transform opacity-0 scale-95"
              >
                <div
                  v-if="isDropdownOpen"
                  class="absolute right-0 mt-2 w-48 bg-white rounded-lg shadow-lg py-2 border"
                >
                  <router-link
                    to="/profile"
                    @click="closeDropdown"
                    class="block px-4 py-2 text-gray-700 hover:bg-gray-50 transition-colors"
                  >
                    <svg class="w-4 h-4 inline mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
                    </svg>
                    My Profile
                  </router-link>
                  <router-link
                    to="/profile/edit"
                    @click="closeDropdown"
                    class="block px-4 py-2 text-gray-700 hover:bg-gray-50 transition-colors"
                  >
                    <svg class="w-4 h-4 inline mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10.325 4.317c.426-1.756 2.924-1.756 3.35 0a1.724 1.724 0 002.573 1.066c1.543-.94 3.31.826 2.37 2.37a1.724 1.724 0 001.065 2.572c1.756.426 1.756 2.924 0 3.35a1.724 1.724 0 00-1.066 2.573c.94 1.543-.826 3.31-2.37 2.37a1.724 1.724 0 00-2.572 1.065c-.426 1.756-2.924 1.756-3.35 0a1.724 1.724 0 00-2.573-1.066c-1.543.94-3.31-.826-2.37-2.37a1.724 1.724 0 00-1.065-2.572c-1.756-.426-1.756-2.924 0-3.35a1.724 1.724 0 001.066-2.573c-.94-1.543.826-3.31 2.37-2.37.996.608 2.296.07 2.572-1.065z" />
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                    </svg>
                    Settings
                  </router-link>
                  <hr class="my-2">
                  <button
                    @click="handleLogout"
                    class="block w-full text-left px-4 py-2 text-red-600 hover:bg-red-50 transition-colors"
                  >
                    <svg class="w-4 h-4 inline mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1" />
                    </svg>
                    Logout
                  </button>
                </div>
              </transition>
            </div>
          </template>

          <!-- Guest Links -->
          <template v-else>
            <router-link to="/login" class="text-gray-600 hover:text-primary-600 font-medium transition-colors">
              Sign In
            </router-link>
            <router-link to="/register" class="btn-primary">
              Get Started
            </router-link>
          </template>
        </div>

        <!-- Mobile Menu Button -->
        <button
          @click="toggleMobileMenu"
          class="md:hidden p-2 text-gray-600 hover:text-primary-600"
        >
          <svg v-if="!isMobileMenuOpen" class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
          </svg>
          <svg v-else class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
          </svg>
        </button>
      </div>

      <!-- Mobile Menu -->
      <transition
        enter-active-class="transition ease-out duration-200"
        enter-from-class="transform opacity-0 -translate-y-2"
        enter-to-class="transform opacity-100 translate-y-0"
        leave-active-class="transition ease-in duration-150"
        leave-from-class="transform opacity-100 translate-y-0"
        leave-to-class="transform opacity-0 -translate-y-2"
      >
        <div v-if="isMobileMenuOpen" class="md:hidden py-4 border-t">
          <div class="flex flex-col gap-2">
            <router-link
              to="/"
              @click="closeMobileMenu"
              class="px-4 py-2 text-gray-600 hover:bg-gray-50 rounded-lg transition-colors"
            >
              Home
            </router-link>
            <router-link
              to="/news"
              @click="closeMobileMenu"
              class="px-4 py-2 text-gray-600 hover:bg-gray-50 rounded-lg transition-colors"
            >
              News
            </router-link>

            <template v-if="authStore.isAuthenticated">
              <router-link
                to="/news/create"
                @click="closeMobileMenu"
                class="px-4 py-2 text-gray-600 hover:bg-gray-50 rounded-lg transition-colors"
              >
                Create News
              </router-link>
              <router-link
                to="/profile"
                @click="closeMobileMenu"
                class="px-4 py-2 text-gray-600 hover:bg-gray-50 rounded-lg transition-colors"
              >
                My Profile
              </router-link>
              <router-link
                to="/profile/edit"
                @click="closeMobileMenu"
                class="px-4 py-2 text-gray-600 hover:bg-gray-50 rounded-lg transition-colors"
              >
                Settings
              </router-link>
              <hr class="my-2">
              <button
                @click="handleLogout"
                class="px-4 py-2 text-left text-red-600 hover:bg-red-50 rounded-lg transition-colors"
              >
                Logout
              </button>
            </template>

            <template v-else>
              <router-link
                to="/login"
                @click="closeMobileMenu"
                class="px-4 py-2 text-gray-600 hover:bg-gray-50 rounded-lg transition-colors"
              >
                Sign In
              </router-link>
              <router-link
                to="/register"
                @click="closeMobileMenu"
                class="px-4 py-2 bg-primary-600 text-white rounded-lg hover:bg-primary-700 transition-colors text-center"
              >
                Get Started
              </router-link>
            </template>
          </div>
        </div>
      </transition>
    </nav>
  </header>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue';
import { useAuthStore } from '../../stores/auth.store';

const authStore = useAuthStore();
const isDropdownOpen = ref(false);
const isMobileMenuOpen = ref(false);
const dropdownRef = ref<HTMLElement>();

const toggleDropdown = () => {
  isDropdownOpen.value = !isDropdownOpen.value;
};

const closeDropdown = () => {
  isDropdownOpen.value = false;
};

const toggleMobileMenu = () => {
  isMobileMenuOpen.value = !isMobileMenuOpen.value;
};

const closeMobileMenu = () => {
  isMobileMenuOpen.value = false;
};

const handleLogout = () => {
  authStore.logout();
  closeDropdown();
  closeMobileMenu();
};

// Close dropdown when clicking outside
const handleClickOutside = (event: MouseEvent) => {
  if (dropdownRef.value && !dropdownRef.value.contains(event.target as Node)) {
    closeDropdown();
  }
};

onMounted(() => {
  document.addEventListener('click', handleClickOutside);
});

onUnmounted(() => {
  document.removeEventListener('click', handleClickOutside);
});
</script>