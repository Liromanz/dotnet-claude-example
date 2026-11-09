export class ApiError extends Error {
  readonly status: number

  constructor(message: string, status: number) {
    super(message)
    this.status = status
  }
}

async function request<TResponse>(url: string, init?: RequestInit): Promise<TResponse> {
  const response = await fetch(url, {
    ...init,
    headers: {
      'Content-Type': 'application/json',
      ...init?.headers,
    },
  })

  if (!response.ok) {
    const problem = await response.json().catch(() => null)
    throw new ApiError(problem?.detail ?? response.statusText, response.status)
  }

  if (response.status === 204) {
    return undefined as TResponse
  }

  return (await response.json()) as TResponse
}

export function buildQuery<TParams extends object>(params: TParams): string {
  const search = new URLSearchParams()
  for (const [key, value] of Object.entries(params as Record<string, unknown>)) {
    if (value !== undefined && value !== null && value !== '') {
      search.set(key, String(value))
    }
  }
  const query = search.toString()
  return query ? `?${query}` : ''
}

export const httpClient = {
  get: <TResponse>(url: string) => request<TResponse>(url),
  post: <TResponse>(url: string, body?: unknown) =>
    request<TResponse>(url, { method: 'POST', body: body ? JSON.stringify(body) : undefined }),
  put: <TResponse>(url: string, body?: unknown) =>
    request<TResponse>(url, { method: 'PUT', body: body ? JSON.stringify(body) : undefined }),
  delete: <TResponse>(url: string) => request<TResponse>(url, { method: 'DELETE' }),
}
