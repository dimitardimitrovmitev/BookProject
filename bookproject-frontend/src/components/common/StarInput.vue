<template>
  <div class="star-input">
    <button
      v-for="i in max"
      :key="i"
      type="button"
      :class="['star-btn', { filled: i <= (hovered || modelValue) }]"
      @mouseenter="hovered = i"
      @mouseleave="hovered = 0"
      @click="$emit('update:modelValue', i)"
    >★</button>
    <span class="star-label" v-if="modelValue">{{ modelValue }} / {{ max }}</span>
  </div>
</template>

<script setup>
import { ref } from 'vue'
defineProps({ modelValue: { type: Number, default: 0 }, max: { type: Number, default: 5 } })
defineEmits(['update:modelValue'])
const hovered = ref(0)
</script>

<style scoped>
.star-input { display: flex; align-items: center; gap: 4px; }
.star-btn { background: none; border: none; cursor: pointer; font-size: 1.4rem; color: var(--text-3); transition: color 0.15s, transform 0.1s; padding: 0; line-height: 1; }
.star-btn:hover, .star-btn.filled { color: var(--amber); }
.star-btn:hover { transform: scale(1.2); }
.star-label { font-size: 0.85rem; color: var(--text-2); margin-left: 6px; font-family: var(--font-body); }
</style>
