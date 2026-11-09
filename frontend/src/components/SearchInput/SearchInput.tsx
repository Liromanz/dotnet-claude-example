import type { InputHTMLAttributes } from 'react'
import { SearchIcon } from '@/components/icons'
import styles from './SearchInput.module.css'

type SearchInputProps = Omit<InputHTMLAttributes<HTMLInputElement>, 'type'>

export function SearchInput({ className, ...rest }: SearchInputProps) {
  return (
    <label className={[styles.wrapper, className].filter(Boolean).join(' ')}>
      <SearchIcon className={styles.icon} />
      <input type="text" className={styles.input} {...rest} />
    </label>
  )
}
