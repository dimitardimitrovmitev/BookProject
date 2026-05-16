<template>
  <RouterLink :to="`/books/${book.id}`" class="book-card card card-hover">
    <div class="book-cover">
      <img
        v-if="coverUrl"
        :src="coverUrl"
        :alt="book.title"
        @error="imgError = true"
      />
      <div v-else class="cover-placeholder">
        <span class="cover-icon">📖</span>
        <span class="cover-title">{{ book.title }}</span>
      </div>
    </div>
    <div class="book-info">
      <h3 class="book-title">{{ book.title }}</h3>
      <p class="book-author">{{ book.author }}</p>
      <p class="book-desc">{{ truncatedDesc }}</p>
      <div class="book-meta">
        <span class="badge badge-gray">{{ pubYear }}</span>
      </div>
    </div>
  </RouterLink>
</template>

<script setup>
import { computed, ref } from 'vue'

const props = defineProps({ book: { type: Object, required: true } })
const imgError = ref(false)

const coverUrl = computed(() => {
  if (imgError.value) return props.book.coverUrl || null
  if (props.book.openLibraryId) return `https://covers.openlibrary.org/b/olid/${props.book.openLibraryId}-M.jpg`
  return props.book.coverUrl || null
})

const truncatedDesc = computed(() => {
  const d = props.book.description || ''
  return d.length > 100 ? d.slice(0, 100) + '…' : d
})

const pubYear = computed(() =>
  props.book.publishedDate ? new Date(props.book.publishedDate).getFullYear() : '—'
)
</script>

<style scoped>
.book-card {
  display: flex; flex-direction: column;
  text-decoration: none; color: inherit;
  overflow: hidden; padding: 0;
}
.book-cover {
  height: 180px; background: var(--bg-3); overflow: hidden;
  display: flex; align-items: center; justify-content: center;
  border-bottom: 1px solid var(--border);
}
.book-cover img { width: 100%; height: 100%; object-fit: cover; transition: transform 0.3s; }
.book-card:hover .book-cover img { transform: scale(1.04); }
.cover-placeholder { display: flex; flex-direction: column; align-items: center; gap: 8px; padding: 20px; text-align: center; }
.cover-icon { font-size: 2rem; opacity: 0.4; }
.cover-title { font-family: var(--font-display); font-style: italic; color: var(--text-3); font-size: 0.85rem; }

.book-info { padding: 16px; display: flex; flex-direction: column; gap: 6px; flex: 1; }
.book-title { font-family: var(--font-display); font-size: 1rem; color: var(--text); transition: color var(--transition); line-height: 1.3; }
.book-card:hover .book-title { color: var(--amber); }
.book-author { font-size: 0.85rem; color: var(--amber); font-style: italic; }
.book-desc { font-size: 0.85rem; color: var(--text-3); line-height: 1.4; flex: 1; }
.book-meta { margin-top: 4px; }
</style>