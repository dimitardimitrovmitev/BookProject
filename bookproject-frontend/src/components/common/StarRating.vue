<template>
  <div class="stars" :title="`${value} / ${max}`">
    <span
      v-for="i in max"
      :key="i"
      :class="['star', { filled: i <= Math.round(value) }]"
    >★</span>
    <span v-if="showValue" class="star-value">{{ Number(value).toFixed(1) }}</span>
  </div>
</template>

<script setup>
defineProps({
  value:     { type: Number, default: 0 },
  max:       { type: Number, default: 5 },
  showValue: { type: Boolean, default: false },
})
</script>

<style scoped>
.star-value { font-size: 0.85rem; color: var(--text-2); margin-left: 4px; font-family: var(--font-body); }
</style>
