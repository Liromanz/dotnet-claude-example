import { buildQuery, httpClient } from '@/api/httpClient'
import type { PagedResponse, PageQuery } from '@/api/types'

const BASE_URL = '/api/analytics'

export type Periodicity = 'Weekly' | 'Monthly' | 'Irregular'

export interface ThemeTagDto {
  id: string
  name: string
}

export interface ArticleListItemDto {
  id: string
  title: string
  subtitle: string
  author: string
  periodicity: Periodicity
  themes: ThemeTagDto[]
  viewCount: number
  publishedAtUtc: string
}

export interface ArticleDetailDto {
  id: string
  title: string
  subtitle: string
  body: string
  author: string
  periodicity: Periodicity
  themes: ThemeTagDto[]
  viewCount: number
  publishedAtUtc: string
}

export interface ThemeRefDto {
  id: string
  name: string
  status: string
}

export interface GetArticlesParams extends PageQuery {
  search?: string
  themeId?: string
  periodicity?: Periodicity
  author?: string
}

export interface CreateArticleRequest {
  title: string
  subtitle: string
  body: string
  author: string
  periodicity: Periodicity
  themeIds: string[]
}

export const analyticsApi = {
  getArticles: (params: GetArticlesParams) =>
    httpClient.get<PagedResponse<ArticleListItemDto>>(`${BASE_URL}/articles${buildQuery(params)}`),

  getArticleById: (id: string) => httpClient.get<ArticleDetailDto>(`${BASE_URL}/articles/${id}`),

  createArticle: (payload: CreateArticleRequest) =>
    httpClient.post<{ id: string }>(`${BASE_URL}/articles`, payload),

  deleteArticle: (id: string) => httpClient.delete<void>(`${BASE_URL}/articles/${id}`),

  getThemeRefs: (search?: string) =>
    httpClient.get<ThemeRefDto[]>(`${BASE_URL}/theme-refs${buildQuery({ search })}`),
}
