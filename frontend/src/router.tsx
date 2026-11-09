import { createRootRoute, createRoute, createRouter, redirect } from '@tanstack/react-router'
import { AppLayout } from '@/AppLayout'
import { ArticlesListPage } from '@/pages/ArticlesList'
import { ArticleDetailPage } from '@/pages/ArticleDetail'
import { ThemesPage } from '@/pages/Themes'

const rootRoute = createRootRoute({ component: AppLayout })

const indexRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '/',
  beforeLoad: () => {
    throw redirect({ to: '/analytics/articles' })
  },
})

const articlesListRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '/analytics/articles',
  component: ArticlesListPage,
})

const articleDetailRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '/analytics/articles/$articleId',
  component: ArticleDetailPage,
})

const themesRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '/dictionaries/themes',
  component: ThemesPage,
})

const routeTree = rootRoute.addChildren([indexRoute, articlesListRoute, articleDetailRoute, themesRoute])

export const router = createRouter({ routeTree })

declare module '@tanstack/react-router' {
  interface Register {
    router: typeof router
  }
}
