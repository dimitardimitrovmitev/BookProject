<template>
  <aside class="sidebar">
    <!-- Logo -->
    <div class="sidebar-logo">
      <div class="logo-icon">📖</div>
      <div class="logo-text">
        <span class="logo-title">BookProject</span>
        <span class="logo-sub">Literary Companion</span>
      </div>
    </div>

    <!-- Nav -->
    <nav class="sidebar-nav">
      <div class="nav-section-label">Library</div>
      <RouterLink to="/books" class="nav-item">
        <svg width="18" height="18" fill="none" stroke="currentColor" stroke-width="1.8" viewBox="0 0 24 24"><path d="M4 19.5A2.5 2.5 0 0 1 6.5 17H20"/><path d="M6.5 2H20v20H6.5A2.5 2.5 0 0 1 4 19.5v-15A2.5 2.5 0 0 1 6.5 2z"/></svg>
        Books
      </RouterLink>
      <RouterLink to="/my-library" class="nav-item">
        <svg width="18" height="18" fill="none" stroke="currentColor" stroke-width="1.8" viewBox="0 0 24 24"><path d="M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z"/></svg>
        My Library
      </RouterLink>
      <RouterLink to="/characters" class="nav-item">
        <svg width="18" height="18" fill="none" stroke="currentColor" stroke-width="1.8" viewBox="0 0 24 24"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/></svg>
        Characters
      </RouterLink>
      <RouterLink to="/scenes" class="nav-item">
        <svg width="18" height="18" fill="none" stroke="currentColor" stroke-width="1.8" viewBox="0 0 24 24"><rect x="3" y="3" width="18" height="18" rx="2"/><path d="M3 9h18M9 21V9"/></svg>
        Scenes
      </RouterLink>

      <RouterLink to="/users" class="nav-item">
        <svg width="18" height="18" fill="none" stroke="currentColor" stroke-width="1.8" viewBox="0 0 24 24"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/></svg>
        Users
      </RouterLink>

      <div class="nav-section-label" style="margin-top:16px">Account</div>
      <RouterLink to="/profile" class="nav-item">
        <svg width="18" height="18" fill="none" stroke="currentColor" stroke-width="1.8" viewBox="0 0 24 24"><path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/><circle cx="12" cy="7" r="4"/></svg>
        Profile
      </RouterLink>
    </nav>

    <!-- User footer -->
    <div class="sidebar-footer">
      <div class="user-info">
        <div class="user-avatar">{{ usernameInitial }}</div>
        <div class="user-details">
          <span class="user-name">{{ authStore.username || 'User' }}</span>
          <span v-if="authStore.isAdmin" class="badge badge-amber" style="font-size:0.65rem;padding:1px 6px">Admin</span>
        </div>
      </div>
      <button class="btn-icon" @click="handleLogout" title="Logout">
        <svg width="16" height="16" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"/><polyline points="16 17 21 12 16 7"/><line x1="21" y1="12" x2="9" y2="12"/></svg>
      </button>
    </div>
  </aside>
</template>

<script setup>
import { computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const authStore = useAuthStore()
const router = useRouter()

const usernameInitial = computed(() =>
  (authStore.username || 'U').charAt(0).toUpperCase()
)

function handleLogout() {
  authStore.logout()
  router.push('/login')
}
</script>

<style scoped>
.sidebar-logo {
  display: flex; align-items: center; gap: 12px;
  padding: 22px 20px 18px;
  border-bottom: 1px solid var(--border);
}
.logo-icon { font-size: 1.6rem; }
.logo-text { display: flex; flex-direction: column; }
.logo-title { font-family: var(--font-display); font-weight: 700; font-size: 1.1rem; color: var(--text); line-height: 1.1; }
.logo-sub   { font-size: 0.7rem; color: var(--text-3); text-transform: uppercase; letter-spacing: 0.08em; }

.sidebar-nav { flex: 1; padding: 16px 10px; display: flex; flex-direction: column; gap: 2px; overflow-y: auto; }
.nav-section-label { font-size: 0.68rem; text-transform: uppercase; letter-spacing: 0.1em; color: var(--text-3); padding: 4px 10px 6px; font-family: var(--font-body); font-weight: 600; }

.nav-item {
  display: flex; align-items: center; gap: 10px;
  padding: 9px 12px; border-radius: var(--radius);
  color: var(--text-2); text-decoration: none;
  font-size: 0.95rem; font-family: var(--font-body);
  transition: all var(--transition);
}
.nav-item:hover { background: var(--bg-3); color: var(--text); }
.nav-item.router-link-active { background: var(--amber-dim); color: var(--amber-2); }
.nav-item.router-link-active svg { stroke: var(--amber-2); }

.sidebar-footer {
  padding: 14px 14px;
  border-top: 1px solid var(--border);
  display: flex; align-items: center; justify-content: space-between;
}
.user-info { display: flex; align-items: center; gap: 10px; }
.user-avatar {
  width: 32px; height: 32px; border-radius: 50%;
  background: var(--amber-dim); color: var(--amber-2);
  font-family: var(--font-display); font-weight: 700; font-size: 0.9rem;
  display: flex; align-items: center; justify-content: center;
  border: 1px solid rgba(200,146,58,0.3);
}
.user-details { display: flex; flex-direction: column; gap: 2px; }
.user-name { font-size: 0.85rem; color: var(--text); font-weight: 600; }
</style>
