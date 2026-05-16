<template>
  <Teleport to="body">
    <div v-if="show" class="modal-overlay" @click.self="$emit('close')">
      <div class="modal modal-lg">
        <div class="modal-header">
          <h3>{{ review ? 'Edit Character Review' : 'Review Character' }}</h3>
          <button class="btn-icon" @click="$emit('close')">✕</button>
        </div>
        <div style="display:flex;flex-direction:column;gap:18px">
          <!-- Sub-ratings -->
          <div class="ratings-grid">
            <div v-for="r in ratingFields" :key="r.key" class="form-group">
              <label class="form-label">{{ r.label }}</label>
              <StarInput v-model="form[r.key]" />
            </div>
          </div>
          <!-- Overall preview -->
          <div class="overall-preview" v-if="overallRating">
            <span class="form-label">Computed Overall</span>
            <StarRating :value="overallRating" :show-value="true" />
          </div>
          <!-- Review text -->
          <div class="form-group">
            <label class="form-label">Review Text</label>
            <textarea v-model="form.reviewText" class="form-textarea" placeholder="Your thoughts on this character…" style="min-height:120px" />
            <span v-if="errors.reviewText" class="form-error">{{ errors.reviewText }}</span>
          </div>
        </div>
        <div class="modal-footer">
          <button class="btn btn-secondary" @click="$emit('close')">Cancel</button>
          <button class="btn btn-primary" :disabled="loading" @click="submit">
            {{ loading ? 'Saving…' : (review ? 'Save Changes' : 'Submit Review') }}
          </button>
        </div>
      </div>
    </div>
  </Teleport>
</template>

<script setup>
import { ref, watch, computed } from 'vue'
import StarInput from '@/components/common/StarInput.vue'
import StarRating from '@/components/common/StarRating.vue'

const props = defineProps({
  show:    { type: Boolean, default: false },
  review:  { type: Object, default: null },
  loading: { type: Boolean, default: false },
})
const emit = defineEmits(['close', 'submit'])

const ratingFields = [
  { key: 'depthComplexity',          label: 'Depth & Complexity' },
  { key: 'characterDevelopment',     label: 'Character Development' },
  { key: 'consistencyBelievability', label: 'Consistency & Believability' },
  { key: 'impactOnStory',            label: 'Impact on Story' },
  { key: 'memorability',             label: 'Memorability' },
]

const defaultForm = () => ({
  reviewText: '',
  depthComplexity: 0,
  characterDevelopment: 0,
  consistencyBelievability: 0,
  impactOnStory: 0,
  memorability: 0,
})

const form = ref(defaultForm())
const errors = ref({})

const overallRating = computed(() => {
  const vals = ratingFields.map(r => form.value[r.key]).filter(v => v > 0)
  if (!vals.length) return 0
  return vals.reduce((a, b) => a + b, 0) / vals.length
})

watch(() => props.show, v => {
  if (v) {
    errors.value = {}
    if (props.review) {
      form.value = {
        reviewText: props.review.reviewText,
        depthComplexity: props.review.depthComplexity,
        characterDevelopment: props.review.characterDevelopment,
        consistencyBelievability: props.review.consistencyBelievability,
        impactOnStory: props.review.impactOnStory,
        memorability: props.review.memorability,
      }
    } else {
      form.value = defaultForm()
    }
  }
})

function validate() {
  errors.value = {}
  if (!form.value.reviewText?.trim() || form.value.reviewText.length < 5) errors.value.reviewText = 'Review must be at least 5 characters'
  return !Object.keys(errors.value).length
}

function submit() {
  if (!validate()) return
  emit('submit', { ...form.value })
}
</script>

<style scoped>
.ratings-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 14px; }
.overall-preview { display: flex; align-items: center; gap: 12px; padding: 12px 14px; background: var(--bg-3); border-radius: var(--radius); border: 1px solid var(--border); }
@media (max-width: 500px) { .ratings-grid { grid-template-columns: 1fr; } }
</style>
