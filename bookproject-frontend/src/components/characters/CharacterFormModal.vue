<template>
  <Teleport to="body">
    <div v-if="show" class="modal-overlay" @click.self="$emit('close')">
      <div class="modal modal-lg">
        <div class="modal-header">
          <h3>{{ character ? 'Edit Character' : 'Add Character' }}</h3>
          <button class="btn-icon" @click="$emit('close')">✕</button>
        </div>

        <div style="display:flex;flex-direction:column;gap:16px">
          <div class="form-group">
            <label class="form-label">Name *</label>
            <input v-model="form.name" class="form-input" placeholder="Character name" />
            <span v-if="errors.name" class="form-error">{{ errors.name }}</span>
          </div>
          <div class="form-group">
            <label class="form-label">Description *</label>
            <textarea v-model="form.description" class="form-textarea" placeholder="Detailed character description (used for AI portraits)" style="min-height:130px" />
            <span class="form-hint">More detail = better AI portraits later</span>
            <span v-if="errors.description" class="form-error">{{ errors.description }}</span>
          </div>
          <div class="grid-2">
            <div class="form-group">
              <label class="form-label">Book *</label>
              <select v-model="form.bookId" class="form-select">
                <option value="">Select a book…</option>
                <option v-for="b in books" :key="b.id" :value="b.id">{{ b.title }}</option>
              </select>
              <span v-if="errors.bookId" class="form-error">{{ errors.bookId }}</span>
            </div>
            <div class="form-group">
              <label class="form-label">Verified</label>
              <div style="display:flex;align-items:center;gap:10px;padding:10px 0">
                <input type="checkbox" v-model="form.verified" id="verified" style="accent-color:var(--amber);width:16px;height:16px" />
                <label for="verified" style="color:var(--text-2);cursor:pointer">Mark as verified character</label>
              </div>
            </div>
          </div>
        </div>

        <div class="modal-footer">
          <button class="btn btn-secondary" @click="$emit('close')">Cancel</button>
          <button class="btn btn-primary" :disabled="loading" @click="submit">
            {{ loading ? 'Saving…' : (character ? 'Save Changes' : 'Add Character') }}
          </button>
        </div>
      </div>
    </div>
  </Teleport>
</template>

<script setup>
import { ref, watch } from 'vue'

const props = defineProps({
  show: { type: Boolean, default: false },
  character: { type: Object, default: null },
  books: { type: Array, default: () => [] },
  loading: { type: Boolean, default: false },
})
const emit = defineEmits(['close', 'submit'])
const form = ref({ name: '', description: '', bookId: '', verified: false })
const errors = ref({})

watch(() => props.show, (v) => {
  if (v) {
    errors.value = {}
    if (props.character) {
      form.value = { name: props.character.name, description: props.character.description, bookId: props.character.bookId, verified: props.character.verified }
    } else {
      form.value = { name: '', description: '', bookId: '', verified: false }
    }
  }
})

function validate() {
  errors.value = {}
  if (!form.value.name?.trim()) errors.value.name = 'Name is required'
  if (!form.value.description?.trim() || form.value.description.length < 5) errors.value.description = 'Description must be at least 5 characters'
  if (!form.value.bookId) errors.value.bookId = 'Please select a book'
  return Object.keys(errors.value).length === 0
}

function submit() {
  if (!validate()) return
  emit('submit', { name: form.value.name, description: form.value.description, bookId: Number(form.value.bookId), verified: form.value.verified })
}
</script>
