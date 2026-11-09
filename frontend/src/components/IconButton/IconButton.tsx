import type { ButtonHTMLAttributes, ReactNode } from 'react'
import styles from './IconButton.module.css'

interface IconButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  children: ReactNode
  'aria-label': string
}

export function IconButton({ className, children, ...rest }: IconButtonProps) {
  return (
    <button className={[styles.iconButton, className].filter(Boolean).join(' ')} {...rest}>
      {children}
    </button>
  )
}
