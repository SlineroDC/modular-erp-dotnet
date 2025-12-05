import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import api from '../api/axios'
import { useAuthStore } from './auth'

export const useCartStore = defineStore('cart', () => {
  const items = ref([])
  const isOpen = ref(false)

  // Computed totals
  const totalItems = computed(() => items.value.reduce((acc, item) => acc + item.quantity, 0))
  const totalPrice = computed(() => items.value.reduce((acc, item) => acc + (item.price * item.quantity), 0))

  // Add (or increase if exists)
  function addToCart(product, quantity = 1) {
    const existing = items.value.find(i => i.id === product.id)
    if (existing) {
      existing.quantity += quantity
    } else {
      // Save name, price and image to show them in the cart
      items.value.push({ 
        id: product.id,
        name: product.name,
        price: product.price,
        image: product.image, 
        quantity 
      })
    }
  }

  // 🟢 NEW: Increase unit
  function increaseItem(id) {
    const item = items.value.find(i => i.id === id)
    if (item) item.quantity++
  }

  // 🔴 NEW: Decrease unit (removes if reaches 0)
  function decreaseItem(id) {
    const item = items.value.find(i => i.id === id)
    if (item) {
      item.quantity--
      if (item.quantity <= 0) {
        removeFromCart(id)
      }
    }
  }

  // Remove entire item
  function removeFromCart(id) {
    items.value = items.value.filter(i => i.id !== id)
  }

  function clearCart() {
    items.value = []
  }

  // Checkout (Your existing logic)
  async function checkout() {
    const authStore = useAuthStore()
    if (!authStore.isAuthenticated) {
      alert("Please log in to checkout")
      return false
    }
    if (items.value.length === 0) return false

    try {
      const saleData = {
        details: items.value.map(item => ({
          productId: item.id,
          quantity: item.quantity
        }))
      }
      const response = await api.post('/Sales', saleData)
      if (response.status === 200) {
        clearCart()
        isOpen.value = false
        return true
      }
    } catch (error) {
      console.error("Checkout error:", error)
      alert("Error processing order: " + (error.response?.data || error.message))
      return false
    }
  }

  return { 
    items, 
    isOpen, 
    totalItems, 
    totalPrice, 
    addToCart, 
    removeFromCart, 
    increaseItem, 
    decreaseItem,
    clearCart, 
    checkout 
  }
})