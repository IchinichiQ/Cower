import {defineConfig} from 'vite'
import react from '@vitejs/plugin-react'
import {fileURLToPath} from "node:url";

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: [
      {find: '@', replacement: fileURLToPath(new URL('./src', import.meta.url))},
    ]
  },
  server: {
    proxy: {
      '/api': {
        target: 'https://185.233.187.57:8081',
        changeOrigin: true,
        secure: false,
        // rewrite: (path) => path.replace(/^\/api/, ''),
      }
    }
  }
})
