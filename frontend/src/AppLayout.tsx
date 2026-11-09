import { Outlet } from '@tanstack/react-router'
import { TopNav } from '@/components/TopNav'
import styles from './AppLayout.module.css'

export function AppLayout() {
  return (
    <div className={styles.shell}>
      <TopNav />
      <main className={styles.content}>
        <Outlet />
      </main>
    </div>
  )
}
