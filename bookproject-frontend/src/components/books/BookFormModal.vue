<template>
  <Teleport to="body">
    <div v-if="show" class="modal-overlay" @click.self="$emit('close')">
      <div class="modal modal-lg">
        <div class="modal-header">
          <h3>{{ book ? 'Edit Book' : 'Add Book' }}</h3>
          <button class="btn-icon" @click="$emit('close')">✕</button>
        </div>

        <div style="display:flex;flex-direction:column;gap:16px">
          <div class="form-group">
            <label class="form-label">Title *</label>
            <input v-model="form.title" class="form-input" placeholder="Book title" />
            <span v-if="errors.title" class="form-error">{{ errors.title }}</span>
          </div>
          <div class="form-group">
            <label class="form-label">Author *</label>
            <input v-model="form.author" class="form-input" placeholder="Author name" />
            <span v-if="errors.author" class="form-error">{{ errors.author }}</span>
          </div>
          <div class="form-group">
            <label class="form-label">Description *</label>
            <textarea v-model="form.description" class="form-textarea" placeholder="Book description" style="min-height:120px" />
            <span v-if="errors.description" class="form-error">{{ errors.description }}</span>
          </div>
          <div class="grid-2">
            <div class="form-group">
              <label class="form-label">OpenLibrary ID</label>
              <input v-model="form.openLibraryId" class="form-input" placeholder="e.g. OL7353617M" />
              <span class="form-hint">Used to fetch book covers automatically</span>
            </div>
            <div class="form-group">
              <label class="form-label">Published Date</label>
              <input v-model="form.publishedDate" type="date" class="form-input" />
            </div>
          </div>
          <div class="form-group">
            <label class="form-label">Manual Cover URL</label>
            <input v-model="form.coverUrl" class="form-input" placeholder="https://… (fallback if OpenLibrary has no cover)" />
            <span class="form-hint">Optional — used when OpenLibrary cover is unavailable</span>
          </div>
        </div>

        <div class="modal-footer">
          <button class="btn btn-secondary" @click="$emit('close')">Cancel</button>
          <button class="btn btn-primary" :disabled="loading" @click="submit">
            {{ loading ? 'Saving…' : (book ? 'Save Changes' : 'Add Book') }}
          </button>
        </div>
      </div>
    </div>
  </Teleport>
</template>

<script setup>
import { ref, watch } from 'vue'

const props = defineProps({
  show:    { type: Boolean, default: false },
  book:    { type: Object, default: null },
  loading: { type: Boolean, default: false },
})
const emit = defineEmits(['close', 'submit'])

const form = ref({ title: '', author: '', description: '', openLibraryId: '', publishedDate: '', coverUrl: '' })
const errors = ref({})

watch(() => props.show, (v) => {
  if (v) {
    errors.value = {}
    if (props.book) {
      form.value = {
        title: props.book.title,
        author: props.book.author,
        description: props.book.description,
        openLibraryId: props.book.openLibraryId,
        publishedDate: props.book.publishedDate ? props.book.publishedDate.slice(0, 10) : '',
        coverUrl: props.book.coverUrl || '',
      }
    } else {
      form.value = { title: '', author: '', description: '', openLibraryId: '', publishedDate: '', coverUrl: '' }
    }
  }
})

function validate() {
  errors.value = {}
  if (!form.value.title?.trim()) errors.value.title = 'Title is required'
  if (!form.value.author?.trim()) errors.value.author = 'Author is required'
  if (!form.value.description?.trim() || form.value.description.length < 5) errors.value.description = 'Description must be at least 5 characters'
  return Object.keys(errors.value).length === 0
}

function submit() {
  if (!validate()) return
  emit('submit', { ...form.value })
}
</script>