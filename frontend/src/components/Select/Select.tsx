import { useEffect, useRef, useState } from 'react'
import { ChevronDownIcon, SearchIcon } from '@/components/icons'
import styles from './Select.module.css'

export interface SelectOption {
  value: string
  label: string
}

interface SelectProps {
  options: SelectOption[]
  value: string | null
  onChange: (value: string | null) => void
  placeholder?: string
  searchable?: boolean
  className?: string
}

export function Select({ options, value, onChange, placeholder = 'Выберите', searchable = true, className }: SelectProps) {
  const [open, setOpen] = useState(false)
  const [search, setSearch] = useState('')
  const rootRef = useRef<HTMLDivElement>(null)

  useEffect(() => {
    function handleClickOutside(event: MouseEvent) {
      if (rootRef.current && !rootRef.current.contains(event.target as Node)) {
        setOpen(false)
        setSearch('')
      }
    }
    document.addEventListener('mousedown', handleClickOutside)
    return () => document.removeEventListener('mousedown', handleClickOutside)
  }, [])

  const selected = options.find((option) => option.value === value)
  const filtered = searchable
    ? options.filter((option) => option.label.toLowerCase().includes(search.toLowerCase()))
    : options

  return (
    <div ref={rootRef} className={[styles.root, className].filter(Boolean).join(' ')}>
      <button type="button" className={styles.trigger} onClick={() => setOpen((isOpen) => !isOpen)}>
        <span className={selected ? styles.value : styles.placeholder}>{selected?.label ?? placeholder}</span>
        <ChevronDownIcon className={styles.chevron} />
      </button>

      {open && (
        <div className={styles.panel}>
          {searchable && (
            <div className={styles.searchRow}>
              <SearchIcon className={styles.searchIcon} />
              <input
                autoFocus
                className={styles.searchInput}
                value={search}
                onChange={(event) => setSearch(event.target.value)}
                placeholder="Найти или выбрать"
              />
            </div>
          )}
          <ul className={styles.options}>
            {value !== null && (
              <li>
                <button
                  type="button"
                  className={styles.clearOption}
                  onClick={() => {
                    onChange(null)
                    setOpen(false)
                    setSearch('')
                  }}
                >
                  Все
                </button>
              </li>
            )}
            {filtered.map((option) => (
              <li key={option.value}>
                <button
                  type="button"
                  className={option.value === value ? styles.optionActive : styles.option}
                  onClick={() => {
                    onChange(option.value)
                    setOpen(false)
                    setSearch('')
                  }}
                >
                  {option.label}
                </button>
              </li>
            ))}
            {filtered.length === 0 && <li className={styles.empty}>Ничего не найдено</li>}
          </ul>
        </div>
      )}
    </div>
  )
}
