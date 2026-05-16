import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { authApi } from '@/api/auth'

function parseJwt(token) {
  try {
    const base64 = token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/')
    return JSON.parse(atob(base64))
  } catch {
    return null
  }
}

export const useAuthStore = defineStore('auth', () => {
  const token = ref(localStorage.getItem('token') || null)
  const user = ref(null)

  const isAuthenticated = computed(() => !!token.value)
  const isAdmin = computed(() => {
    if (!token.value) return false
    const payload = parseJwt(token.value)
    return payload?.['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] === 'Admin'
  })
  const userId = computed(() => {
    if (!token.value) return null
    const payload = parseJwt(token.value)
    return payload?.userId ? parseInt(payload.userId) : null
  })
  const username = computed(() => {
    if (!token.value) return null
    const payload = parseJwt(token.value)
    return payload?.['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || null
  })

  async function login(credentials) {
    const { data } = await authApi.login(credentials)
    token.value = data.token
    localStorage.setItem('token', data.token)
  }

  async function register(data) {
    await authApi.register(data)
  }

  function logout() {
    token.value = null
    user.value = null
    localStorage.removeItem('token')
  }

  return { token, user, isAuthenticated, isAdmin, userId, username, login, register, logout }
})
