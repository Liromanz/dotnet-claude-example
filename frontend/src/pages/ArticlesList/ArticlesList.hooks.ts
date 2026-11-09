import { useMemo } from 'react'
import { analyticsApi } from '@/api/analyticsApi'
import { useServerTable } from '@/hooks/useServerTable'
import { useArticlesListStore } from './ArticlesList.store'

export function useArticlesListTable() {
  const filters = useArticlesListStore((state) => state.filters)

  const apiFilters = useMemo(
    () => ({
      search: filters.search || undefined,
      themeId: filters.themeId ?? undefined,
      periodicity: filters.periodicity ?? undefined,
      author: filters.author ?? undefined,
    }),
    [filters],
  )

  return useServerTable({
    fetchPage: analyticsApi.getArticles,
    filters: apiFilters,
    initialPageSize: 10,
    initialSortBy: 'publishedAtUtc',
    initialSortDirection: 'Descending',
  })
}
