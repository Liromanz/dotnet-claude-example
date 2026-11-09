import { create } from 'zustand'
import type { Periodicity } from '@/api/analyticsApi'

export interface ArticlesListFilters {
  search: string
  themeId: string | null
  periodicity: Periodicity | null
  author: string | null
}

const STORAGE_KEY = 'metricboard.articlesList.filters'

const defaultFilters: ArticlesListFilters = { search: '', themeId: null, periodicity: null, author: null }

function loadPersistedFilters(): ArticlesListFilters {
  try {
    const raw = localStorage.getItem(STORAGE_KEY)
    if (!raw) return defaultFilters
    return { ...defaultFilters, ...(JSON.parse(raw) as Partial<ArticlesListFilters>) }
  } catch {
    return defaultFilters
  }
}

function persist(filters: ArticlesListFilters) {
  try {
    localStorage.setItem(STORAGE_KEY, JSON.stringify(filters))
  } catch {
    // Private browsing / storage disabled — filters just won't survive a reload.
  }
}

interface ArticlesListState {
  filters: ArticlesListFilters
  isCreateModalOpen: boolean
  setSearch: (search: string) => void
  applyFilters: (filters: Omit<ArticlesListFilters, 'search'>) => void
  resetFilters: () => void
  openCreateModal: () => void
  closeCreateModal: () => void
}

export const useArticlesListStore = create<ArticlesListState>((set, get) => ({
  filters: loadPersistedFilters(),
  isCreateModalOpen: false,

  setSearch: (search) => {
    const filters = { ...get().filters, search }
    persist(filters)
    set({ filters })
  },

  applyFilters: (partial) => {
    const filters = { ...get().filters, ...partial }
    persist(filters)
    set({ filters })
  },

  resetFilters: () => {
    const filters = { ...defaultFilters, search: get().filters.search }
    persist(filters)
    set({ filters })
  },

  openCreateModal: () => set({ isCreateModalOpen: true }),
  closeCreateModal: () => set({ isCreateModalOpen: false }),
}))
