<template>
  <div v-if="totalPages > 1" class="pagination">
    <button class="page-btn" :disabled="modelValue === 1" @click="$emit('update:modelValue', 1)">«</button>
    <button class="page-btn" :disabled="modelValue === 1" @click="$emit('update:modelValue', modelValue - 1)">‹</button>
    <button
      v-for="p in pages"
      :key="p"
      :class="['page-btn', { active: p === modelValue }]"
      @click="$emit('update:modelValue', p)"
    >{{ p }}</button>
    <button class="page-btn" :disabled="modelValue === totalPages" @click="$emit('update:modelValue', modelValue + 1)">›</button>
    <button class="page-btn" :disabled="modelValue === totalPages" @click="$emit('update:modelValue', totalPages)">»</button>
  </div>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  modelValue: { type: Number, required: true },
  totalPages: { type: Number, required: true },
})
defineEmits(['update:modelValue'])

const pages = computed(() => {
  const all = []
  const start = Math.max(1, props.modelValue - 2)
  const end   = Math.min(props.totalPages, props.modelValue + 2)
  for (let i = start; i <= end; i++) all.push(i)
  return all
})
</script>
