import type { ThemeStatus } from '@/api/dictionariesApi'

export const THEME_STATUS_LABELS: Record<ThemeStatus, string> = {
  Draft: 'Черновик',
  Approved: 'Утверждён',
}

export const THEME_STATUS_OPTIONS = (Object.keys(THEME_STATUS_LABELS) as ThemeStatus[]).map((value) => ({
  value,
  label: THEME_STATUS_LABELS[value],
}))
