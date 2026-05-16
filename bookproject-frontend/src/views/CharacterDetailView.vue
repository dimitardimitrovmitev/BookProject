<template>
  <div v-if="character">
    <RouterLink to="/characters" class="back-link">← Back to Characters</RouterLink>

    <div class="char-hero">
      <div class="char-big-avatar">{{ initial }}</div>
      <div class="char-hero-info">
        <div class="flex items-center gap-3" style="flex-wrap:wrap">
          <h1>{{ character.name }}</h1>
          <span v-if="character.verified" class="badge badge-green">✓ Verified</span>
          <span v-else class="badge badge-gray">Unverified</span>
        </div>
        <p v-if="character.bookTitle" style="color:var(--text-3);margin:6px 0">
          From <RouterLink :to="`/books/${character.bookId}`" class="text-amber" style="font-style:italic">{{ character.bookTitle }}</RouterLink>
        </p>
        <p class="char-desc">{{ character.description }}</p>
        <div class="flex gap-2" style="margin-top:16px">
          <button class="btn btn-primary" @click="showReviewModal = true">Write Character Review</button>
          <template v-if="authStore.isAdmin">
            <button class="btn btn-ghost" @click="showEdit = true">Edit</button>
          </template>
        </div>
      </div>
    </div>

    <!-- Reviews -->
    <section style="margin-top:48px">
      <div class="flex items-center justify-between mb-4">
        <h2>Character Reviews</h2>
        <div v-if="avgOverall" class="flex items-center gap-2">
          <StarRating :value="avgOverall" :show-value="true" />
          <span class="text-muted" style="font-size:0.85rem">({{ reviews.length }})</span>
        </div>
      </div>

      <div v-if="reviews.length" style="display:flex;flex-direction:column;gap:16px">
        <div v-for="r in reviews" :key="r.characterReviewId" class="card review-card">
          <div class="review-header">
            <div class="sub-ratings">
              <div v-for="field in ratingFields" :key="field.key" class="sub-rating">
                <span class="sub-label">{{ field.label }}</span>
                <StarRating :value="r[field.key]" />
              </div>
            </div>
            <div class="review-overall">
              <span class="form-label">Overall</span>
              <StarRating :value="r.overallRating" :show-value="true" />
            </div>
          </div>
          <p style="color:var(--text);margin-top:12px">{{ r.reviewText }}</p>
          <div class="review-footer">
            <span class="text-muted" style="font-size:0.78rem">{{ formatDate(r.createdAt) }}</span>
            <div v-if="r.userId === authStore.userId" class="flex gap-2">
              <button class="btn-icon" style="font-size:0.8rem" @click="editReview(r)">✎</button>
              <button class="btn-icon" style="font-size:0.8rem;color:var(--danger)" @click="deleteReview(r)">✕</button>
            </div>
          </div>
          <div v-if="r.aiImageUrl" style="margin-top:12px">
            <img :src="r.aiImageUrl" alt="AI Portrait" style="border-radius:var(--radius);max-height:200px;border:1px solid var(--border)" />
          </div>
        </div>
      </div>

      <div v-else class="empty-state" style="padding:40px 0">
        <div class="empty-state-icon">✍️</div>
        <h3>No reviews yet</h3>
        <p>Be the first to review {{ character.name }}</p>
      </div>
    </section>

    <CharacterReviewModal :show="showReviewModal" :review="editingReview" :loading="reviewLoading" @close="closeReview" @submit="submitReview" />
    <CharacterFormModal :show="showEdit" :character="character" :books="books" :loading="editLoading" @close="showEdit=false" @submit="handleEdit" />
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { charactersApi, characterReviewsApi } from '@/api/index'
import { booksApi } from '@/api/books'
import { useToast } from '@/composables/useToast'
import StarRating from '@/components/common/StarRating.vue'
import CharacterReviewModal from '@/components/reviews/CharacterReviewModal.vue'
import CharacterFormModal from '@/components/characters/CharacterFormModal.vue'

const route = useRoute()
const authStore = useAuthStore()
const toast = useToast()

const character = ref(null)
const reviews = ref([])
const books = ref([])
const showReviewModal = ref(false)
const editingReview = ref(null)
const reviewLoading = ref(false)
const showEdit = ref(false)
const editLoading = ref(false)

const ratingFields = [
  { key: 'depthComplexity', label: 'Depth' },
  { key: 'characterDevelopment', label: 'Development' },
  { key: 'consistencyBelievability', label: 'Consistency' },
  { key: 'impactOnStory', label: 'Impact' },
  { key: 'memorability', label: 'Memorability' },
]

const initial = computed(() => character.value?.name?.charAt(0)?.toUpperCase() || '?')
const avgOverall = computed(() => {
  if (!reviews.value.length) return 0
  return reviews.value.reduce((a, b) => a + b.overallRating, 0) / reviews.value.length
})

function formatDate(d) { return new Date(d).toLocaleDateString('en-US', { year:'numeric', month:'short', day:'numeric' }) }

async function fetchAll() {
  const id = route.params.id
  try {
    const [c, r] = await Promise.all([
      charactersApi.getById(id),
      characterReviewsApi.getAll({ characterId: id, pageSize: 100 }).catch(() => ({ data: { items: [] } })),
    ])
    character.value = c.data
    reviews.value = r.data.items || []
  } catch { toast.error('Failed to load character') }

  try {
    const { data } = await booksApi.getAll({ pageSize: 200 })
    books.value = data.items
  } catch {}
}

function closeReview() { showReviewModal.value = false; editingReview.value = null }
function editReview(r) { editingReview.value = r; showReviewModal.value = true }

async function submitReview(formData) {
  reviewLoading.value = true
  try {
    if (editingReview.value) {
      await characterReviewsApi.update(editingReview.value.characterReviewId, formData)
      toast.success('Review updated')
    } else {
      await characterReviewsApi.create({ ...formData, characterId: character.value.characterId })
      toast.success('Review submitted!')
    }
    closeReview()
    const r = await characterReviewsApi.getAll({ characterId: route.params.id, pageSize: 100 }).catch(() => ({ data: { items: [] } }))
    reviews.value = r.data.items || []
  } catch (e) { toast.error(e.response?.data || 'Failed to submit') }
  finally { reviewLoading.value = false }
}

async function deleteReview(r) {
  try {
    await characterReviewsApi.delete(r.characterReviewId)
    reviews.value = reviews.value.filter(x => x.characterReviewId !== r.characterReviewId)
    toast.success('Review deleted')
  } catch { toast.error('Failed to delete') }
}

async function handleEdit(formData) {
  editLoading.value = true
  try {
    const { data } = await charactersApi.update(character.value.characterId, formData)
    character.value = data; showEdit.value = false; toast.success('Updated')
  } catch { toast.error('Failed to update') }
  finally { editLoading.value = false }
}

onMounted(fetchAll)
</script>

<style scoped>
.back-link { font-size:0.85rem;color:var(--text-3);text-decoration:none;display:block;margin-bottom:20px; }
.back-link:hover { color:var(--amber); }
.char-hero { display:flex;gap:32px;margin-bottom:16px;align-items:flex-start; }
.char-big-avatar { flex-shrink:0;width:80px;height:80px;border-radius:50%;background:var(--amber-dim);border:2px solid rgba(200,146,58,0.3);color:var(--amber-2);font-family:var(--font-display);font-weight:700;font-size:2rem;display:flex;align-items:center;justify-content:center; }
.char-desc { color:var(--text-2);line-height:1.65;max-width:700px;margin-top:10px; }
.review-card { padding:18px; }
.review-header { display:flex;justify-content:space-between;align-items:flex-start;gap:16px;flex-wrap:wrap; }
.sub-ratings { display:flex;flex-wrap:wrap;gap:12px; }
.sub-rating { display:flex;flex-direction:column;gap:3px; }
.sub-label { font-size:0.72rem;color:var(--text-3);text-transform:uppercase;letter-spacing:0.05em;font-weight:600; }
.review-overall { display:flex;flex-direction:column;gap:3px; }
.review-footer { display:flex;align-items:center;justify-content:space-between;margin-top:10px; }
</style>
