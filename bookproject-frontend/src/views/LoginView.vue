<template>
  <div class="auth-layout">
    <div class="auth-card">
      <div class="auth-brand">
        <span class="auth-icon">📖</span>
        <h1>BookProject</h1>
        <p>Your literary companion</p>
      </div>

      <div style="display:flex;flex-direction:column;gap:16px">
        <div class="form-group">
          <label class="form-label">Email</label>
          <input v-model="form.email" type="email" class="form-input" placeholder="you@example.com" @keyup.enter="submit" />
          <span v-if="errors.email" class="form-error">{{ errors.email }}</span>
        </div>
        <div class="form-group">
          <label class="form-label">Password</label>
          <input v-model="form.password" type="password" class="form-input" placeholder="••••••••" @keyup.enter="submit" />
          <span v-if="errors.password" class="form-error">{{ errors.password }}</span>
        </div>
        <div v-if="serverError" class="form-error" style="padding:10px;background:rgba(184,84,80,0.1);border-radius:var(--radius);border:1px solid rgba(184,84,80,0.2)">
          {{ serverError }}
        </div>
        <button class="btn btn-primary w-full" :disabled="loading" @click="submit" style="justify-content:center;margin-top:4px">
          {{ loading ? 'Signing in…' : 'Sign In' }}
        </button>
      </div>

      <p class="auth-switch">
        No account? <RouterLink to="/register">Register here</RouterLink>
      </p>
    </div>

    <div class="auth-deco">
      <div class="deco-quote">
        <p class="quote-line">"A reader lives a thousand lives before he dies. The man who never reads lives only one."</p>
        <span class="deco-attr">— George R.R. Martin</span>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const router = useRouter()
const authStore = useAuthStore()
const form = ref({ email: '', password: '' })
const errors = ref({})
const serverError = ref('')
const loading = ref(false)

function validate() {
  errors.value = {}
  if (!form.value.email) errors.value.email = 'Email is required'
  if (!form.value.password) errors.value.password = 'Password is required'
  return !Object.keys(errors.value).length
}

async function submit() {
  if (!validate()) return
  loading.value = true
  serverError.value = ''
  try {
    await authStore.login(form.value)
    router.push('/books')
  } catch (e) {
    serverError.value = e.response?.data || 'Invalid email or password'
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.auth-layout {
  min-height: 100vh; display: flex;
}
.auth-card {
  width: 420px; flex-shrink: 0;
  padding: 48px 40px;
  background: var(--bg-2);
  border-right: 1px solid var(--border);
  display: flex; flex-direction: column; gap: 28px;
  justify-content: center;
}
.auth-brand { text-align: center; }
.auth-icon { font-size: 2.5rem; display: block; margin-bottom: 12px; }
.auth-brand h1 { font-size: 2rem; }
.auth-brand p { color: var(--text-3); font-style: italic; }
.auth-switch { text-align: center; color: var(--text-3); font-size: 0.9rem; }

.auth-deco {
  flex: 1; display: flex; align-items: center; justify-content: center;
  padding: 60px;
  background: radial-gradient(ellipse at 30% 60%, rgba(200,146,58,0.06) 0%, transparent 60%);
}
.deco-quote { max-width: 500px; }
.deco-quote .quote-line { font-size: 1.3rem; line-height: 1.7; }
.deco-attr { display: block; margin-top: 16px; color: var(--text-3); font-style: italic; }

@media (max-width: 700px) {
  .auth-deco { display: none; }
  .auth-card { width: 100%; border-right: none; }
}
</style>
