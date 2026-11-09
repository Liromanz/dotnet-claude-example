import { buildQuery, httpClient } from '@/api/httpClient'
import type { PagedResponse, PageQuery } from '@/api/types'

const BASE_URL = '/api/dictionaries'

export type ThemeStatus = 'Draft' | 'Approved'

export interface ThemeDto {
  id: string
  name: string
  status: ThemeStatus
  author: string
  editor: string | null
  createdAtUtc: string
  updatedAtUtc: string
}

export interface GetThemesParams extends PageQuery {
  id?: string
  name?: string
  author?: string
  editor?: string
  status?: ThemeStatus
}

export interface CreateThemeRequest {
  name: string
  author: string
}

export interface UpdateThemeRequest {
  name: string
  editor: string
}

export interface ApproveThemeRequest {
  editor: string
}

export const dictionariesApi = {
  getThemes: (params: GetThemesParams) =>
    httpClient.get<PagedResponse<ThemeDto>>(`${BASE_URL}/themes${buildQuery(params)}`),

  createTheme: (payload: CreateThemeRequest) =>
    httpClient.post<{ id: string }>(`${BASE_URL}/themes`, payload),

  updateTheme: (id: string, payload: UpdateThemeRequest) =>
    httpClient.put<void>(`${BASE_URL}/themes/${id}`, payload),

  approveTheme: (id: string, payload: ApproveThemeRequest) =>
    httpClient.post<void>(`${BASE_URL}/themes/${id}/approve`, payload),

  deleteTheme: (id: string) => httpClient.delete<void>(`${BASE_URL}/themes/${id}`),
}
