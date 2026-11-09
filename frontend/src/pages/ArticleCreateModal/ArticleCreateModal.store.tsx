import { create } from 'zustand'
import type { Periodicity } from '@/api/analyticsApi'

interface ArticleCreateModalState {
  title: string
  subtitle: string
  body: string
  author: string
  periodicity: Periodicity | null
  themeIds: string[]
  submitting: boolean
  error: string | null
  setTitle: (value: string) => void
  setSubtitle: (value: string) => void
  setBody: (value: string) => void
  setAuthor: (value: string) => void
  setPeriodicity: (value: Periodicity | null) => void
  setThemeIds: (value: string[]) => void
  setSubmitting: (value: boolean) => void
  setError: (value: string | null) => void
  reset: () => void
}

const initialFields = {
  title: '',
  subtitle: '',
  body: '',
  author: '',
  periodicity: null as Periodicity | null,
  themeIds: [] as string[],
  submitting: false,
  error: null as string | null,
}

export const useArticleCreateModalStore = create<ArticleCreateModalState>((set) => ({
  ...initialFields,
  setTitle: (title) => set({ title }),
  setSubtitle: (subtitle) => set({ subtitle }),
  setBody: (body) => set({ body }),
  setAuthor: (author) => set({ author }),
  setPeriodicity: (periodicity) => set({ periodicity }),
  setThemeIds: (themeIds) => set({ themeIds }),
  setSubmitting: (submitting) => set({ submitting }),
  setError: (error) => set({ error }),
  reset: () => set({ ...initialFields }),
}))
