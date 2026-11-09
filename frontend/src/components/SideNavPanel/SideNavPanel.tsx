import { Link, useRouterState } from '@tanstack/react-router'
import { ArrowRightIcon } from '@/components/icons'
import type { DirectoryEntry } from '@/directories'
import styles from './SideNavPanel.module.css'

interface SideNavPanelProps {
  title: string
  entries: DirectoryEntry[]
}

export function SideNavPanel({ title, entries }: SideNavPanelProps) {
  const pathname = useRouterState({ select: (state) => state.location.pathname })

  return (
    <nav className={styles.panel}>
      <h3 className={styles.title}>{title}</h3>
      <ul className={styles.list}>
        {entries.map((entry) => {
          const active = pathname.startsWith(entry.path)
          return (
            <li key={entry.id}>
              <Link to={entry.path} className={active ? styles.itemActive : styles.item}>
                <span>{entry.label}</span>
                {active && <ArrowRightIcon className={styles.arrow} />}
              </Link>
            </li>
          )
        })}
      </ul>
    </nav>
  )
}
