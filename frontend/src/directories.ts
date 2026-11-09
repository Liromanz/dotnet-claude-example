/**
 * Single source of truth for navigation: which top-level sections exist, which ones are
 * actually wired up to a page, and what the side nav shows underneath each one. TopNav,
 * SideNavPanel and router.tsx all read from here instead of hard-coding paths.
 */
export type DirectorySection = 'analytics' | 'dictionaries'

export interface DirectoryEntry {
  id: string
  label: string
  path: string
  enabled: boolean
}

export const TOP_NAV_ENTRIES: DirectoryEntry[] = [
  { id: 'data', label: 'Данные', path: '/data', enabled: false },
  { id: 'analytics', label: 'Аналитика', path: '/analytics/articles', enabled: true },
  { id: 'dashboards', label: 'Дашборды', path: '/dashboards', enabled: false },
  { id: 'dictionaries', label: 'Справочники', path: '/dictionaries/themes', enabled: true },
  { id: 'tariffs', label: 'Тарифы', path: '/tariffs', enabled: false },
  { id: 'reports', label: 'Отчетности', path: '/reports', enabled: false },
]

export const SIDE_NAV_ENTRIES: Record<DirectorySection, DirectoryEntry[]> = {
  analytics: [{ id: 'articles', label: 'Статьи', path: '/analytics/articles', enabled: true }],
  dictionaries: [{ id: 'themes', label: 'Темы', path: '/dictionaries/themes', enabled: true }],
}

export function findTopNavEntry(pathname: string): DirectoryEntry | undefined {
  return TOP_NAV_ENTRIES.find((entry) => pathname.startsWith(entry.path))
}
