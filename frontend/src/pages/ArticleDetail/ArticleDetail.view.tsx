import { Link, useNavigate, useParams } from '@tanstack/react-router'
import { IconButton } from '@/components/IconButton'
import { Tag } from '@/components/Tag'
import { ViewCount } from '@/components/ViewCount'
import { ArrowLeftIcon, TrashIcon } from '@/components/icons'
import { PERIODICITY_LABELS } from '@/constants/periodicity'
import { analyticsApi } from '@/api/analyticsApi'
import { useLoadArticleDetail } from './ArticleDetail.hooks'
import { useArticleDetailStore } from './ArticleDetail.store'
import styles from './ArticleDetail.module.css'

export function ArticleDetailView() {
  const { articleId } = useParams({ from: '/analytics/articles/$articleId' })
  const navigate = useNavigate()

  useLoadArticleDetail(articleId)

  const article = useArticleDetailStore((state) => state.article)
  const loading = useArticleDetailStore((state) => state.loading)
  const error = useArticleDetailStore((state) => state.error)

  async function handleDelete() {
    if (!window.confirm('Удалить статью безвозвратно?')) return
    await analyticsApi.deleteArticle(articleId)
    void navigate({ to: '/analytics/articles' })
  }

  if (loading) {
    return <div className={styles.state}>Загрузка…</div>
  }

  if (error || !article) {
    return <div className={styles.state}>{error ?? 'Статья не найдена'}</div>
  }

  return (
    <div className={styles.layout}>
      <aside className={styles.infoPanel}>
        <h3 className={styles.infoTitle}>Информация о статье</h3>

        <div className={styles.infoRow}>
          <span className={styles.infoLabel}>Дата публикации</span>
          <span className={styles.infoValue}>
            {new Date(article.publishedAtUtc).toLocaleString('ru-RU')}
          </span>
        </div>

        <div className={styles.infoRow}>
          <span className={styles.infoLabel}>Темы</span>
          <div className={styles.infoTags}>
            {article.themes.length === 0 && <span className={styles.infoValue}>—</span>}
            {article.themes.map((theme) => (
              <Tag key={theme.id}>{theme.name}</Tag>
            ))}
          </div>
        </div>

        <div className={styles.infoRow}>
          <span className={styles.infoLabel}>Периодичность</span>
          <span className={styles.infoValue}>{PERIODICITY_LABELS[article.periodicity]}</span>
        </div>

        <div className={styles.infoRow}>
          <span className={styles.infoLabel}>Автор</span>
          <span className={styles.infoValue}>{article.author}</span>
        </div>

        <div className={styles.infoRow}>
          <span className={styles.infoLabel}>Просмотры</span>
          <ViewCount count={article.viewCount} />
        </div>
      </aside>

      <section className={styles.content}>
        <div className={styles.topRow}>
          <Link to="/analytics/articles" className={styles.backLink}>
            <ArrowLeftIcon /> Все статьи
          </Link>
          <IconButton aria-label="Удалить статью" onClick={handleDelete}>
            <TrashIcon />
          </IconButton>
        </div>

        <h1 className={styles.title}>{article.title}</h1>
        <p className={styles.subtitle}>{article.subtitle}</p>

        <div className={styles.divider} />

        <div className={styles.body}>
          {article.body.split('\n').map((paragraph, index) => (
            // Body is plain text; this is the simplest stable key for a static split.
            // eslint-disable-next-line react/no-array-index-key
            <p key={index}>{paragraph}</p>
          ))}
        </div>
      </section>
    </div>
  )
}
