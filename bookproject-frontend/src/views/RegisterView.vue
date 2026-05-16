<template>
  <div class="auth-layout">
    <div class="auth-card">
      <div class="auth-brand">
        <span class="auth-icon">📖</span>
        <h1>Join BookProject</h1>
        <p>Start your literary journey</p>
      </div>

      <div style="display:flex;flex-direction:column;gap:16px">
        <div class="form-group">
          <label class="form-label">Username</label>
          <input v-model="form.username" class="form-input" placeholder="your_username" @keyup.enter="submit" />
          <span v-if="errors.username" class="form-error">{{ errors.username }}</span>
        </div>
        <div class="form-group">
          <label class="form-label">Email</label>
          <input v-model="form.email" type="email" class="form-input" placeholder="you@example.com" @keyup.enter="submit" />
          <span v-if="errors.email" class="form-error">{{ errors.email }}</span>
        </div>
        <div class="form-group">
          <label class="form-label">Password</label>
          <input v-model="form.password" type="password" class="form-input" placeholder="Min 8 characters" @keyup.enter="submit" />
          <span v-if="errors.password" class="form-error">{{ errors.password }}</span>
        </div>
        <div v-if="serverError" class="form-error" style="padding:10px;background:rgba(184,84,80,0.1);border-radius:var(--radius)">
          {{ serverError }}
        </div>
        <div v-if="success" style="padding:10px;background:rgba(74,158,107,0.1);border-radius:var(--radius);border:1px solid rgba(74,158,107,0.2);color:#6dc48a;font-size:0.9rem">
          Registered! <RouterLink to="/login">Sign in now →</RouterLink>
        </div>
        <button class="btn btn-primary w-full" :disabled="loading || success" @click="submit" style="justify-content:center;margin-top:4px">
          {{ loading ? 'Creating account…' : 'Create Account' }}
        </button>
      </div>

      <p class="auth-switch">
        Already have an account? <RouterLink to="/login">Sign in</RouterLink>
      </p>
    </div>

    <div class="auth-deco">
      <div class="deco-quote">
        <p class="quote-line">"Not all those who wander are lost."</p>
        <span class="deco-attr">— J.R.R. Tolkien, The Fellowship of the Ring</span>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useAuthStore } from '@/stores/auth'

const authStore = useAuthStore()
const form = ref({ username: '', email: '', password: '' })
const errors = ref({})
const serverError = ref('')
const loading = ref(false)
const success = ref(false)

function validate() {
  errors.value = {}
  if (!form.value.username || form.value.username.length < 3) errors.value.username = 'Username must be at least 3 characters'
  if (!form.value.email) errors.value.email = 'Email is required'
  if (!form.value.password || form.value.password.length < 8) errors.value.password = 'Password must be at least 8 characters'
  return !Object.keys(errors.value).length
}

async function submit() {
  if (!validate()) return
  loading.value = true
  serverError.value = ''
  try {
    await authStore.register(form.value)
    success.value = true
  } catch (e) {
    serverError.value = e.response?.data || 'Registration failed'
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.auth-layout { min-height: 100vh; display: flex; }
.auth-card { width: 420px; flex-shrink: 0; padding: 48px 40px; background: var(--bg-2); border-right: 1px solid var(--border); display: flex; flex-direction: column; gap: 28px; justify-content: center; }
.auth-brand { text-align: center; }
.auth-icon { font-size: 2.5rem; display: block; margin-bottom: 12px; }
.auth-brand h1 { font-size: 2rem; }
.auth-brand p { color: var(--text-3); font-style: italic; }
.auth-switch { text-align: center; color: var(--text-3); font-size: 0.9rem; }
.auth-deco { flex: 1; display: flex; align-items: center; justify-content: center; padding: 60px; background: radial-gradient(ellipse at 70% 40%, rgba(200,146,58,0.06) 0%, transparent 60%); }
.deco-quote { max-width: 500px; }
.deco-quote .quote-line { font-size: 1.3rem; line-height: 1.7; }
.deco-attr { display: block; margin-top: 16px; color: var(--text-3); font-style: italic; }
@media (max-width: 700px) { .auth-deco { display: none; } .auth-card { width: 100%; border-right: none; } }
</style>
