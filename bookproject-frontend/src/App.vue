<template>
  <div>
    <template v-if="showLayout">
      <div class="layout">
        <AppSidebar />
        <main class="main-content">
          <RouterView />
        </main>
      </div>
    </template>
    <template v-else>
      <RouterView />
    </template>
    <ToastContainer />
  </div>
</template>

<script setup>
import { computed } from 'vue'
import { useRoute } from 'vue-router'
import AppSidebar from '@/components/common/AppSidebar.vue'
import ToastContainer from '@/components/common/ToastContainer.vue'

const route = useRoute()
const guestRoutes = ['/login', '/register']
const showLayout = computed(() => !guestRoutes.includes(route.path))
</script>
