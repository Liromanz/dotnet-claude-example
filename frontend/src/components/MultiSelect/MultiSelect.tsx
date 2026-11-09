import { useEffect, useRef, useState } from 'react'
import { CloseIcon, SearchIcon } from '@/components/icons'
import styles from './MultiSelect.module.css'

export interface MultiSelectOption {
  value: string
  label: string
}

interface MultiSelectProps {
  options: MultiSelectOption[]
  value: string[]
  onChange: (value: string[]) => void
  placeholder?: string
  maxSelected?: number
  className?: string
}

export function MultiSelect({ options, value, onChange, placeholder = 'Найти или выбрать', maxSelected, className }: MultiSelectProps) {
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

  const selectedOptions = options.filter((option) => value.includes(option.value))
  const atLimit = maxSelected !== undefined && value.length >= maxSelected
  const filtered = options.filter(
    (option) => !value.includes(option.value) && option.label.toLowerCase().includes(search.toLowerCase()),
  )

  function toggle(optionValue: string) {
    if (value.includes(optionValue)) {
      onChange(value.filter((v) => v !== optionValue))
    } else if (!atLimit) {
      onChange([...value, optionValue])
    }
  }

  return (
    <div ref={rootRef} className={[styles.root, className].filter(Boolean).join(' ')}>
      <button type="button" className={styles.trigger} onClick={() => setOpen((isOpen) => !isOpen)}>
        {selectedOptions.length === 0 ? (
          <span className={styles.placeholder}>{placeholder}</span>
        ) : (
          <span className={styles.chips}>
            {selectedOptions.map((option) => (
              <span key={option.value} className={styles.chip}>
                {option.label}
                <span
                  role="button"
                  tabIndex={-1}
                  className={styles.chipRemove}
                  onClick={(event) => {
                    event.stopPropagation()
                    toggle(option.value)
                  }}
                >
                  <CloseIcon width={10} height={10} />
                </span>
              </span>
            ))}
          </span>
        )}
      </button>

      {open && (
        <div className={styles.panel}>
          <div className={styles.searchRow}>
            <SearchIcon className={styles.searchIcon} />
            <input
              autoFocus
              className={styles.searchInput}
              value={search}
              onChange={(event) => setSearch(event.target.value)}
              placeholder={placeholder}
            />
          </div>
          <ul className={styles.options}>
            {filtered.map((option) => (
              <li key={option.value}>
                <button type="button" className={styles.option} disabled={atLimit} onClick={() => toggle(option.value)}>
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
