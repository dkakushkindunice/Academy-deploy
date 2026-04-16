<template>
  <div class="flex items-center justify-center" :class="containerClass">
    <div
      class="animate-spin rounded-full border-b-2"
      :class="spinnerClass"
      :style="{ width: size, height: size }"
    ></div>
    <p v-if="text" class="ml-3 text-gray-600" :class="textClass">{{ text }}</p>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';

interface Props {
  size?: string;
  color?: string;
  text?: string;
  fullscreen?: boolean;
}

const props = withDefaults(defineProps<Props>(), {
  size: '48px',
  color: 'border-primary-600',
  text: '',
  fullscreen: false,
});

const containerClass = computed(() => {
  return props.fullscreen ? 'min-h-screen' : 'py-8';
});

const spinnerClass = computed(() => props.color);

const textClass = computed(() => {
  const sizeMap: Record<string, string> = {
    '24px': 'text-sm',
    '32px': 'text-base',
    '48px': 'text-lg',
    '64px': 'text-xl',
  };
  return sizeMap[props.size] || 'text-base';
});
</script>