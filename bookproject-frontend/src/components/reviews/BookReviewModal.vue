<template>
  <Teleport to="body">
    <div v-if="show" class="modal-overlay" @click.self="$emit('close')">
      <div class="modal">
        <div class="modal-header">
          <h3>{{ review ? 'Edit Review' : 'Write a Review' }}</h3>
          <button class="btn-icon" @click="$emit('close')">✕</button>
        </div>
        <div style="display:flex;flex-direction:column;gap:16px">
          <div class="form-group">
            <label class="form-label">Rating</label>
            <StarInput v-model="form.rating" />
            <span v-if="errors.rating" class="form-error">{{ errors.rating }}</span>
          </div>
          <div class="form-group">
            <label class="form-label">Review</label>
            <textarea v-model="form.reviewText" class="form-textarea" placeholder="Share your thoughts… (optional)" style="min-height:140px" />
            <span v-if="errors.reviewText" class="form-error">{{ errors.reviewText }}</span>
          </div>
        </div>
        <div class="modal-footer">
          <button class="btn btn-secondary" @click="$emit('close')">Cancel</button>
          <button class="btn btn-primary" :disabled="loading" @click="submit">
            {{ loading ? 'Saving…' : (review ? 'Save' : 'Submit Review') }}
          </button>
        </div>
      </div>
    </div>
  </Teleport>
</template>

<script setup>
import { ref, watch } from 'vue'
import StarInput from '@/components/common/StarInput.vue'

const props = defineProps({
  show: { type: Boolean, default: false },
  review: { type: Object, default: null },
  pendingRating: { type: Number, default: 0 },
  loading: { type: Boolean, default: false },
})
const emit = defineEmits(['close', 'submit'])
const form = ref({ rating: 0, reviewText: '' })
const errors = ref({})

watch(() => props.show, v => {
  if (v) {
    errors.value = {}
    form.value = props.review
      ? { rating: props.review.rating, reviewText: props.review.reviewText ?? ''  }
      : { rating: props.pendingRating || 0, reviewText: ''  }
  }
})

function validate() {
  errors.value = {}
  if (!form.value.rating) errors.value.rating = 'Please select a rating'
  if (form.value.reviewText?.trim() && form.value.reviewText.trim().length < 5) errors.value.reviewText = 'Review must be at least 5 characters'
  return !Object.keys(errors.value).length
}

function submit() {
  if (!validate()) return
  emit('submit', { ...form.value, reviewText: form.value.reviewText?.trim() || null })
}
</script>