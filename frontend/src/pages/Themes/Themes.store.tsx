import { create } from 'zustand'

interface ThemeFormFields {
  name: string
  author: string
  editor: string
}

type ThemesModal = 'create' | 'edit' | 'approve' | null

interface ThemesState {
  selectedIds: Set<string>
  modal: ThemesModal
  editingThemeId: string | null
  form: ThemeFormFields
  submitting: boolean
  error: string | null

  toggleSelect: (id: string) => void
  toggleSelectAll: (ids: string[]) => void
  clearSelection: () => void

  openCreateModal: () => void
  openEditModal: (theme: { id: string; name: string }) => void
  openApproveModal: () => void
  closeModal: () => void

  setFormField: <K extends keyof ThemeFormFields>(key: K, value: ThemeFormFields[K]) => void
  setSubmitting: (value: boolean) => void
  setError: (value: string | null) => void
}

const emptyForm: ThemeFormFields = { name: '', author: '', editor: '' }

export const useThemesStore = create<ThemesState>((set, get) => ({
  selectedIds: new Set(),
  modal: null,
  editingThemeId: null,
  form: emptyForm,
  submitting: false,
  error: null,

  toggleSelect: (id) => {
    const next = new Set(get().selectedIds)
    if (next.has(id)) {
      next.delete(id)
    } else {
      next.add(id)
    }
    set({ selectedIds: next })
  },

  toggleSelectAll: (ids) => {
    const allSelected = ids.length > 0 && ids.every((id) => get().selectedIds.has(id))
    set({ selectedIds: allSelected ? new Set() : new Set(ids) })
  },

  clearSelection: () => set({ selectedIds: new Set() }),

  openCreateModal: () => set({ modal: 'create', form: emptyForm, error: null }),

  openEditModal: (theme) =>
    set({ modal: 'edit', editingThemeId: theme.id, form: { ...emptyForm, name: theme.name }, error: null }),

  openApproveModal: () => set({ modal: 'approve', form: emptyForm, error: null }),

  closeModal: () => set({ modal: null, error: null }),

  setFormField: (key, value) => set({ form: { ...get().form, [key]: value } }),
  setSubmitting: (submitting) => set({ submitting }),
  setError: (error) => set({ error }),
}))
