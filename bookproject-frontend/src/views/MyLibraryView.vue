<template>
  <div>
    <div class="page-header">
      <div class="page-header-left">
        <h1>My Library</h1>
        <p>Your personal reading list</p>
      </div>
    </div>

    <!-- Status tabs -->
    <div class="status-tabs">
      <button v-for="tab in tabs" :key="tab.value" :class="['tab-btn', { active: activeTab === tab.value }]" @click="activeTab = tab.value">
        {{ tab.label }}
        <span class="tab-count">{{ countByStatus(tab.value) }}</span>
      </button>
    </div>

    <div v-if="loading" class="grid-4" style="margin-top:24px">
      <div v-for="i in 6" :key="i" class="skeleton" style="height:260px;border-radius:12px" />
    </div>

    <div v-else-if="filteredBooks.length" class="grid-4" style="margin-top:24px">
      <div v-for="ub in filteredBooks" :key="ub.bookId" class="library-card card card-hover">
        <RouterLink :to="`/books/${ub.bookId}`" style="text-decoration:none;color:inherit">
          <div class="lib-cover">
            <img v-if="!coverErrors[ub.bookId]" :src="coverUrl(ub)" :alt="bookMap[ub.bookId]?.title" @error="coverErrors[ub.bookId] = true" />
            <div v-else class="lib-cover-fallback">📖</div>
          </div>
          <div class="lib-info">
            <h3 class="lib-title">{{ bookMap[ub.bookId]?.title || `Book #${ub.bookId}` }}</h3>
            <p class="lib-author">{{ bookMap[ub.bookId]?.author }}</p>
          </div>
        </RouterLink>
        <div class="lib-controls">
          <select :value="ub.status" class="form-select" style="font-size:0.82rem;padding:5px 10px" @change="updateStatus(ub, $event.target.value)">
            <option value="WantToRead">Want to Read</option>
            <option value="Reading">Reading</option>
            <option value="Read">Read</option>
            <option value="DNF">DNF</option>
          </select>
          <div class="lib-rating" v-if="ub.userRating">
            <StarRating :value="ub.userRating" :show-value="true" />
          </div>
          <button class="btn btn-ghost btn-sm" @click="confirmRemove(ub)">Remove</button>
        </div>
      </div>
    </div>

    <div v-else class="empty-state">
      <div class="empty-state-icon">📚</div>
      <h3>No books here yet</h3>
      <p v-if="activeTab === 'all'">Browse the collection and add books to your library!</p>
      <RouterLink to="/books" class="btn btn-primary" style="margin-top:16px;display:inline-flex">Browse Books</RouterLink>
    </div>

    <ConfirmModal :show="showDelete" title="Remove from Library" :message="`Remove '${removingBook?.bookId}' from your library?`" :loading="deleteLoading" @confirm="handleRemove" @cancel="showDelete=false" />
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { userBooksApi } from '@/api/index'
import { booksApi } from '@/api/books'
import { useToast } from '@/composables/useToast'
import StarRating from '@/components/common/StarRating.vue'
import ConfirmModal from '@/components/common/ConfirmModal.vue'

const toast = useToast()
const userBooks = ref([])
const bookMap = ref({})
const loading = ref(false)
const activeTab = ref('all')
const coverErrors = ref({})
const showDelete = ref(false)
const removingBook = ref(null)
const deleteLoading = ref(false)

const tabs = [
  { label: 'All', value: 'all' },
  { label: 'Want to Read', value: 'WantToRead' },
  { label: 'Reading', value: 'Reading' },
  { label: 'Read', value: 'Read' },
  { label: 'DNF', value: 'DNF' },
]

const filteredBooks = computed(() =>
  activeTab.value === 'all' ? userBooks.value : userBooks.value.filter(ub => ub.status === activeTab.value)
)

function countByStatus(s) {
  return s === 'all' ? userBooks.value.length : userBooks.value.filter(ub => ub.status === s).length
}

function coverUrl(ub) {
  const b = bookMap.value[ub.bookId]
  return b?.openLibraryId ? `https://covers.openlibrary.org/b/olid/${b.openLibraryId}-M.jpg` : ''
}

async function fetchLibrary() {
  loading.value = true
  try {
    const { data } = await userBooksApi.getMy({ pageSize: 200 })
    userBooks.value = data.items || []
    // Fetch book details
    const ids = [...new Set(userBooks.value.map(ub => ub.bookId))]
    const results = await Promise.allSettled(ids.map(id => booksApi.getById(id)))
    results.forEach((r, i) => {
      if (r.status === 'fulfilled') bookMap.value[ids[i]] = r.value.data
    })
  } catch {}
  finally { loading.value = false }
}

async function updateStatus(ub, status) {
  try {
    const { data } = await userBooksApi.updateStatus(ub.bookId, { status })
    const idx = userBooks.value.findIndex(x => x.bookId === ub.bookId)
    if (idx !== -1) userBooks.value[idx] = data
    toast.success('Status updated')
  } catch { toast.error('Failed to update') }
}

function confirmRemove(ub) { removingBook.value = ub; showDelete.value = true }

async function handleRemove() {
  deleteLoading.value = true
  try {
    await userBooksApi.remove(removingBook.value.bookId)
    userBooks.value = userBooks.value.filter(ub => ub.bookId !== removingBook.value.bookId)
    showDelete.value = false; removingBook.value = null
    toast.success('Removed from library')
  } catch { toast.error('Failed to remove') }
  finally { deleteLoading.value = false }
}

onMounted(fetchLibrary)
</script>

<style scoped>
.status-tabs { display:flex;gap:6px;flex-wrap:wrap;border-bottom:1px solid var(--border);padding-bottom:0;margin-bottom:0; }
.tab-btn { background:none;border:none;border-bottom:2px solid transparent;padding:10px 14px;color:var(--text-3);cursor:pointer;font-family:var(--font-body);font-size:0.9rem;display:flex;align-items:center;gap:6px;transition:all var(--transition);margin-bottom:-1px; }
.tab-btn:hover { color:var(--text); }
.tab-btn.active { color:var(--amber);border-bottom-color:var(--amber); }
.tab-count { font-size:0.72rem;padding:1px 6px;background:var(--bg-4);border-radius:20px;color:var(--text-3); }
.tab-btn.active .tab-count { background:var(--amber-dim);color:var(--amber); }
.library-card { padding:0;overflow:hidden; }
.lib-cover { height:160px;background:var(--bg-3);overflow:hidden; }
.lib-cover img { width:100%;height:100%;object-fit:cover; }
.lib-cover-fallback { height:100%;display:flex;align-items:center;justify-content:center;font-size:2.5rem;opacity:0.3; }
.lib-info { padding:12px 14px 8px; }
.lib-title { font-family:var(--font-display);font-size:0.95rem;color:var(--text);line-height:1.3; }
.lib-author { font-size:0.8rem;color:var(--text-3);font-style:italic;margin-top:4px; }
.lib-controls { padding:8px 14px 14px;display:flex;flex-direction:column;gap:8px; }
.lib-rating { display:flex;align-items:center; }
</style>
