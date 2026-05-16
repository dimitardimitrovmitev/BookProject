<template>
  <div>
    <div class="page-header">
      <div class="page-header-left">
        <h1>Profile</h1>
        <p>Manage your account</p>
      </div>
    </div>

    <div class="grid-2" style="max-width:800px;gap:24px">
      <!-- Account info -->
      <div class="card">
        <h3 style="margin-bottom:20px">Account Info</h3>
        <div style="display:flex;flex-direction:column;gap:14px">
          <div class="profile-field">
            <span class="form-label">Username</span>
            <span>{{ authStore.username }}</span>
          </div>
          <div class="profile-field">
            <span class="form-label">Role</span>
            <span :class="authStore.isAdmin ? 'badge badge-amber' : 'badge badge-gray'">
              {{ authStore.isAdmin ? 'Admin' : 'User' }}
            </span>
          </div>
          <div class="profile-field">
            <span class="form-label">User ID</span>
            <span class="text-muted">#{{ authStore.userId }}</span>
          </div>
        </div>
      </div>

      <!-- Reading stats -->
      <div class="card">
        <h3 style="margin-bottom:20px">Reading Stats</h3>
        <div v-if="statsLoading" style="display:flex;flex-direction:column;gap:10px">
          <div class="skeleton" style="height:20px" v-for="i in 4" :key="i" />
        </div>
        <div v-else style="display:flex;flex-direction:column;gap:12px">
          <div class="stat-row" v-for="s in stats" :key="s.label">
            <span class="stat-label">{{ s.label }}</span>
            <span class="stat-value">{{ s.value }}</span>
          </div>
        </div>
      </div>

      <!-- Update account -->
      <div class="card" style="grid-column:span 2">
        <h3 style="margin-bottom:20px">Update Account</h3>
        <div class="grid-2" style="gap:16px">
          <div class="form-group">
            <label class="form-label">New Username</label>
            <input v-model="updateForm.username" class="form-input" placeholder="Leave blank to keep current" />
          </div>
          <div class="form-group">
            <label class="form-label">New Email</label>
            <input v-model="updateForm.email" type="email" class="form-input" placeholder="Leave blank to keep current" />
          </div>
          <div class="form-group">
            <label class="form-label">Current Password</label>
            <input v-model="updateForm.currentPassword" type="password" class="form-input" placeholder="Required if changing password" />
          </div>
          <div class="form-group">
            <label class="form-label">New Password</label>
            <input v-model="updateForm.newPassword" type="password" class="form-input" placeholder="Min 8 characters" />
          </div>
        </div>
        <div style="margin-top:20px;display:flex;gap:10px;align-items:center">
          <button class="btn btn-primary" :disabled="updateLoading" @click="handleUpdate">
            {{ updateLoading ? 'Saving…' : 'Save Changes' }}
          </button>
          <span v-if="updateSuccess" style="color:var(--success);font-size:0.9rem">✓ Updated successfully</span>
          <span v-if="updateError" style="color:var(--danger);font-size:0.9rem">{{ updateError }}</span>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import { useAuthStore } from '@/stores/auth'
import { userBooksApi } from '@/api/index'
import { usersApi } from '@/api/users'
import { useToast } from '@/composables/useToast'

const authStore = useAuthStore()
const toast = useToast()

const statsLoading = ref(true)
const updateLoading = ref(false)
const updateSuccess = ref(false)
const updateError = ref('')
const updateForm = ref({ username: '', email: '', currentPassword: '', newPassword: '' })
const userBooks = ref([])

const stats = computed(() => {
  const total = userBooks.value.length
  const read = userBooks.value.filter(ub => ub.status === 'Read').length
  const reading = userBooks.value.filter(ub => ub.status === 'Reading').length
  const wtr = userBooks.value.filter(ub => ub.status === 'WantToRead').length
  const rated = userBooks.value.filter(ub => ub.userRating)
  const avgRating = rated.length ? (rated.reduce((a, b) => a + b.userRating, 0) / rated.length).toFixed(1) : '—'
  return [
    { label: 'Total in Library', value: total },
    { label: 'Read', value: read },
    { label: 'Currently Reading', value: reading },
    { label: 'Want to Read', value: wtr },
    { label: 'Avg Rating Given', value: avgRating },
  ]
})

async function fetchStats() {
  statsLoading.value = true
  try {
    const { data } = await userBooksApi.getMy({ pageSize: 500 })
    userBooks.value = data.items || []
  } catch {}
  finally { statsLoading.value = false }
}

async function handleUpdate() {
  updateLoading.value = true
  updateSuccess.value = false
  updateError.value = ''
  const payload = {}
  if (updateForm.value.username) payload.username = updateForm.value.username
  if (updateForm.value.email) payload.email = updateForm.value.email
  if (updateForm.value.currentPassword) payload.currentPassword = updateForm.value.currentPassword
  if (updateForm.value.newPassword) payload.newPassword = updateForm.value.newPassword

  try {
    await usersApi.update(payload)
    updateSuccess.value = true
    updateForm.value = { username: '', email: '', currentPassword: '', newPassword: '' }
    toast.success('Account updated!')
  } catch (e) { updateError.value = e.response?.data || 'Update failed' }
  finally { updateLoading.value = false }
}

onMounted(fetchStats)
</script>

<style scoped>
.profile-field { display:flex;flex-direction:column;gap:4px; }
.stat-row { display:flex;align-items:center;justify-content:space-between;padding:8px 0;border-bottom:1px solid var(--border); }
.stat-row:last-child { border-bottom:none; }
.stat-label { color:var(--text-2);font-size:0.9rem; }
.stat-value { font-family:var(--font-display);font-weight:700;color:var(--amber); }
</style>
