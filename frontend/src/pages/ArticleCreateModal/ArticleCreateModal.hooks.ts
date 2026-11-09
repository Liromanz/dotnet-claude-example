import { analyticsApi } from '@/api/analyticsApi'
import { useArticleCreateModalStore } from './ArticleCreateModal.store'

export function useArticleCreateModalSubmit(onCreated: () => void) {
  const state = useArticleCreateModalStore()

  const isValid = state.title.trim() !== '' && state.author.trim() !== '' && state.periodicity !== null

  async function submit() {
    if (!isValid || state.periodicity === null) {
      state.setError('Заполните обязательные поля.')
      return
    }

    state.setSubmitting(true)
    state.setError(null)

    try {
      await analyticsApi.createArticle({
        title: state.title,
        subtitle: state.subtitle,
        body: state.body,
        author: state.author,
        periodicity: state.periodicity,
        themeIds: state.themeIds,
      })
      onCreated()
    } catch (err) {
      state.setError(err instanceof Error ? err.message : 'Не удалось создать статью.')
    } finally {
      state.setSubmitting(false)
    }
  }

  return { submit, isValid, submitting: state.submitting, error: state.error }
}
