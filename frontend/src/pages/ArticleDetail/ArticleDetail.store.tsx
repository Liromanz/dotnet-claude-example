import { create } from 'zustand'
import type { ArticleDetailDto } from '@/api/analyticsApi'

interface ArticleDetailState {
  article: ArticleDetailDto | null
  loading: boolean
  error: string | null
  setArticle: (article: ArticleDetailDto | null) => void
  setLoading: (loading: boolean) => void
  setError: (error: string | null) => void
}

export const useArticleDetailStore = create<ArticleDetailState>((set) => ({
  article: null,
  loading: true,
  error: null,
  setArticle: (article) => set({ article }),
  setLoading: (loading) => set({ loading }),
  setError: (error) => set({ error }),
}))
