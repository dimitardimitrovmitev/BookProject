<template>
  <Teleport to="body">
    <div v-if="show" class="modal-overlay" @click.self="$emit('cancel')">
      <div class="modal" style="max-width:400px">
        <div class="modal-header">
          <h3>{{ title }}</h3>
          <button class="btn-icon" @click="$emit('cancel')">✕</button>
        </div>
        <p style="color:var(--text-2)">{{ message }}</p>
        <div class="modal-footer">
          <button class="btn btn-secondary" @click="$emit('cancel')">Cancel</button>
          <button class="btn btn-danger" :disabled="loading" @click="$emit('confirm')">
            {{ loading ? 'Deleting…' : 'Delete' }}
          </button>
        </div>
      </div>
    </div>
  </Teleport>
</template>

<script setup>
defineProps({
  show:    { type: Boolean, default: false },
  title:   { type: String, default: 'Confirm Delete' },
  message: { type: String, default: 'Are you sure? This cannot be undone.' },
  loading: { type: Boolean, default: false },
})
defineEmits(['confirm', 'cancel'])
</script>
