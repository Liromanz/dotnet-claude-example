import type { Periodicity } from '@/api/analyticsApi'

export const PERIODICITY_LABELS: Record<Periodicity, string> = {
  Weekly: 'Еженедельный',
  Monthly: 'Ежемесячный',
  Irregular: 'Нерегулярный',
}

export const PERIODICITY_OPTIONS = (Object.keys(PERIODICITY_LABELS) as Periodicity[]).map((value) => ({
  value,
  label: PERIODICITY_LABELS[value],
}))
