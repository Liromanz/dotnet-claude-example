import { useMemo, useState } from 'react'
import { dictionariesApi, type ThemeStatus } from '@/api/dictionariesApi'
import { useDebouncedValue } from '@/hooks/useDebouncedValue'
import { useServerTable } from '@/hooks/useServerTable'
import { useThemesStore } from './Themes.store'

export function useThemesTable() {
  const [idFilter, setIdFilter] = useState('')
  const [nameFilter, setNameFilter] = useState('')
  const [authorFilter, setAuthorFilter] = useState('')
  const [editorFilter, setEditorFilter] = useState('')
  const [statusFilter, setStatusFilter] = useState<ThemeStatus | null>(null)

  const debouncedId = useDebouncedValue(idFilter)
  const debouncedName = useDebouncedValue(nameFilter)
  const debouncedAuthor = useDebouncedValue(authorFilter)
  const debouncedEditor = useDebouncedValue(editorFilter)

  const filters = useMemo(
    () => ({
      id: debouncedId || undefined,
      name: debouncedName || undefined,
      author: debouncedAuthor || undefined,
      editor: debouncedEditor || undefined,
      status: statusFilter ?? undefined,
    }),
    [debouncedId, debouncedName, debouncedAuthor, debouncedEditor, statusFilter],
  )

  const table = useServerTable({
    fetchPage: dictionariesApi.getThemes,
    filters,
    initialPageSize: 20,
    initialSortBy: 'createdAtUtc',
    initialSortDirection: 'Descending',
  })

  return {
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
  }
}

export function useThemeActions(refetch: () => void) {
  const form = useThemesStore((state) => state.form)
  const editingThemeId = useThemesStore((state) => state.editingThemeId)
  const selectedIds = useThemesStore((state) => state.selectedIds)
  const setSubmitting = useThemesStore((state) => state.setSubmitting)
  const setError = useThemesStore((state) => state.setError)
  const closeModal = useThemesStore((state) => state.closeModal)
  const clearSelection = useThemesStore((state) => state.clearSelection)

  async function submitCreate() {
    if (!form.name.trim() || !form.author.trim()) {
      setError('Заполните обязательные поля.')
      return
    }
    setSubmitting(true)
    try {
      await dictionariesApi.createTheme({ name: form.name, author: form.author })
      closeModal()
      refetch()
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Не удалось создать тему.')
    } finally {
      setSubmitting(false)
    }
  }

  async function submitEdit() {
    if (!editingThemeId || !form.name.trim() || !form.editor.trim()) {
      setError('Заполните обязательные поля.')
      return
    }
    setSubmitting(true)
    try {
      await dictionariesApi.updateTheme(editingThemeId, { name: form.name, editor: form.editor })
      closeModal()
      refetch()
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Не удалось обновить тему.')
    } finally {
      setSubmitting(false)
    }
  }

  async function submitApprove() {
    if (!form.editor.trim()) {
      setError('Укажите редактора.')
      return
    }
    setSubmitting(true)
    try {
      await Promise.all(Array.from(selectedIds).map((id) => dictionariesApi.approveTheme(id, { editor: form.editor })))
      closeModal()
      clearSelection()
      refetch()
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Не удалось утвердить темы.')
    } finally {
      setSubmitting(false)
    }
  }

  async function deleteSelected() {
    if (selectedIds.size === 0) return
    if (!window.confirm(`Удалить ${selectedIds.size} тем(у/ы) безвозвратно?`)) return
    await Promise.all(Array.from(selectedIds).map((id) => dictionariesApi.deleteTheme(id)))
    clearSelection()
    refetch()
  }

  return { submitCreate, submitEdit, submitApprove, deleteSelected }
}
