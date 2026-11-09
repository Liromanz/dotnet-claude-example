import { useEffect, useState } from 'react'
import { Link } from '@tanstack/react-router'
import { Button } from '@/components/Button'
import { SearchInput } from '@/components/SearchInput'
import { Select } from '@/components/Select'
import { SideNavPanel } from '@/components/SideNavPanel'
import { Tag } from '@/components/Tag'
import { ViewCount } from '@/components/ViewCount'
import { ArrowRightIcon, PlusIcon } from '@/components/icons'
import { SIDE_NAV_ENTRIES } from '@/directories'
import { PERIODICITY_OPTIONS, PERIODICITY_LABELS } from '@/constants/periodicity'
import { useDebouncedValue } from '@/hooks/useDebouncedValue'
import { useThemeOptions } from '@/hooks/useThemeOptions'
import { ArticleCreateModal } from '@/pages/ArticleCreateModal'
import type { Periodicity } from '@/api/analyticsApi'
import { useArticlesListTable } from './ArticlesList.hooks'
import { useArticlesListStore } from './ArticlesList.store'
import styles from './ArticlesList.module.css'

export function ArticlesListView() {
  const filters = useArticlesListStore((state) => state.filters)
  const setSearch = useArticlesListStore((state) => state.setSearch)
  const applyFilters = useArticlesListStore((state) => state.applyFilters)
  const resetFilters = useArticlesListStore((state) => state.resetFilters)
  const isCreateModalOpen = useArticlesListStore((state) => state.isCreateModalOpen)
  const openCreateModal = useArticlesListStore((state) => state.openCreateModal)
  const closeCreateModal = useArticlesListStore((state) => state.closeCreateModal)

  const [searchDraft, setSearchDraft] = useState(filters.search)
  const debouncedSearch = useDebouncedValue(searchDraft)

  const [draftThemeId, setDraftThemeId] = useState(filters.themeId)
  const [draftPeriodicity, setDraftPeriodicity] = useState(filters.periodicity)
  const [draftAuthor, setDraftAuthor] = useState(filters.author)

  const { themes } = useThemeOptions()
  const table = useArticlesListTable()

  useEffect(() => {
    setSearch(debouncedSearch)
  }, [debouncedSearch, setSearch])

  const themeOptions = themes.map((theme) => ({ value: theme.id, label: theme.name }))

  function handleApply() {
    applyFilters({ themeId: draftThemeId, periodicity: draftPeriodicity, author: draftAuthor })
  }

  function handleReset() {
    setDraftThemeId(null)
    setDraftPeriodicity(null)
    setDraftAuthor(null)
    resetFilters()
  }

  return (
    <div className={styles.layout}>
      <aside className={styles.sidebar}>
        <SideNavPanel title="Разделы" entries={SIDE_NAV_ENTRIES.analytics} />

        <section className={styles.filtersPanel}>
          <h3 className={styles.filtersTitle}>Фильтры</h3>

          <label className={styles.filterField}>
            <span className={styles.filterLabel}>Темы</span>
            <Select
              options={themeOptions}
              value={draftThemeId}
              onChange={setDraftThemeId}
              placeholder="Найти или выбрать"
            />
          </label>

          <label className={styles.filterField}>
            <span className={styles.filterLabel}>Периодичность</span>
            <Select
              options={PERIODICITY_OPTIONS}
              value={draftPeriodicity}
              onChange={(value) => setDraftPeriodicity(value as Periodicity | null)}
              placeholder="Найти или выбрать"
              searchable={false}
            />
          </label>

          <label className={styles.filterField}>
            <span className={styles.filterLabel}>Авторы</span>
            <input
              className={styles.filterInput}
              value={draftAuthor ?? ''}
              onChange={(event) => setDraftAuthor(event.target.value || null)}
              placeholder="Найти или выбрать"
            />
          </label>

          <div className={styles.filterActions}>
            <Button variant="ghost" onClick={handleReset}>
              Сбросить
            </Button>
            <Button variant="secondary" onClick={handleApply}>
              Применить
            </Button>
          </div>
        </section>
      </aside>

      <section className={styles.content}>
        <div className={styles.contentHeader}>
          <h1 className={styles.title}>Статьи</h1>
          <Button variant="primary" icon={<PlusIcon />} onClick={openCreateModal}>
            Новая статья
          </Button>
        </div>

        <SearchInput
          placeholder="Поиск по материалам"
          value={searchDraft}
          onChange={(event) => setSearchDraft(event.target.value)}
        />

        <ul className={styles.cardList}>
          {table.items.map((article) => (
            <li key={article.id} className={styles.card}>
              <div className={styles.cardMeta}>
                {article.author} · {new Date(article.publishedAtUtc).toLocaleDateString('ru-RU')} ·{' '}
                {PERIODICITY_LABELS[article.periodicity]}
              </div>
              <h2 className={styles.cardTitle}>{article.title}</h2>
              <p className={styles.cardSubtitle}>{article.subtitle}</p>

              <div className={styles.cardFooter}>
                <div className={styles.cardTags}>
                  {article.themes.map((theme) => (
                    <Tag key={theme.id}>{theme.name}</Tag>
                  ))}
                </div>
                <div className={styles.cardStats}>
                  <ViewCount count={article.viewCount} />
                  <Link to="/analytics/articles/$articleId" params={{ articleId: article.id }} className={styles.detailLink}>
                    Подробнее <ArrowRightIcon />
                  </Link>
                </div>
              </div>
            </li>
          ))}

          {!table.loading && table.items.length === 0 && (
            <li className={styles.empty}>Ничего не найдено</li>
          )}
        </ul>
      </section>

      {isCreateModalOpen && (
        <ArticleCreateModal
          onClose={closeCreateModal}
          onCreated={() => {
            closeCreateModal()
            table.refetch()
          }}
        />
      )}
    </div>
  )
}
