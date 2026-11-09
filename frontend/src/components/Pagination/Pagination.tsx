import { ArrowLeftIcon, ArrowRightIcon } from '@/components/icons'
import styles from './Pagination.module.css'

interface PaginationProps {
  page: number
  pageSize: number
  totalCount: number
  onPageChange: (page: number) => void
  onPageSizeChange: (pageSize: number) => void
  pageSizeOptions?: number[]
}

export function Pagination({
  page,
  pageSize,
  totalCount,
  onPageChange,
  onPageSizeChange,
  pageSizeOptions = [20, 50, 100],
}: PaginationProps) {
  const pageCount = Math.max(1, Math.ceil(totalCount / pageSize))

  return (
    <div className={styles.row}>
      <button type="button" className={styles.navButton} disabled={page <= 1} onClick={() => onPageChange(page - 1)}>
        <ArrowLeftIcon />
      </button>
      <span className={styles.pageLabel}>
        {page} / {pageCount}
      </span>
      <button
        type="button"
        className={styles.navButton}
        disabled={page >= pageCount}
        onClick={() => onPageChange(page + 1)}
      >
        <ArrowRightIcon />
      </button>

      <select
        className={styles.pageSizeSelect}
        value={pageSize}
        onChange={(event) => onPageSizeChange(Number(event.target.value))}
      >
        {pageSizeOptions.map((size) => (
          <option key={size} value={size}>
            {size}
          </option>
        ))}
      </select>

      <span className={styles.total}>Всего: {totalCount}</span>
    </div>
  )
}
