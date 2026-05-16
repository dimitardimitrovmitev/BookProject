<template>
  <RouterLink :to="`/characters/${character.characterId}`" class="char-card card card-hover">
    <div class="char-header">
      <div class="char-avatar">{{ initial }}</div>
      <div class="char-badge-wrap">
        <span v-if="character.verified" class="badge badge-green">✓ Verified</span>
        <span v-else class="badge badge-gray">Unverified</span>
      </div>
    </div>
    <div class="char-body">
      <h3 class="char-name">{{ character.name }}</h3>
      <p class="char-book" v-if="character.bookTitle">from <em>{{ character.bookTitle }}</em></p>
      <p class="char-desc">{{ truncated }}</p>
    </div>
  </RouterLink>
</template>

<script setup>
import { computed } from 'vue'
const props = defineProps({ character: { type: Object, required: true } })
const initial = computed(() => props.character.name?.charAt(0)?.toUpperCase() || '?')
const truncated = computed(() => {
  const d = props.character.description || ''
  return d.length > 110 ? d.slice(0, 110) + '…' : d
})
</script>

<style scoped>
.char-card { text-decoration: none; color: inherit; display: flex; flex-direction: column; gap: 12px; }
.char-header { display: flex; align-items: center; justify-content: space-between; }
.char-avatar {
  width: 44px; height: 44px; border-radius: 50%;
  background: var(--amber-dim); border: 1px solid rgba(200,146,58,0.25);
  color: var(--amber-2); font-family: var(--font-display); font-weight: 700; font-size: 1.2rem;
  display: flex; align-items: center; justify-content: center;
}
.char-name { font-family: var(--font-display); font-size: 1.05rem; color: var(--text); transition: color var(--transition); }
.char-card:hover .char-name { color: var(--amber); }
.char-book { font-size: 0.82rem; color: var(--text-3); font-style: normal; }
.char-book em { font-style: italic; color: var(--amber); }
.char-desc { font-size: 0.87rem; color: var(--text-3); line-height: 1.45; }
</style>
