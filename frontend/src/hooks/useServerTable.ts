import { useCallback, useEffect, useState } from 'react'
import type { PagedResponse, SortDirection } from '@/api/types'

export interface UseServerTableOptions<TItem, TFilters extends object> {
  fetchPage: (
    params: TFilters & { page: number; pageSize: number; sortBy?: string; sortDirection?: SortDirection },
  ) => Promise<PagedResponse<TItem>>
  filters: TFilters
  initialPageSize?: number
  initialSortBy?: string
  initialSortDirection?: SortDirection
}

export interface UseServerTableResult<TItem> {
  items: TItem[]
  totalCount: number
  page: number
  pageSize: number
  setPage: (page: number) => void
  setPageSize: (pageSize: number) => void
  sortBy: string | undefined
  sortDirection: SortDirection
  toggleSort: (column: string) => void
  loading: boolean
  error: string | null
  refetch: () => void
}

/**
 * Every table/list page in this app is server-driven: no client-side filtering, sorting or
 * paging. This hook just tracks the query state and re-fetches whenever it changes — the
 * backend owns all the actual logic.
 */
export function useServerTable<TItem, TFilters extends object>(
  options: UseServerTableOptions<TItem, TFilters>,
): UseServerTableResult<TItem> {
  const { fetchPage, filters, initialPageSize = 20, initialSortBy, initialSortDirection = 'Ascending' } = options

  const [page, setPage] = useState(1)
  const [pageSize, setPageSize] = useState(initialPageSize)
  const [sortBy, setSortBy] = useState<string | undefined>(initialSortBy)
  const [sortDirection, setSortDirection] = useState<SortDirection>(initialSortDirection)
  const [items, setItems] = useState<TItem[]>([])
  const [totalCount, setTotalCount] = useState(0)
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [requestId, setRequestId] = useState(0)

  const filtersKey = JSON.stringify(filters)

  useEffect(() => {
    setPage(1)
    // Filters changed — always restart at page 1, same as the "Применить" button would.
  }, [filtersKey])

  useEffect(() => {
    let cancelled = false
    setLoading(true)
    setError(null)

    fetchPage({ ...filters, page, pageSize, sortBy, sortDirection })
      .then((result) => {
        if (cancelled) return
        setItems(result.items)
        setTotalCount(result.totalCount)
      })
      .catch((err: unknown) => {
        if (cancelled) return
        setError(err instanceof Error ? err.message : 'Не удалось загрузить данные')
      })
      .finally(() => {
        if (!cancelled) setLoading(false)
      })

    return () => {
      cancelled = true
    }
    // `fetchPage` is intentionally not in the dependency list — it closes over nothing
    // filter-specific (filters travel through `filters`/`filtersKey`), so re-running on an
    // unstable function identity would only cause redundant fetches.
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [filtersKey, page, pageSize, sortBy, sortDirection, requestId])

  const toggleSort = useCallback((column: string) => {
    setSortBy((previousColumn) => {
      if (previousColumn !== column) {
        setSortDirection('Ascending')
        return column
      }
      setSortDirection((previousDirection) => (previousDirection === 'Ascending' ? 'Descending' : 'Ascending'))
      return column
    })
  }, [])

  const refetch = useCallback(() => setRequestId((id) => id + 1), [])

  return {
    items,
    totalCount,
    page,
    pageSize,
    setPage,
    setPageSize,
    sortBy,
    sortDirection,
    toggleSort,
    loading,
    error,
    refetch,
  }
}
