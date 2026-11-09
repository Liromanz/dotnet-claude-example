import { EyeIcon } from '@/components/icons'
import styles from './ViewCount.module.css'

export function ViewCount({ count }: { count: number }) {
  return (
    <span className={styles.viewCount}>
      <EyeIcon />
      {count}
    </span>
  )
}
