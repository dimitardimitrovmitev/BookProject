<template>
  <div>
    <div class="page-header">
      <div class="page-header-left">
        <h1>My Scenes</h1>
        <p>Character groupings for AI image generation</p>
      </div>
      <div class="page-actions">
        <button class="btn btn-primary" @click="showForm = true">+ New Scene</button>
      </div>
    </div>

    <div v-if="loading" class="grid-3" style="margin-top:24px">
      <div v-for="i in 6" :key="i" class="skeleton" style="height:180px;border-radius:12px" />
    </div>

    <div v-else-if="scenes.length" class="grid-3" style="margin-top:24px">
      <div v-for="scene in scenes" :key="scene.sceneId" class="card scene-card">
        <div class="scene-header">
          <h3 class="scene-title">{{ scene.title }}</h3>
          <div class="flex gap-2">
            <button class="btn-icon" @click="editScene(scene)">✎</button>
            <button class="btn-icon" @click="confirmDelete(scene)" style="color:var(--danger)">✕</button>
          </div>
        </div>
        <div class="scene-meta">
          <span class="badge badge-gray">{{ scene.characterIds?.length || 0 }} characters</span>
          <span class="text-muted" style="font-size:0.78rem">{{ formatDate(scene.createdAt) }}</span>
        </div>
        <div v-if="scene.imageUrls?.length" class="scene-images">
          <img v-for="url in scene.imageUrls" :key="url" :src="url" alt="Scene" class="scene-img" />
        </div>
        <div v-else class="scene-placeholder">
          <span>🎨</span>
          <span style="font-size:0.82rem;color:var(--text-3)">No images yet</span>
        </div>
      </div>
    </div>

    <div v-else class="empty-state">
      <div class="empty-state-icon">🎭</div>
      <h3>No scenes yet</h3>
      <p>Create a scene to group characters together for AI image generation</p>
    </div>

    <!-- Scene form modal -->
    <Teleport to="body">
      <div v-if="showForm" class="modal-overlay" @click.self="closeForm">
        <div class="modal">
          <div class="modal-header">
            <h3>{{ editingScene ? 'Edit Scene' : 'New Scene' }}</h3>
            <button class="btn-icon" @click="closeForm">✕</button>
          </div>
          <div style="display:flex;flex-direction:column;gap:16px">
            <div class="form-group">
              <label class="form-label">Scene Title *</label>
              <input v-model="form.title" class="form-input" placeholder="e.g. The Final Confrontation" />
              <span v-if="formError" class="form-error">{{ formError }}</span>
            </div>
            <div class="form-group">
              <label class="form-label">Characters (by ID)</label>
              <input v-model="charIdsInput" class="form-input" placeholder="e.g. 1, 3, 7" />
              <span class="form-hint">Comma-separated character IDs</span>
            </div>
          </div>
          <div class="modal-footer">
            <button class="btn btn-secondary" @click="closeForm">Cancel</button>
            <button class="btn btn-primary" :disabled="formLoading" @click="handleSubmit">
              {{ formLoading ? 'Saving…' : (editingScene ? 'Save' : 'Create Scene') }}
            </button>
          </div>
        </div>
      </div>
    </Teleport>

    <ConfirmModal :show="showDelete" title="Delete Scene" :message="`Delete '${deletingScene?.title}'?`" :loading="deleteLoading" @confirm="handleDelete" @cancel="showDelete=false" />
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { scenesApi } from '@/api/index'
import { useToast } from '@/composables/useToast'
import ConfirmModal from '@/components/common/ConfirmModal.vue'

const toast = useToast()
const scenes = ref([])
const loading = ref(false)
const showForm = ref(false)
const editingScene = ref(null)
const form = ref({ title: '' })
const charIdsInput = ref('')
const formError = ref('')
const formLoading = ref(false)
const showDelete = ref(false)
const deletingScene = ref(null)
const deleteLoading = ref(false)

function formatDate(d) { return new Date(d).toLocaleDateString('en-US', { month:'short', day:'numeric', year:'numeric' }) }

async function fetchScenes() {
  loading.value = true
  try {
    const { data } = await scenesApi.getMy()
    scenes.value = Array.isArray(data) ? data : (data.items || [])
  } catch {}
  finally { loading.value = false }
}

function editScene(s) {
  editingScene.value = s
  form.value = { title: s.title }
  charIdsInput.value = (s.characterIds || []).join(', ')
  showForm.value = true
}

function closeForm() { showForm.value = false; editingScene.value = null; form.value = { title: '' }; charIdsInput.value = ''; formError.value = '' }

async function handleSubmit() {
  if (!form.value.title?.trim()) { formError.value = 'Title is required'; return }
  formError.value = ''
  const characterIds = charIdsInput.value ? charIdsInput.value.split(',').map(s => parseInt(s.trim())).filter(n => !isNaN(n)) : []
  formLoading.value = true
  try {
    if (editingScene.value) {
      await scenesApi.update(editingScene.value.sceneId, { title: form.value.title, characterIds })
      toast.success('Scene updated')
    } else {
      await scenesApi.create({ title: form.value.title, characterIds })
      toast.success('Scene created')
    }
    closeForm(); fetchScenes()
  } catch (e) { toast.error(e.response?.data || 'Failed to save') }
  finally { formLoading.value = false }
}

function confirmDelete(s) { deletingScene.value = s; showDelete.value = true }

async function handleDelete() {
  deleteLoading.value = true
  try {
    await scenesApi.delete(deletingScene.value.sceneId)
    toast.success('Scene deleted')
    showDelete.value = false; deletingScene.value = null; fetchScenes()
  } catch { toast.error('Failed to delete') }
  finally { deleteLoading.value = false }
}

onMounted(fetchScenes)
</script>

<style scoped>
.scene-card { display:flex;flex-direction:column;gap:10px; }
.scene-header { display:flex;align-items:flex-start;justify-content:space-between;gap:8px; }
.scene-title { font-family:var(--font-display);font-size:1.05rem; }
.scene-meta { display:flex;align-items:center;gap:10px;flex-wrap:wrap; }
.scene-images { display:flex;gap:8px;flex-wrap:wrap; }
.scene-img { height:80px;border-radius:var(--radius);object-fit:cover;border:1px solid var(--border); }
.scene-placeholder { display:flex;align-items:center;gap:8px;padding:16px;background:var(--bg-3);border-radius:var(--radius);border:1px dashed var(--border); }
</style>
