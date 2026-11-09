import type { ReactNode } from 'react'
import { SortIcon } from '@/components/icons'
import type { SortDirection } from '@/api/types'
import styles from './DataTable.module.css'

export interface DataTableColumn<TRow> {
  key: string
  header: string
  sortable?: boolean
  render: (row: TRow) => ReactNode
  filter?: ReactNode
  width?: string
}

interface DataTableProps<TRow> {
  columns: DataTableColumn<TRow>[]
  rows: TRow[]
  rowKey: (row: TRow) => string
  sortBy?: string
  sortDirection: SortDirection
  onSort: (column: string) => void
  selectable?: boolean
  selectedKeys?: Set<string>
  onToggleSelect?: (key: string) => void
  onToggleSelectAll?: () => void
  loading?: boolean
  emptyMessage?: string
}

export function DataTable<TRow>({
  columns,
  rows,
  rowKey,
  sortBy,
  sortDirection,
  onSort,
  selectable,
  selectedKeys,
  onToggleSelect,
  onToggleSelectAll,
  loading,
  emptyMessage = 'Нет данных',
}: DataTableProps<TRow>) {
  const allSelected = selectable && rows.length > 0 && rows.every((row) => selectedKeys?.has(rowKey(row)))
  const hasFilters = columns.some((column) => column.filter)

  return (
    <div className={styles.wrapper}>
      <table className={styles.table}>
        <thead>
          <tr>
            {selectable && (
              <th className={styles.checkboxCell}>
                <input type="checkbox" checked={allSelected} onChange={onToggleSelectAll} />
              </th>
            )}
            {columns.map((column) => (
              <th key={column.key} style={{ width: column.width }}>
                {column.sortable ? (
                  <button type="button" className={styles.sortButton} onClick={() => onSort(column.key)}>
                    {column.header}
                    <SortIcon direction={sortBy === column.key ? sortDirection : undefined} />
                  </button>
                ) : (
                  column.header
                )}
              </th>
            ))}
          </tr>
          {hasFilters && (
            <tr className={styles.filterRow}>
              {selectable && <th />}
              {columns.map((column) => (
                <th key={column.key}>{column.filter}</th>
              ))}
            </tr>
          )}
        </thead>
        <tbody>
          {rows.map((row) => {
            const key = rowKey(row)
            return (
              <tr key={key}>
                {selectable && (
                  <td className={styles.checkboxCell}>
                    <input
                      type="checkbox"
                      checked={selectedKeys?.has(key) ?? false}
                      onChange={() => onToggleSelect?.(key)}
                    />
                  </td>
                )}
                {columns.map((column) => (
                  <td key={column.key}>{column.render(row)}</td>
                ))}
              </tr>
            )
          })}
          {!loading && rows.length === 0 && (
            <tr>
              <td className={styles.empty} colSpan={columns.length + (selectable ? 1 : 0)}>
                {emptyMessage}
              </td>
            </tr>
          )}
        </tbody>
      </table>
      {loading && <div className={styles.loadingOverlay}>Загрузка…</div>}
    </div>
  )
}
