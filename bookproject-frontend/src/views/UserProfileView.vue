<template>
  <div v-if="user">
    <!-- Header -->
    <RouterLink to="/users" class="back-link">← Back to Users</RouterLink>

    <div class="profile-hero">
      <div class="profile-avatar">{{ initial }}</div>
      <div class="profile-hero-info">
        <h1>{{ user.username }}</h1>
        <p class="text-muted">User #{{ user.userId }}</p>
        <div class="profile-stats">
          <div class="stat-pill">
            <span class="stat-num">{{ readBooks.length }}</span>
            <span class="stat-lbl">Books Read</span>
          </div>
          <div class="stat-pill">
            <span class="stat-num">{{ bookReviews.length }}</span>
            <span class="stat-lbl">Book Reviews</span>
          </div>
          <div class="stat-pill">
            <span class="stat-num">{{ charReviews.length }}</span>
            <span class="stat-lbl">Character Reviews</span>
          </div>
          <div class="stat-pill" v-if="avgRating">
            <span class="stat-num text-amber">{{ avgRating }}</span>
            <span class="stat-lbl">Avg Rating</span>
          </div>
        </div>
      </div>
    </div>

    <!-- Tabs -->
    <div class="profile-tabs">
      <button v-for="tab in tabs" :key="tab.value" :class="['tab-btn', { active: activeTab === tab.value }]" @click="activeTab = tab.value">
        {{ tab.label }}
      </button>
    </div>

    <!-- Books tab -->
    <div v-if="activeTab === 'books'">
      <div v-if="userBooksLoading" class="grid-4" style="margin-top:20px">
        <div v-for="i in 4" :key="i" class="skeleton" style="height:200px;border-radius:12px" />
      </div>
      <div v-else-if="userBooks.length" style="margin-top:20px">
        <!-- Status groups -->
        <div v-for="group in bookGroups" :key="group.status" style="margin-bottom:32px">
          <h3 style="margin-bottom:14px;display:flex;align-items:center;gap:10px">
            {{ group.label }}
            <span class="badge badge-gray">{{ group.items.length }}</span>
          </h3>
          <div class="grid-4">
            <RouterLink
              v-for="ub in group.items"
              :key="ub.bookId"
              :to="`/books/${ub.bookId}`"
              class="card card-hover user-book-card"
            >
              <div class="ub-cover">
                <img
                  v-if="bookMap[ub.bookId]?.openLibraryId && !coverErrors[ub.bookId]"
                  :src="`https://covers.openlibrary.org/b/olid/${bookMap[ub.bookId].openLibraryId}-M.jpg`"
                  @error="coverErrors[ub.bookId] = true"
                />
                <div v-else class="ub-cover-fallback">📖</div>
              </div>
              <div class="ub-info">
                <p class="ub-title">{{ bookMap[ub.bookId]?.title || `Book #${ub.bookId}` }}</p>
                <p class="ub-author">{{ bookMap[ub.bookId]?.author }}</p>
                <StarRating v-if="ub.userRating" :value="ub.userRating" style="margin-top:4px" />
              </div>
            </RouterLink>
          </div>
        </div>
      </div>
      <div v-else class="empty-state" style="padding:40px 0">
        <div class="empty-state-icon">📚</div>
        <h3>No books in library yet</h3>
      </div>
    </div>

    <!-- Book reviews tab -->
    <div v-if="activeTab === 'bookreviews'">
      <div v-if="reviewsLoading" style="margin-top:20px;display:flex;flex-direction:column;gap:12px">
        <div v-for="i in 3" :key="i" class="skeleton" style="height:100px;border-radius:12px" />
      </div>
      <div v-else-if="bookReviews.length" style="margin-top:20px;display:flex;flex-direction:column;gap:12px">
        <div v-for="r in bookReviews" :key="r.bookReviewId" class="card review-item">
          <div class="review-item-header">
            <RouterLink :to="`/books/${r.bookId}`" class="review-book-link">
              {{ bookMap[r.bookId]?.title || `Book #${r.bookId}` }}
            </RouterLink>
            <StarRating :value="r.rating" :show-value="true" />
          </div>
          <p style="color:var(--text);margin-top:8px">{{ r.reviewText }}</p>
          <span class="text-muted" style="font-size:0.78rem;margin-top:6px;display:block">{{ formatDate(r.createdAt) }}</span>
        </div>
      </div>
      <div v-else class="empty-state" style="padding:40px 0">
        <div class="empty-state-icon">✍️</div>
        <h3>No book reviews yet</h3>
      </div>
    </div>

    <!-- Character reviews tab -->
    <div v-if="activeTab === 'charreviews'">
      <div v-if="charReviewsLoading" style="margin-top:20px;display:flex;flex-direction:column;gap:12px">
        <div v-for="i in 3" :key="i" class="skeleton" style="height:120px;border-radius:12px" />
      </div>
      <div v-else-if="charReviews.length" style="margin-top:20px;display:flex;flex-direction:column;gap:12px">
        <div v-for="r in charReviews" :key="r.characterReviewId" class="card review-item">
          <div class="review-item-header">
            <RouterLink :to="`/characters/${r.characterId}`" class="review-book-link">
              Character #{{ r.characterId }}
            </RouterLink>
            <StarRating :value="r.overallRating" :show-value="true" />
          </div>
          <div class="sub-ratings" style="margin-top:10px">
            <div v-for="f in ratingFields" :key="f.key" class="sub-rating">
              <span class="sub-label">{{ f.label }}</span>
              <StarRating :value="r[f.key]" />
            </div>
          </div>
          <p style="color:var(--text);margin-top:10px">{{ r.reviewText }}</p>
          <span class="text-muted" style="font-size:0.78rem;margin-top:6px;display:block">{{ formatDate(r.createdAt) }}</span>
        </div>
      </div>
      <div v-else class="empty-state" style="padding:40px 0">
        <div class="empty-state-icon">👤</div>
        <h3>No character reviews yet</h3>
      </div>
    </div>
  </div>

  <div v-else-if="notFound" class="empty-state" style="padding:80px">
    <div class="empty-state-icon">🔍</div>
    <h3>User not found</h3>
    <RouterLink to="/users" class="btn btn-secondary" style="margin-top:16px;display:inline-flex">← Back</RouterLink>
  </div>

  <div v-else style="padding:40px 0">
    <div class="skeleton" style="height:120px;border-radius:12px;margin-bottom:24px" />
    <div class="skeleton" style="height:40px;border-radius:8px" />
  </div>
</template>

<script setup>
import { ref, computed, onMounted, watch } from 'vue'
import { useRoute } from 'vue-router'
import { usersApi } from '@/api/users'
import { userBooksApi, bookReviewsApi, characterReviewsApi } from '@/api/index'
import { booksApi } from '@/api/books'
import StarRating from '@/components/common/StarRating.vue'

const route = useRoute()

const user = ref(null)
const notFound = ref(false)
const activeTab = ref('books')

const userBooks = ref([])
const userBooksLoading = ref(false)
const bookMap = ref({})
const coverErrors = ref({})

const bookReviews = ref([])
const reviewsLoading = ref(false)

const charReviews = ref([])
const charReviewsLoading = ref(false)

const tabs = [
  { label: 'Books', value: 'books' },
  { label: 'Book Reviews', value: 'bookreviews' },
  { label: 'Character Reviews', value: 'charreviews' },
]

const ratingFields = [
  { key: 'depthComplexity', label: 'Depth' },
  { key: 'characterDevelopment', label: 'Development' },
  { key: 'consistencyBelievability', label: 'Consistency' },
  { key: 'impactOnStory', label: 'Impact' },
  { key: 'memorability', label: 'Memorability' },
]

const initial = computed(() => user.value?.username?.charAt(0)?.toUpperCase() || '?')

const readBooks = computed(() => userBooks.value.filter(ub => ub.status === 'Read'))

const avgRating = computed(() => {
  const rated = bookReviews.value.filter(r => r.rating)
  if (!rated.length) return null
  return (rated.reduce((a, b) => a + b.rating, 0) / rated.length).toFixed(1)
})

const bookGroups = computed(() => [
  { status: 'Read', label: 'Read', items: userBooks.value.filter(ub => ub.status === 'Read') },
  { status: 'Reading', label: 'Currently Reading', items: userBooks.value.filter(ub => ub.status === 'Reading') },
  { status: 'WantToRead', label: 'Want to Read', items: userBooks.value.filter(ub => ub.status === 'WantToRead') },
  { status: 'DNF', label: 'Did Not Finish', items: userBooks.value.filter(ub => ub.status === 'DNF') },
].filter(g => g.items.length))

function formatDate(d) {
  return new Date(d).toLocaleDateString('en-US', { year: 'numeric', month: 'short', day: 'numeric' })
}

async function fetchUser() {
  try {
    const { data } = await usersApi.getById(route.params.id)
    user.value = data
  } catch {
    notFound.value = true
  }
}

async function fetchUserBooks() {
  userBooksLoading.value = true
  try {
    const { data } = await userBooksApi.getByUser(route.params.id, { pageSize: 200 })
    userBooks.value = data.items || []

    // Fetch book details for all
    const ids = [...new Set(userBooks.value.map(ub => ub.bookId))]
    const results = await Promise.allSettled(ids.map(id => booksApi.getById(id)))
    results.forEach((r, i) => {
      if (r.status === 'fulfilled') bookMap.value[ids[i]] = r.value.data
    })
  } catch {}
  finally { userBooksLoading.value = false }
}

async function fetchBookReviews() {
  reviewsLoading.value = true
  try {
    const { data } = await bookReviewsApi.getAll({ userId: route.params.id, pageSize: 100 })
    bookReviews.value = data.items || []

    // Fetch book titles we don't have yet
    const ids = [...new Set(bookReviews.value.map(r => r.bookId))].filter(id => !bookMap.value[id])
    const results = await Promise.allSettled(ids.map(id => booksApi.getById(id)))
    results.forEach((r, i) => {
      if (r.status === 'fulfilled') bookMap.value[ids[i]] = r.value.data
    })
  } catch {}
  finally { reviewsLoading.value = false }
}

async function fetchCharReviews() {
  charReviewsLoading.value = true
  try {
    const { data } = await characterReviewsApi.getAll({ userId: route.params.id, pageSize: 100 })
    charReviews.value = data.items || []
  } catch {}
  finally { charReviewsLoading.value = false }
}

// Lazy load tabs
watch(activeTab, (tab) => {
  if (tab === 'bookreviews' && !bookReviews.value.length) fetchBookReviews()
  if (tab === 'charreviews' && !charReviews.value.length) fetchCharReviews()
})

onMounted(async () => {
  await fetchUser()
  fetchUserBooks()
})
</script>

<style scoped>
.back-link { font-size: 0.85rem; color: var(--text-3); text-decoration: none; display: block; margin-bottom: 20px; }
.back-link:hover { color: var(--amber); }

.profile-hero { display: flex; align-items: flex-start; gap: 24px; margin-bottom: 32px; }
.profile-avatar {
  width: 72px; height: 72px; border-radius: 50%; flex-shrink: 0;
  background: var(--amber-dim); border: 2px solid rgba(200,146,58,0.3);
  color: var(--amber-2); font-family: var(--font-display); font-weight: 700; font-size: 2rem;
  display: flex; align-items: center; justify-content: center;
}
.profile-hero-info h1 { margin-bottom: 4px; }
.profile-stats { display: flex; gap: 12px; flex-wrap: wrap; margin-top: 16px; }
.stat-pill {
  display: flex; flex-direction: column; align-items: center;
  padding: 10px 18px; background: var(--bg-3); border: 1px solid var(--border);
  border-radius: var(--radius-2); min-width: 80px;
}
.stat-num { font-family: var(--font-display); font-weight: 700; font-size: 1.4rem; color: var(--text); }
.stat-lbl { font-size: 0.72rem; color: var(--text-3); text-transform: uppercase; letter-spacing: 0.05em; margin-top: 2px; }

.profile-tabs { display: flex; gap: 6px; border-bottom: 1px solid var(--border); margin-bottom: 0; }
.tab-btn { background: none; border: none; border-bottom: 2px solid transparent; padding: 10px 16px; color: var(--text-3); cursor: pointer; font-family: var(--font-body); font-size: 0.95rem; transition: all var(--transition); margin-bottom: -1px; }
.tab-btn:hover { color: var(--text); }
.tab-btn.active { color: var(--amber); border-bottom-color: var(--amber); }

.user-book-card { padding: 0; overflow: hidden; text-decoration: none; color: inherit; }
.ub-cover { height: 140px; background: var(--bg-3); overflow: hidden; }
.ub-cover img { width: 100%; height: 100%; object-fit: cover; }
.ub-cover-fallback { height: 100%; display: flex; align-items: center; justify-content: center; font-size: 2rem; opacity: 0.3; }
.ub-info { padding: 10px 12px 12px; }
.ub-title { font-family: var(--font-display); font-size: 0.9rem; color: var(--text); line-height: 1.3; }
.ub-author { font-size: 0.78rem; color: var(--text-3); font-style: italic; margin-top: 3px; }

.review-item { padding: 16px; }
.review-item-header { display: flex; align-items: center; justify-content: space-between; gap: 12px; flex-wrap: wrap; }
.review-book-link { font-family: var(--font-display); font-size: 1rem; color: var(--amber); text-decoration: none; }
.review-book-link:hover { color: var(--amber-2); }

.sub-ratings { display: flex; flex-wrap: wrap; gap: 12px; }
.sub-rating { display: flex; flex-direction: column; gap: 2px; }
.sub-label { font-size: 0.7rem; color: var(--text-3); text-transform: uppercase; letter-spacing: 0.05em; font-weight: 600; }
</style>
