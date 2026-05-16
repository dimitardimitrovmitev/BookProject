<template>
  <div>
    <div class="page-header">
      <div class="page-header-left">
        <h1>Users</h1>
        <p>Browse reader profiles</p>
      </div>
    </div>

    <div class="search-bar" style="max-width:400px;margin-bottom:24px">
      <svg class="search-icon" width="16" height="16" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><circle cx="11" cy="11" r="8"/><path d="m21 21-4.35-4.35"/></svg>
      <input v-model="search" class="form-input" placeholder="Search by username…" />
    </div>

    <div v-if="loading" class="users-grid">
      <div v-for="i in 12" :key="i" class="skeleton" style="height:90px;border-radius:12px" />
    </div>

    <div v-else-if="filtered.length" class="users-grid">
      <RouterLink
        v-for="u in filtered"
        :key="u.userId"
        :to="`/users/${u.userId}`"
        class="user-card card card-hover"
      >
        <div class="user-card-avatar">{{ u.username.charAt(0).toUpperCase() }}</div>
        <div class="user-card-info">
          <span class="user-card-name">{{ u.username }}</span>
          <span class="user-card-id text-muted">#{{ u.userId }}</span>
        </div>
        <svg class="user-card-arrow" width="16" height="16" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><path d="m9 18 6-6-6-6"/></svg>
      </RouterLink>
    </div>

    <div v-else class="empty-state">
      <div class="empty-state-icon">👥</div>
      <h3>No users found</h3>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { usersApi } from '@/api/users'
import { useAuthStore } from '@/stores/auth'

const authStore = useAuthStore()
const users = ref([])
const loading = ref(false)
const search = ref('')

const filtered = computed(() => {
  const q = search.value.toLowerCase().trim()
  return q ? users.value.filter(u => u.username.toLowerCase().includes(q)) : users.value
})

async function fetchUsers() {
  loading.value = true
  try {
    const { data } = await usersApi.getAll()
    // If admin returns full list; otherwise we only have public data
    users.value = Array.isArray(data) ? data : (data.items || [])
  } catch {}
  finally { loading.value = false }
}

onMounted(fetchUsers)
</script>

<style scoped>
.users-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(220px, 1fr)); gap: 12px; }
.user-card { display: flex; align-items: center; gap: 14px; text-decoration: none; color: inherit; padding: 14px 16px; }
.user-card-avatar {
  width: 40px; height: 40px; border-radius: 50%; flex-shrink: 0;
  background: var(--amber-dim); color: var(--amber-2);
  font-family: var(--font-display); font-weight: 700; font-size: 1.1rem;
  display: flex; align-items: center; justify-content: center;
  border: 1px solid rgba(200,146,58,0.25);
}
.user-card-info { flex: 1; display: flex; flex-direction: column; gap: 2px; }
.user-card-name { font-weight: 600; color: var(--text); font-size: 0.95rem; }
.user-card-id { font-size: 0.78rem; }
.user-card-arrow { color: var(--text-3); flex-shrink: 0; transition: color var(--transition); }
.user-card:hover .user-card-arrow { color: var(--amber); }
</style>
