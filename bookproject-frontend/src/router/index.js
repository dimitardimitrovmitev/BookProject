import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const routes = [
  { path: '/', redirect: '/books' },
  { path: '/login', component: () => import('@/views/LoginView.vue'), meta: { guest: true } },
  { path: '/register', component: () => import('@/views/RegisterView.vue'), meta: { guest: true } },
  { path: '/books', component: () => import('@/views/BooksView.vue'), meta: { auth: true } },
  { path: '/books/:id', component: () => import('@/views/BookDetailView.vue'), meta: { auth: true } },
  { path: '/characters', component: () => import('@/views/CharactersView.vue'), meta: { auth: true } },
  { path: '/characters/:id', component: () => import('@/views/CharacterDetailView.vue'), meta: { auth: true } },
  { path: '/my-library', component: () => import('@/views/MyLibraryView.vue'), meta: { auth: true } },
  { path: '/scenes', component: () => import('@/views/ScenesView.vue'), meta: { auth: true } },
  { path: '/users', component: () => import('@/views/UsersView.vue'), meta: { auth: true } },
  { path: '/users/:id', component: () => import('@/views/UserProfileView.vue'), meta: { auth: true } },
  { path: '/profile', component: () => import('@/views/ProfileView.vue'), meta: { auth: true } },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
  scrollBehavior: () => ({ top: 0 })
})

router.beforeEach((to) => {
  const auth = useAuthStore()
  if (to.meta.auth && !auth.isAuthenticated) return '/login'
  if (to.meta.guest && auth.isAuthenticated) return '/books'
})

export default router
