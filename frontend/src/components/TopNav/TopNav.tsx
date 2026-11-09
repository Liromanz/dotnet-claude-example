import { Link, useRouterState } from '@tanstack/react-router'
import { TOP_NAV_ENTRIES } from '@/directories'
import styles from './TopNav.module.css'

export function TopNav() {
  const pathname = useRouterState({ select: (state) => state.location.pathname })

  return (
    <header className={styles.header}>
      <div className={styles.brand}>
        <svg width="22" height="22" viewBox="0 0 22 22" fill="none" aria-hidden>
          <circle cx="11" cy="11" r="9.5" stroke="currentColor" strokeWidth="1.5" />
          <path d="M6.5 11.5 9.5 14.5 15.5 7.5" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round" />
        </svg>
        MetricBoard
      </div>

      <nav className={styles.nav}>
        {TOP_NAV_ENTRIES.map((entry) => {
          const active = entry.enabled && pathname.startsWith(entry.path)
          if (!entry.enabled) {
            return (
              <span key={entry.id} className={styles.navItemDisabled} title="Пока недоступно">
                {entry.label}
              </span>
            )
          }
          return (
            <Link key={entry.id} to={entry.path} className={active ? styles.navItemActive : styles.navItem}>
              {entry.label}
            </Link>
          )
        })}
      </nav>

      <div className={styles.actions}>
        <button type="button" className={styles.iconSlot} aria-label="Информация">
          i
        </button>
        <button type="button" className={styles.avatar} aria-label="Профиль">
          <svg width="16" height="16" viewBox="0 0 16 16" fill="none">
            <circle cx="8" cy="5.5" r="2.5" stroke="currentColor" strokeWidth="1.3" />
            <path d="M2.5 13.5c1-2.8 3.2-4 5.5-4s4.5 1.2 5.5 4" stroke="currentColor" strokeWidth="1.3" strokeLinecap="round" />
          </svg>
        </button>
      </div>
    </header>
  )
}
