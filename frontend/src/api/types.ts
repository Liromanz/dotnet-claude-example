export interface PagedResponse<TItem> {
  items: TItem[]
  totalCount: number
  page: number
  pageSize: number
}

export type SortDirection = 'Ascending' | 'Descending'

export interface PageQuery {
  page: number
  pageSize: number
  sortBy?: string
  sortDirection?: SortDirection
}
