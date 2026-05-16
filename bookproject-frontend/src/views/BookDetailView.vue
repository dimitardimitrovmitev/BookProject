<template>
  <div v-if="book">
    <!-- Header -->
    <div class="book-hero">
      <div class="book-cover-large">
        <img v-if="!imgError && coverUrl" :src="coverUrl" :alt="book.title" @error="imgError = true" />
        <div v-else class="cover-fallback">📖</div>
      </div>
      <div class="book-hero-info">
        <RouterLink to="/books" class="back-link">← Back to Books</RouterLink>
        <h1>{{ book.title }}</h1>
        <p class="hero-author">by <span class="text-amber">{{ book.author }}</span></p>
        <p class="hero-desc">{{ book.description }}</p>
        <div class="hero-meta">
          <span class="badge badge-gray">{{ pubYear }}</span>
          <span class="badge badge-blue">{{ book.openLibraryId }}</span>
        </div>
        <div class="hero-actions">
          <button v-if="!inLibrary" class="btn btn-primary" :disabled="libraryLoading" @click="addToLibrary">
            + Add to Library
          </button>
          <template v-else>
            <div class="library-status">
              <span class="badge badge-green">✓ In Library</span>
              <select v-model="statusValue" class="form-select" style="width:160px" @change="updateStatus">
                <option value="WantToRead">Want to Read</option>
                <option value="Reading">Reading</option>
                <option value="Read">Read</option>
                <option value="DNF">DNF</option>
              </select>
              <button class="btn btn-secondary btn-sm" @click="showRate = true">Rate</button>
              <button class="btn btn-ghost btn-sm" @click="removeFromLibrary">Remove</button>
            </div>
          </template>
          <button class="btn btn-secondary" @click="showReviewModal = true">Write Review</button>
          <template v-if="authStore.isAdmin">
            <button class="btn btn-ghost" @click="showEdit = true">Edit</button>
          </template>
        </div>
      </div>
    </div>

    <!-- Characters -->
    <section class="detail-section">
      <h2>Characters</h2>
      <div v-if="characters.length" class="grid-4" style="margin-top:16px">
        <CharacterCard v-for="c in characters" :key="c.characterId" :character="c" />
      </div>
      <p v-else class="text-muted" style="margin-top:12px">No characters listed for this book.</p>
    </section>

    <!-- Reviews -->
    <section class="detail-section">
      <div class="flex items-center justify-between mb-4">
        <h2>Reviews</h2>
        <div v-if="avgRating" class="flex items-center gap-2">
          <StarRating :value="avgRating" :show-value="true" />
          <span class="text-muted" style="font-size:0.85rem">({{ reviews.length }})</span>
        </div>
      </div>
      <div v-if="reviews.length" style="display:flex;flex-direction:column;gap:12px">
        <div v-for="r in reviews" :key="r.bookReviewId" class="card review-card">
          <div class="review-header">
            <StarRating :value="r.rating" />
            <div class="flex gap-2" v-if="r.userId === authStore.userId">
              <button class="btn-icon" style="font-size:0.8rem" @click="editReview(r)">✎ Edit</button>
              <button class="btn-icon" style="font-size:0.8rem;color:var(--danger)" @click="deleteReview(r)">✕</button>
            </div>
          </div>
          <p style="color:var(--text);margin-top:8px">{{ r.reviewText }}</p>
          <span class="text-muted" style="font-size:0.78rem;margin-top:4px;display:block">{{ formatDate(r.createdAt) }}</span>
        </div>
      </div>
      <div v-else class="empty-state" style="padding:40px 0">
        <div class="empty-state-icon">✍️</div>
        <h3>No reviews yet</h3>
        <p>Be the first to review this book</p>
      </div>
    </section>

    <!-- Modals -->
    <BookReviewModal :show="showReviewModal" :review="editingReview" :loading="reviewLoading" @close="closeReview" @submit="submitReview" />
    <BookFormModal :show="showEdit" :book="book" :loading="editLoading" @close="showEdit=false" @submit="handleEdit" />
    <Teleport to="body">
      <div v-if="showRate" class="modal-overlay" @click.self="showRate=false">
        <div class="modal" style="max-width:320px">
          <div class="modal-header"><h3>Rate this Book</h3><button class="btn-icon" @click="showRate=false">✕</button></div>
          <div style="display:flex;flex-direction:column;align-items:center;gap:16px;padding:16px 0">
            <StarInput v-model="rateValue" />
            <span class="text-muted">{{ rateValue ? `${rateValue} / 5` : 'Pick a rating' }}</span>
          </div>
          <div class="modal-footer">
            <button class="btn btn-secondary" @click="showRate=false">Cancel</button>
            <button class="btn btn-primary" :disabled="!rateValue" @click="submitRate">Save Rating</button>
          </div>
        </div>
      </div>
    </Teleport>
  </div>

  <div v-else-if="notFound" class="empty-state" style="padding:80px">
    <div class="empty-state-icon">🔍</div>
    <h3>Book not found</h3>
    <RouterLink to="/books" class="btn btn-secondary" style="margin-top:16px;display:inline-flex">← Back</RouterLink>
  </div>

  <div v-else class="empty-state" style="padding:80px">
    <div class="skeleton" style="height:40px;width:300px;margin:0 auto 16px" />
    <div class="skeleton" style="height:20px;width:200px;margin:0 auto" />
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { booksApi } from '@/api/books'
import { bookReviewsApi, charactersApi, userBooksApi } from '@/api/index'
import { useToast } from '@/composables/useToast'
import StarRating from '@/components/common/StarRating.vue'
import StarInput from '@/components/common/StarInput.vue'
import CharacterCard from '@/components/characters/CharacterCard.vue'
import BookReviewModal from '@/components/reviews/BookReviewModal.vue'
import BookFormModal from '@/components/books/BookFormModal.vue'

const route = useRoute()
const authStore = useAuthStore()
const toast = useToast()

const book = ref(null)
const notFound = ref(false)
const imgError = ref(false)
const characters = ref([])
const reviews = ref([])
const userBook = ref(null)
const libraryLoading = ref(false)
const statusValue = ref('WantToRead')
const showRate = ref(false)
const rateValue = ref(0)

const showReviewModal = ref(false)
const editingReview = ref(null)
const reviewLoading = ref(false)
const showEdit = ref(false)
const editLoading = ref(false)

const coverUrl = computed(() => {
  if (imgError.value) return book.value?.coverUrl || null
  if (book.value?.openLibraryId) return `https://covers.openlibrary.org/b/olid/${book.value.openLibraryId}-L.jpg`
  return book.value?.coverUrl || null
})
const pubYear = computed(() => book.value?.publishedDate ? new Date(book.value.publishedDate).getFullYear() : '—')
const inLibrary = computed(() => !!userBook.value)
const avgRating = computed(() => {
  if (!reviews.value.length) return 0
  return reviews.value.reduce((a, b) => a + b.rating, 0) / reviews.value.length
})

function formatDate(d) { return new Date(d).toLocaleDateString('en-US', { year:'numeric', month:'short', day:'numeric' }) }

async function fetchAll() {
  const id = route.params.id
  try {
    const [b, c, r] = await Promise.all([
      booksApi.getById(id),
      charactersApi.getByBook(id, { pageSize: 20 }).catch(() => ({ data: { items: [] } })),
      bookReviewsApi.getByBook(id).catch(() => ({ data: [] })),
    ])
    book.value = b.data
    characters.value = c.data.items || []
    reviews.value = Array.isArray(r.data) ? r.data : []
  } catch { notFound.value = true }

  try {
    const my = await userBooksApi.getMy({ pageSize: 200 })
    const found = (my.data.items || []).find(ub => ub.bookId === parseInt(id))
    if (found) { userBook.value = found; statusValue.value = found.status; rateValue.value = found.userRating || 0 }
  } catch {}
}

async function addToLibrary() {
  libraryLoading.value = true
  try {
    const { data } = await userBooksApi.add({ bookId: book.value.id })
    userBook.value = data; statusValue.value = data.status; toast.success('Added to library!')
  } catch (e) { toast.error(e.response?.data || 'Failed to add') }
  finally { libraryLoading.value = false }
}

async function updateStatus() {
  try {
    const { data } = await userBooksApi.updateStatus(book.value.id, { status: statusValue.value })
    userBook.value = data; toast.success('Status updated')
  } catch { toast.error('Failed to update status') }
}

async function submitRate() {
  try {
    const { data } = await userBooksApi.rate(book.value.id, { userRating: rateValue.value })
    userBook.value = data; showRate.value = false; toast.success('Rating saved!')
  } catch { toast.error('Failed to save rating') }
}

async function removeFromLibrary() {
  try {
    await userBooksApi.remove(book.value.id)
    userBook.value = null; toast.success('Removed from library')
  } catch { toast.error('Failed to remove') }
}

function closeReview() { showReviewModal.value = false; editingReview.value = null }
function editReview(r) { editingReview.value = r; showReviewModal.value = true }

async function submitReview(formData) {
  reviewLoading.value = true
  try {
    if (editingReview.value) {
      await bookReviewsApi.update(editingReview.value.bookReviewId, formData)
      toast.success('Review updated')
    } else {
      await bookReviewsApi.create({ ...formData, bookId: book.value.id })
      toast.success('Review submitted!')
    }
    closeReview()
    const r = await bookReviewsApi.getByBook(book.value.id).catch(() => ({ data: [] }))
    reviews.value = Array.isArray(r.data) ? r.data : []
  } catch (e) { toast.error(e.response?.data || 'Failed to submit review') }
  finally { reviewLoading.value = false }
}

async function deleteReview(r) {
  try {
    await bookReviewsApi.delete(r.bookReviewId)
    reviews.value = reviews.value.filter(x => x.bookReviewId !== r.bookReviewId)
    toast.success('Review deleted')
  } catch { toast.error('Failed to delete review') }
}

async function handleEdit(formData) {
  editLoading.value = true
  try {
    const { data } = await booksApi.update(book.value.id, formData)
    book.value = data; showEdit.value = false; toast.success('Book updated')
  } catch { toast.error('Failed to update') }
  finally { editLoading.value = false }
}

onMounted(fetchAll)
</script>

<style scoped>
.book-hero { display: flex; gap: 40px; margin-bottom: 48px; }
.book-cover-large { flex-shrink: 0; width: 200px; border-radius: var(--radius-2); overflow: hidden; border: 1px solid var(--border); box-shadow: var(--shadow-2); }
.book-cover-large img { width: 100%; display: block; }
.cover-fallback { height: 280px; display: flex; align-items: center; justify-content: center; font-size: 3rem; background: var(--bg-3); }
.back-link { font-size: 0.85rem; color: var(--text-3); text-decoration: none; display: block; margin-bottom: 12px; }
.back-link:hover { color: var(--amber); }
.hero-author { color: var(--text-2); font-style: italic; font-size: 1.05rem; margin: 8px 0; }
.hero-desc { color: var(--text-2); max-width: 600px; line-height: 1.6; margin: 12px 0; }
.hero-meta { display: flex; gap: 8px; flex-wrap: wrap; margin-bottom: 16px; }
.hero-actions { display: flex; gap: 10px; flex-wrap: wrap; align-items: center; }
.library-status { display: flex; align-items: center; gap: 10px; flex-wrap: wrap; }
.detail-section { margin-bottom: 48px; }
.detail-section h2 { margin-bottom: 4px; }
.review-card { padding: 16px; }
.review-header { display: flex; align-items: center; justify-content: space-between; }
@media (max-width: 700px) { .book-hero { flex-direction: column; } .book-cover-large { width: 140px; } }
</style>