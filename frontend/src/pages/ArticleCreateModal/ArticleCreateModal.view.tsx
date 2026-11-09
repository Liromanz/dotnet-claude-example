import { useEffect } from 'react'
import { Button } from '@/components/Button'
import { Modal } from '@/components/Modal'
import { MultiSelect } from '@/components/MultiSelect'
import { Select } from '@/components/Select'
import { PERIODICITY_OPTIONS } from '@/constants/periodicity'
import { useThemeOptions } from '@/hooks/useThemeOptions'
import type { Periodicity } from '@/api/analyticsApi'
import { useArticleCreateModalSubmit } from './ArticleCreateModal.hooks'
import { useArticleCreateModalStore } from './ArticleCreateModal.store'
import styles from './ArticleCreateModal.module.css'

interface ArticleCreateModalProps {
  onClose: () => void
  onCreated: () => void
}

const MAX_SUBTITLE = 340
const MAX_BODY = 20000
const MAX_THEMES = 3

export function ArticleCreateModalView({ onClose, onCreated }: ArticleCreateModalProps) {
  const title = useArticleCreateModalStore((state) => state.title)
  const subtitle = useArticleCreateModalStore((state) => state.subtitle)
  const body = useArticleCreateModalStore((state) => state.body)
  const author = useArticleCreateModalStore((state) => state.author)
  const periodicity = useArticleCreateModalStore((state) => state.periodicity)
  const themeIds = useArticleCreateModalStore((state) => state.themeIds)
  const setTitle = useArticleCreateModalStore((state) => state.setTitle)
  const setSubtitle = useArticleCreateModalStore((state) => state.setSubtitle)
  const setBody = useArticleCreateModalStore((state) => state.setBody)
  const setAuthor = useArticleCreateModalStore((state) => state.setAuthor)
  const setPeriodicity = useArticleCreateModalStore((state) => state.setPeriodicity)
  const setThemeIds = useArticleCreateModalStore((state) => state.setThemeIds)
  const reset = useArticleCreateModalStore((state) => state.reset)

  const { themes } = useThemeOptions()
  const { submit, isValid, submitting, error } = useArticleCreateModalSubmit(onCreated)

  useEffect(() => {
    reset()
    // Only on mount — this is a fresh form every time the modal opens.
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [])

  const themeOptions = themes.map((theme) => ({ value: theme.id, label: theme.name }))

  return (
    <Modal
      title="Новая статья"
      onClose={onClose}
      footer={
        <>
          <Button variant="ghost" onClick={onClose}>
            Отмена
          </Button>
          <Button variant="primary" disabled={!isValid || submitting} onClick={submit}>
            {submitting ? 'Сохранение…' : 'Сохранить'}
          </Button>
        </>
      }
    >
      <div className={styles.grid}>
        <div className={styles.main}>
          <label className={styles.field}>
            <span className={styles.label}>
              Заголовок <span className={styles.required}>*</span>
            </span>
            <input
              className={styles.input}
              value={title}
              onChange={(event) => setTitle(event.target.value)}
              placeholder="Введите заголовок статьи"
            />
          </label>

          <label className={styles.field}>
            <div className={styles.labelRow}>
              <span className={styles.label}>
                Подзаголовок <span className={styles.required}>*</span>
              </span>
              <span className={styles.counter}>
                {subtitle.length}/{MAX_SUBTITLE}
              </span>
            </div>
            <textarea
              className={styles.textarea}
              rows={3}
              maxLength={MAX_SUBTITLE}
              value={subtitle}
              onChange={(event) => setSubtitle(event.target.value)}
              placeholder="Краткое содержание или основные тезисы статьи"
            />
          </label>

          <label className={styles.field}>
            <div className={styles.labelRow}>
              <span className={styles.label}>
                Текст статьи <span className={styles.required}>*</span>
              </span>
              <span className={styles.counter}>
                {body.length}/{MAX_BODY}
              </span>
            </div>
            <textarea
              className={styles.bodyTextarea}
              maxLength={MAX_BODY}
              value={body}
              onChange={(event) => setBody(event.target.value)}
              placeholder="Начните писать статью…"
            />
          </label>
        </div>

        <aside className={styles.sidebar}>
          <h3 className={styles.sidebarTitle}>Атрибуты статьи</h3>

          <label className={styles.field}>
            <div className={styles.labelRow}>
              <span className={styles.label}>Темы</span>
              <span className={styles.counter}>
                {themeIds.length}/{MAX_THEMES}
              </span>
            </div>
            <MultiSelect
              options={themeOptions}
              value={themeIds}
              onChange={setThemeIds}
              maxSelected={MAX_THEMES}
              placeholder="Найти или добавить тему"
            />
          </label>

          <label className={styles.field}>
            <span className={styles.label}>
              Периодичность <span className={styles.required}>*</span>
            </span>
            <Select
              options={PERIODICITY_OPTIONS}
              value={periodicity}
              onChange={(value) => setPeriodicity(value as Periodicity | null)}
              placeholder="Выберите периодичность"
              searchable={false}
            />
          </label>

          <label className={styles.field}>
            <span className={styles.label}>
              Автор <span className={styles.required}>*</span>
            </span>
            <input
              className={styles.input}
              value={author}
              onChange={(event) => setAuthor(event.target.value)}
              placeholder="Укажите автора"
            />
          </label>
        </aside>
      </div>

      {error && <p className={styles.error}>{error}</p>}
    </Modal>
  )
}
