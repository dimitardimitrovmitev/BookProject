<template>
  <div>
    <div class="page-header">
      <div class="page-header-left">
        <h1>Books</h1>
        <p>Explore the literary collection</p>
      </div>
      <div class="page-actions">
        <button v-if="authStore.isAdmin" class="btn btn-primary" @click="showForm = true">
          + Add Book
        </button>
      </div>
    </div>

    <!-- Filters -->
    <div class="filters-bar">
      <div class="search-bar" style="flex:1;max-width:360px">
        <svg class="search-icon" width="16" height="16" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><circle cx="11" cy="11" r="8"/><path d="m21 21-4.35-4.35"/></svg>
        <input v-model="search" class="form-input" placeholder="Search by title or author…" @input="onSearch" />
      </div>
      <select v-model="sortBy" class="form-select" style="width:180px" @change="fetchBooks">
        <option value="title">Sort by Title</option>
        <option value="author">Sort by Author</option>
        <option value="publishedDate">Sort by Date</option>
      </select>
      <select v-model="sortDesc" class="form-select" style="width:120px" @change="fetchBooks">
        <option :value="false">Asc</option>
        <option :value="true">Desc</option>
      </select>
    </div>

    <!-- Grid -->
    <div v-if="loading" class="grid-4" style="margin-top:24px">
      <div v-for="i in 8" :key="i" class="skeleton" style="height:280px;border-radius:12px" />
    </div>

    <div v-else-if="books.length" class="grid-4" style="margin-top:24px">
      <div v-for="book in books" :key="book.id" style="position:relative">
        <BookCard :book="book" />
        <div v-if="authStore.isAdmin" class="admin-actions">
          <button class="btn-icon" @click.prevent="editBook(book)" title="Edit">✎</button>
          <button class="btn-icon" @click.prevent="confirmDelete(book)" title="Delete" style="color:var(--danger)">✕</button>
        </div>
      </div>
    </div>

    <div v-else class="empty-state">
      <div class="empty-state-icon">📚</div>
      <h3>No books found</h3>
      <p>Try a different search term</p>
    </div>

    <div style="margin-top:32px">
      <AppPagination v-model="page" :total-pages="totalPages" @update:model-value="fetchBooks" />
    </div>

    <!-- Modals -->
    <BookFormModal
      :show="showForm"
      :book="editingBook"
      :loading="formLoading"
      @close="closeForm"
      @submit="handleSubmit"
    />
    <ConfirmModal
      :show="showDelete"
      title="Delete Book"
      :message="`Delete '${deletingBook?.title}'? This cannot be undone.`"
      :loading="deleteLoading"
      @confirm="handleDelete"
      @cancel="showDelete = false"
    />
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useAuthStore } from '@/stores/auth'
import { booksApi } from '@/api/books'
import { useToast } from '@/composables/useToast'
import BookCard from '@/components/books/BookCard.vue'
import BookFormModal from '@/components/books/BookFormModal.vue'
import AppPagination from '@/components/common/AppPagination.vue'
import ConfirmModal from '@/components/common/ConfirmModal.vue'

const authStore = useAuthStore()
const toast = useToast()

const books = ref([])
const loading = ref(false)
const page = ref(1)
const totalPages = ref(1)
const search = ref('')
const sortBy = ref('title')
const sortDesc = ref(false)
let searchTimer = null

const showForm = ref(false)
const editingBook = ref(null)
const formLoading = ref(false)
const showDelete = ref(false)
const deletingBook = ref(null)
const deleteLoading = ref(false)

async function fetchBooks() {
  loading.value = true
  try {
    const { data } = await booksApi.getAll({
      pageNumber: page.value,
      pageSize: 12,
      search: search.value || undefined,
      sortBy: sortBy.value,
      isDescending: sortDesc.value,
    })
    books.value = data.items
    totalPages.value = data.totalPages
  } catch { toast.error('Failed to load books') }
  finally { loading.value = false }
}

function onSearch() {
  clearTimeout(searchTimer)
  searchTimer = setTimeout(() => { page.value = 1; fetchBooks() }, 400)
}

function editBook(book) { editingBook.value = book; showForm.value = true }
function closeForm() { showForm.value = false; editingBook.value = null }
function confirmDelete(book) { deletingBook.value = book; showDelete.value = true }

async function handleSubmit(formData) {
  formLoading.value = true
  try {
    if (editingBook.value) {
      await booksApi.update(editingBook.value.id, formData)
      toast.success('Book updated')
    } else {
      await booksApi.create(formData)
      toast.success('Book added')
    }
    closeForm(); fetchBooks()
  } catch (e) { toast.error(e.response?.data || 'Failed to save book') }
  finally { formLoading.value = false }
}

async function handleDelete() {
  deleteLoading.value = true
  try {
    await booksApi.delete(deletingBook.value.id)
    toast.success('Book deleted')
    showDelete.value = false; deletingBook.value = null; fetchBooks()
  } catch { toast.error('Failed to delete book') }
  finally { deleteLoading.value = false }
}

onMounted(fetchBooks)
</script>

<style scoped>
.filters-bar { display: flex; gap: 12px; flex-wrap: wrap; align-items: center; }
.admin-actions {
  position: absolute; top: 8px; right: 8px;
  display: flex; gap: 4px; opacity: 0; transition: opacity var(--transition);
}
div:hover > .admin-actions { opacity: 1; }
</style>
