import styles from './Badge.module.css'

interface BadgeProps {
  tone: 'approved' | 'draft' | 'neutral'
  children: string
}

export function Badge({ tone, children }: BadgeProps) {
  return <span className={[styles.badge, styles[tone]].join(' ')}>{children}</span>
}
