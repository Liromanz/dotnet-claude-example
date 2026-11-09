import { useEffect, useRef } from 'react'
import { analyticsApi } from '@/api/analyticsApi'
import { useArticleDetailStore } from './ArticleDetail.store'

export function useLoadArticleDetail(articleId: string) {
  const setArticle = useArticleDetailStore((state) => state.setArticle)
  const setLoading = useArticleDetailStore((state) => state.setLoading)
  const setError = useArticleDetailStore((state) => state.setError)

  // GET increments the view-count metric server-side, so this fetch must fire exactly once
  // per articleId — without this guard, StrictMode's dev-only double-invoke of effects would
  // record two views for a single page open.
  const requestedFor = useRef<string | null>(null)

  useEffect(() => {
    if (requestedFor.current === articleId) return
    requestedFor.current = articleId

    let cancelled = false
    setLoading(true)
    setError(null)

    analyticsApi
      .getArticleById(articleId)
      .then((article) => {
        if (!cancelled) setArticle(article)
      })
      .catch((err: unknown) => {
        if (!cancelled) setError(err instanceof Error ? err.message : 'Не удалось загрузить статью.')
      })
      .finally(() => {
        if (!cancelled) setLoading(false)
      })

    return () => {
      cancelled = true
    }
  }, [articleId, setArticle, setLoading, setError])
}
