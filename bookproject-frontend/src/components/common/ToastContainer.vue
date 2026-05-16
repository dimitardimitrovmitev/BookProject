<template>
  <div class="toast-container">
    <TransitionGroup name="toast">
      <div
        v-for="toast in toasts"
        :key="toast.id"
        :class="['toast', `toast-${toast.type}`]"
        @click="remove(toast.id)"
      >
        {{ toast.message }}
      </div>
    </TransitionGroup>
  </div>
</template>

<script setup>
import { useToast } from '@/composables/useToast'
const { toasts, remove } = useToast()
</script>

<style scoped>
.toast-enter-from { transform: translateX(100%); opacity: 0; }
.toast-enter-active { transition: all 0.25s ease; }
.toast-leave-to { transform: translateX(100%); opacity: 0; }
.toast-leave-active { transition: all 0.2s ease; }
.toast { cursor: pointer; }
</style>
