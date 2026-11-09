import { useEffect, useState } from 'react'
import { analyticsApi, type ThemeRefDto } from '@/api/analyticsApi'

/** Shared between the article filters sidebar and the "Новая статья" theme picker. */
export function useThemeOptions() {
  const [themes, setThemes] = useState<ThemeRefDto[]>([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    let cancelled = false
    analyticsApi
      .getThemeRefs()
      .then((result) => {
        if (!cancelled) setThemes(result)
      })
      .finally(() => {
        if (!cancelled) setLoading(false)
      })
    return () => {
      cancelled = true
    }
  }, [])

  return { themes, loading }
}
