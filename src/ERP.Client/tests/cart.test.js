import { setActivePinia, createPinia } from 'pinia'
import { describe, it, expect, beforeEach } from 'vitest'
import { useCartStore } from '../src/stores/cart' 

describe('Cart Store', () => {
  beforeEach(() => {
    // Reiniciar Pinia antes de cada test
    setActivePinia(createPinia())
  })

  it('adds items to the cart', () => {
    const cart = useCartStore()
    const product = { id: 1, name: 'Drill', price: 100 }

    cart.addToCart(product, 1)

    expect(cart.items.length).toBe(1)
    expect(cart.items[0].id).toBe(1)
  })

  it('calculates total price correctly', () => {
    const cart = useCartStore()
    
    // 2 Taladros ($100 c/u) + 1 Casco ($50)
    cart.addToCart({ id: 1, price: 100 }, 2)
    cart.addToCart({ id: 2, price: 50 }, 1)

    // Total esperado: 250
    expect(cart.totalPrice).toBe(250)
  })
})