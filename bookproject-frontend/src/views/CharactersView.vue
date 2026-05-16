<template>
  <div>
    <div class="page-header">
      <div class="page-header-left">
        <h1>Characters</h1>
        <p>All characters across the collection</p>
      </div>
      <div class="page-actions">
        <button v-if="authStore.isAdmin" class="btn btn-primary" @click="showForm = true">
          + Add Character
        </button>
      </div>
    </div>

    <div class="filters-bar">
      <div class="search-bar" style="flex:1;max-width:360px">
        <svg class="search-icon" width="16" height="16" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><circle cx="11" cy="11" r="8"/><path d="m21 21-4.35-4.35"/></svg>
        <input v-model="search" class="form-input" placeholder="Search characters…" @input="onSearch" />
      </div>
      <select v-model="verifiedFilter" class="form-select" style="width:150px" @change="fetchCharacters">
        <option value="">All Characters</option>
        <option value="true">Verified Only</option>
        <option value="false">Unverified</option>
      </select>
    </div>

    <div v-if="loading" class="grid-4" style="margin-top:24px">
      <div v-for="i in 8" :key="i" class="skeleton" style="height:200px;border-radius:12px" />
    </div>

    <div v-else-if="characters.length" class="grid-4" style="margin-top:24px">
      <div v-for="c in characters" :key="c.characterId" style="position:relative">
        <CharacterCard :character="c" />
        <div v-if="authStore.isAdmin" class="admin-actions">
          <button class="btn-icon" @click.prevent="editChar(c)">✎</button>
          <button class="btn-icon" @click.prevent="confirmDelete(c)" style="color:var(--danger)">✕</button>
        </div>
      </div>
    </div>

    <div v-else class="empty-state">
      <div class="empty-state-icon">👤</div>
      <h3>No characters found</h3>
    </div>

    <div style="margin-top:32px">
      <AppPagination v-model="page" :total-pages="totalPages" @update:model-value="fetchCharacters" />
    </div>

    <CharacterFormModal :show="showForm" :character="editingChar" :books="books" :loading="formLoading" @close="closeForm" @submit="handleSubmit" />
    <ConfirmModal :show="showDelete" title="Delete Character" :message="`Delete '${deletingChar?.name}'?`" :loading="deleteLoading" @confirm="handleDelete" @cancel="showDelete=false" />
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useAuthStore } from '@/stores/auth'
import { charactersApi } from '@/api/index'
import { booksApi } from '@/api/books'
import { useToast } from '@/composables/useToast'
import CharacterCard from '@/components/characters/CharacterCard.vue'
import CharacterFormModal from '@/components/characters/CharacterFormModal.vue'
import AppPagination from '@/components/common/AppPagination.vue'
import ConfirmModal from '@/components/common/ConfirmModal.vue'

const authStore = useAuthStore()
const toast = useToast()

const characters = ref([])
const books = ref([])
const loading = ref(false)
const page = ref(1)
const totalPages = ref(1)
const search = ref('')
const verifiedFilter = ref('')
let searchTimer = null

const showForm = ref(false)
const editingChar = ref(null)
const formLoading = ref(false)
const showDelete = ref(false)
const deletingChar = ref(null)
const deleteLoading = ref(false)

async function fetchCharacters() {
  loading.value = true
  try {
    const params = { pageNumber: page.value, pageSize: 12, search: search.value || undefined }
    if (verifiedFilter.value !== '') params.verified = verifiedFilter.value === 'true'
    const { data } = await charactersApi.getAll(params)
    characters.value = data.items
    totalPages.value = data.totalPages
  } catch { toast.error('Failed to load characters') }
  finally { loading.value = false }
}

async function fetchBooks() {
  try {
    const { data } = await booksApi.getAll({ pageSize: 200 })
    books.value = data.items
  } catch {}
}

function onSearch() {
  clearTimeout(searchTimer)
  searchTimer = setTimeout(() => { page.value = 1; fetchCharacters() }, 400)
}

function editChar(c) { editingChar.value = c; showForm.value = true }
function closeForm() { showForm.value = false; editingChar.value = null }
function confirmDelete(c) { deletingChar.value = c; showDelete.value = true }

async function handleSubmit(formData) {
  formLoading.value = true
  try {
    if (editingChar.value) {
      await charactersApi.update(editingChar.value.characterId, formData)
      toast.success('Character updated')
    } else {
      await charactersApi.create(formData)
      toast.success('Character added')
    }
    closeForm(); fetchCharacters()
  } catch (e) { toast.error(e.response?.data || 'Failed to save') }
  finally { formLoading.value = false }
}

async function handleDelete() {
  deleteLoading.value = true
  try {
    await charactersApi.delete(deletingChar.value.characterId)
    toast.success('Character deleted')
    showDelete.value = false; deletingChar.value = null; fetchCharacters()
  } catch { toast.error('Failed to delete') }
  finally { deleteLoading.value = false }
}

onMounted(() => { fetchCharacters(); fetchBooks() })
</script>

<style scoped>
.filters-bar { display: flex; gap: 12px; flex-wrap: wrap; align-items: center; }
.admin-actions { position: absolute; top: 8px; right: 8px; display: flex; gap: 4px; opacity: 0; transition: opacity var(--transition); }
div:hover > .admin-actions { opacity: 1; }
</style>
