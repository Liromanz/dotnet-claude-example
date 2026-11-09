import path from 'node:path'
import react from '@vitejs/plugin-react'
import { defineConfig } from 'vite'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      '@': path.resolve(import.meta.dirname, 'src'),
    },
  },
  server: {
    port: 5173,
    proxy: {
      // Frontend never talks cross-origin in dev: each microservice is proxied under its
      // own prefix so the browser only ever sees one origin.
      '/api/dictionaries': {
        target: 'http://localhost:5081',
        changeOrigin: true,
        rewrite: (requestPath) => requestPath.replace(/^\/api\/dictionaries/, '/api'),
      },
      '/api/analytics': {
        target: 'http://localhost:5082',
        changeOrigin: true,
        rewrite: (requestPath) => requestPath.replace(/^\/api\/analytics/, '/api'),
      },
    },
  },
})
