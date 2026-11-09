import { Link } from '@tanstack/react-router'
import { Badge } from '@/components/Badge'
import { Button } from '@/components/Button'
import { DataTable, type DataTableColumn } from '@/components/DataTable'
import { IconButton } from '@/components/IconButton'
import { Modal } from '@/components/Modal'
import { Pagination } from '@/components/Pagination'
import { Select } from '@/components/Select'
import { SideNavPanel } from '@/components/SideNavPanel'
import { ArrowLeftIcon, PencilIcon, PlusIcon, TrashIcon } from '@/components/icons'
import { SIDE_NAV_ENTRIES } from '@/directories'
import { THEME_STATUS_LABELS, THEME_STATUS_OPTIONS } from '@/constants/themeStatus'
import type { ThemeDto } from '@/api/dictionariesApi'
import { useThemeActions, useThemesTable } from './Themes.hooks'
import { useThemesStore } from './Themes.store'
import styles from './Themes.module.css'

export function ThemesView() {
  const {
    table,
    idFilter,
    setIdFilter,
    nameFilter,
    setNameFilter,
    authorFilter,
    setAuthorFilter,
    editorFilter,
    setEditorFilter,
    statusFilter,
    setStatusFilter,
  } = useThemesTable()

  const selectedIds = useThemesStore((state) => state.selectedIds)
  const toggleSelect = useThemesStore((state) => state.toggleSelect)
  const toggleSelectAll = useThemesStore((state) => state.toggleSelectAll)
  const modal = useThemesStore((state) => state.modal)
  const form = useThemesStore((state) => state.form)
  const submitting = useThemesStore((state) => state.submitting)
  const error = useThemesStore((state) => state.error)
  const setFormField = useThemesStore((state) => state.setFormField)
  const openCreateModal = useThemesStore((state) => state.openCreateModal)
  const openEditModal = useThemesStore((state) => state.openEditModal)
  const openApproveModal = useThemesStore((state) => state.openApproveModal)
  const closeModal = useThemesStore((state) => state.closeModal)

  const { submitCreate, submitEdit, submitApprove, deleteSelected } = useThemeActions(table.refetch)

  const selectedArray = Array.from(selectedIds)
  const singleSelectedTheme =
    selectedArray.length === 1 ? table.items.find((theme) => theme.id === selectedArray[0]) : undefined

  const tableColumns: DataTableColumn<ThemeDto>[] = [
    {
      key: 'id',
      header: 'ID темы',
      width: '240px',
      render: (theme) => <span className={styles.mono}>{theme.id}</span>,
      filter: (
        <input
          className={styles.filterInput}
          value={idFilter}
          onChange={(event) => setIdFilter(event.target.value)}
          placeholder="Введите ID"
        />
      ),
    },
    {
      key: 'name',
      header: 'Наименование',
      sortable: true,
      render: (theme) => theme.name,
      filter: (
        <input
          className={styles.filterInput}
          value={nameFilter}
          onChange={(event) => setNameFilter(event.target.value)}
          placeholder="Введите наименование"
        />
      ),
    },
    {
      key: 'author',
      header: 'Автор',
      sortable: true,
      render: (theme) => theme.author,
      filter: (
        <input
          className={styles.filterInput}
          value={authorFilter}
          onChange={(event) => setAuthorFilter(event.target.value)}
          placeholder="Все поля"
        />
      ),
    },
    {
      key: 'editor',
      header: 'Редактор',
      sortable: true,
      render: (theme) => theme.editor ?? '—',
      filter: (
        <input
          className={styles.filterInput}
          value={editorFilter}
          onChange={(event) => setEditorFilter(event.target.value)}
          placeholder="Все поля"
        />
      ),
    },
    {
      key: 'status',
      header: 'Статус',
      sortable: true,
      render: (theme) => (
        <Badge tone={theme.status === 'Approved' ? 'approved' : 'draft'}>{THEME_STATUS_LABELS[theme.status]}</Badge>
      ),
      filter: (
        <Select
          options={THEME_STATUS_OPTIONS}
          value={statusFilter}
          onChange={(value) => setStatusFilter(value as typeof statusFilter)}
          placeholder="Все статусы"
          searchable={false}
        />
      ),
    },
    {
      key: 'createdAtUtc',
      header: 'Дата создания',
      sortable: true,
      render: (theme) => new Date(theme.createdAtUtc).toLocaleDateString('ru-RU'),
    },
    {
      key: 'updatedAtUtc',
      header: 'Дата изменения',
      sortable: true,
      render: (theme) => new Date(theme.updatedAtUtc).toLocaleDateString('ru-RU'),
    },
  ]

  return (
    <div className={styles.layout}>
      <aside className={styles.sidebar}>
        <SideNavPanel title="Темы" entries={SIDE_NAV_ENTRIES.dictionaries} />
      </aside>

      <section className={styles.content}>
        <Link to="/analytics/articles" className={styles.backLink}>
          <ArrowLeftIcon /> К аналитике
        </Link>

        <div className={styles.contentHeader}>
          <h1 className={styles.title}>Справочник тем</h1>
          <div className={styles.toolbar}>
            <Button variant="primary" icon={<PlusIcon />} onClick={openCreateModal}>
              Добавить
            </Button>
            <IconButton
              aria-label="Редактировать"
              disabled={!singleSelectedTheme}
              onClick={() => singleSelectedTheme && openEditModal(singleSelectedTheme)}
            >
              <PencilIcon />
            </IconButton>
            <IconButton aria-label="Удалить" disabled={selectedIds.size === 0} onClick={deleteSelected}>
              <TrashIcon />
            </IconButton>
          </div>
        </div>

        <p className={styles.hint}>
          Для редактирования выберите одну тему. Выберите темы для удаления. Выберите черновики для утверждения.
        </p>

        <DataTable
          columns={tableColumns}
          rows={table.items}
          rowKey={(theme) => theme.id}
          sortBy={table.sortBy}
          sortDirection={table.sortDirection}
          onSort={table.toggleSort}
          selectable
          selectedKeys={selectedIds}
          onToggleSelect={toggleSelect}
          onToggleSelectAll={() => toggleSelectAll(table.items.map((theme) => theme.id))}
          loading={table.loading}
        />

        <div className={styles.footer}>
          <Pagination
            page={table.page}
            pageSize={table.pageSize}
            totalCount={table.totalCount}
            onPageChange={table.setPage}
            onPageSizeChange={table.setPageSize}
          />
          <div className={styles.footerRight}>
            <span className={styles.selectedCount}>Выбрано: {selectedIds.size}</span>
            <Button variant="primary" disabled={selectedIds.size === 0} onClick={openApproveModal}>
              Утвердить
            </Button>
          </div>
        </div>
      </section>

      {modal === 'create' && (
        <Modal
          title="Новая тема"
          width={480}
          onClose={closeModal}
          footer={
            <>
              <Button variant="ghost" onClick={closeModal}>
                Отмена
              </Button>
              <Button variant="primary" disabled={submitting} onClick={submitCreate}>
                Сохранить
              </Button>
            </>
          }
        >
          <ThemeForm
            nameValue={form.name}
            onNameChange={(value) => setFormField('name', value)}
            authorValue={form.author}
            onAuthorChange={(value) => setFormField('author', value)}
          />
          {error && <p className={styles.error}>{error}</p>}
        </Modal>
      )}

      {modal === 'edit' && (
        <Modal
          title="Редактирование темы"
          width={480}
          onClose={closeModal}
          footer={
            <>
              <Button variant="ghost" onClick={closeModal}>
                Отмена
              </Button>
              <Button variant="primary" disabled={submitting} onClick={submitEdit}>
                Сохранить
              </Button>
            </>
          }
        >
          <label className={styles.field}>
            <span className={styles.label}>Наименование</span>
            <input
              className={styles.input}
              value={form.name}
              onChange={(event) => setFormField('name', event.target.value)}
            />
          </label>
          <label className={styles.field}>
            <span className={styles.label}>Редактор</span>
            <input
              className={styles.input}
              value={form.editor}
              onChange={(event) => setFormField('editor', event.target.value)}
              placeholder="Укажите редактора"
            />
          </label>
          {error && <p className={styles.error}>{error}</p>}
        </Modal>
      )}

      {modal === 'approve' && (
        <Modal
          title={`Утвердить темы (${selectedIds.size})`}
          width={420}
          onClose={closeModal}
          footer={
            <>
              <Button variant="ghost" onClick={closeModal}>
                Отмена
              </Button>
              <Button variant="primary" disabled={submitting} onClick={submitApprove}>
                Утвердить
              </Button>
            </>
          }
        >
          <label className={styles.field}>
            <span className={styles.label}>Редактор</span>
            <input
              className={styles.input}
              value={form.editor}
              onChange={(event) => setFormField('editor', event.target.value)}
              placeholder="Укажите редактора"
            />
          </label>
          {error && <p className={styles.error}>{error}</p>}
        </Modal>
      )}
    </div>
  )
}

interface ThemeFormProps {
  nameValue: string
  onNameChange: (value: string) => void
  authorValue: string
  onAuthorChange: (value: string) => void
}

function ThemeForm({ nameValue, onNameChange, authorValue, onAuthorChange }: ThemeFormProps) {
  return (
    <>
      <label className={styles.field}>
        <span className={styles.label}>Наименование</span>
        <input className={styles.input} value={nameValue} onChange={(event) => onNameChange(event.target.value)} />
      </label>
      <label className={styles.field}>
        <span className={styles.label}>Автор</span>
        <input className={styles.input} value={authorValue} onChange={(event) => onAuthorChange(event.target.value)} />
      </label>
    </>
  )
}
