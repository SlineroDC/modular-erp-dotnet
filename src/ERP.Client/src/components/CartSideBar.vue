<script setup>
import { useCartStore } from '../stores/cart';

const cart = useCartStore();
</script>

<template>
  <div class="relative z-50" aria-labelledby="slide-over-title" role="dialog" aria-modal="true" v-if="cart.isOpen">
    <div class="fixed inset-0 bg-gray-500/75 transition-opacity backdrop-blur-sm" @click="cart.isOpen = false"></div>

    <div class="fixed inset-0 overflow-hidden">
      <div class="absolute inset-0 overflow-hidden">
        <div class="pointer-events-none fixed inset-y-0 right-0 flex max-w-full pl-10">
          
          <div class="pointer-events-auto w-screen max-w-md transform transition duration-500 sm:duration-700 bg-white dark:bg-gray-900 shadow-xl">
            <div class="flex h-full flex-col overflow-y-scroll bg-white dark:bg-gray-900 shadow-xl">
              <div class="flex-1 overflow-y-auto px-4 py-6 sm:px-6">
                
                <div class="flex items-start justify-between">
                  <h2 class="text-lg font-medium text-gray-900 dark:text-white">Shopping Cart</h2>
                  <button @click="cart.isOpen = false" class="text-gray-400 hover:text-gray-500">
                    <span class="material-symbols-outlined">close</span>
                  </button>
                </div>

                <div class="mt-8">
                  <div class="flow-root">
                    <ul role="list" class="-my-6 divide-y divide-gray-200 dark:divide-gray-700">
                      <li v-for="item in cart.items" :key="item.id" class="flex py-6">
                        <div class="h-24 w-24 shrink-0 overflow-hidden rounded-md border border-gray-200 dark:border-gray-700">
                          <img :src="item.image || '/placeholder.svg'" class="h-full w-full object-cover object-center">
                        </div>

                        <div class="ml-4 flex flex-1 flex-col">
                          <div>
                            <div class="flex justify-between text-base font-medium text-gray-900 dark:text-white">
                              <h3>{{ item.name }}</h3>
                              <p class="ml-4">${{ (item.price * item.quantity).toFixed(2) }}</p>
                            </div>
                            <p class="mt-1 text-sm text-gray-500 dark:text-gray-400">${{ item.price }} each</p>
                          </div>
                          <div class="flex flex-1 items-end justify-between text-sm">
                            <p class="text-gray-500 dark:text-gray-400">Qty {{ item.quantity }}</p>
                            <button @click="cart.removeFromCart(item.id)" type="button" class="font-medium text-safety-orange hover:text-red-500">Remove</button>
                          </div>
                        </div>
                      </li>
                      
                      <li v-if="cart.items.length === 0" class="py-10 text-center text-gray-500">
                        Your cart is empty.
                      </li>
                    </ul>
                  </div>
                </div>
              </div>

              <div class="border-t border-gray-200 dark:border-gray-700 px-4 py-6 sm:px-6">
                <div class="flex justify-between text-base font-medium text-gray-900 dark:text-white">
                  <p>Subtotal</p>
                  <p>${{ cart.totalPrice.toFixed(2) }}</p>
                </div>
                <p class="mt-0.5 text-sm text-gray-500 dark:text-gray-400">Shipping calculated at checkout.</p>
                <div class="mt-6">
                  <a href="#" class="flex items-center justify-center rounded-md border border-transparent bg-constructor-blue px-6 py-3 text-base font-medium text-white shadow-sm hover:bg-safety-orange transition-colors">Checkout</a>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>