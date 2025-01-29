import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import OrderManagement from './Pages/OrderManagement'

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <OrderManagement />
  </StrictMode>,
)
