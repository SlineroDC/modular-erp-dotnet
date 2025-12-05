<script setup>
import { ref } from 'vue';
import { useCartStore } from '../stores/cart';
import { useRouter } from 'vue-router';

const cart = useCartStore();
const router = useRouter();
const isProcessing = ref(false);

async function handleCheckout() {
  isProcessing.value = true;
  const success = await cart.checkout();
  isProcessing.value = false;

  if (success) {
    alert("Order placed successfully! Check your email for the receipt.");
  }
}
</script>

<template>
  <div class="relative z-50" aria-labelledby="slide-over-title" role="dialog" aria-modal="true" v-if="cart.isOpen">
    
    <div class="fixed inset-0 bg-gray-900/60 backdrop-blur-sm transition-opacity duration-500 ease-in-out" 
         @click="cart.isOpen = false"></div>

    <div class="fixed inset-0 overflow-hidden">
      <div class="absolute inset-0 overflow-hidden">
        <div class="pointer-events-none fixed inset-y-0 right-0 flex max-w-full pl-10">
          
          <div class="pointer-events-auto w-screen max-w-md transform transition duration-500 sm:duration-700 translate-x-0 bg-white dark:bg-[#0a0a0a] shadow-2xl border-l border-gray-200 dark:border-gray-800 flex flex-col h-full">
            
            <div class="flex items-center justify-between px-4 py-6 sm:px-6 border-b border-gray-200 dark:border-gray-800 bg-gray-50 dark:bg-gray-900/50">
              <h2 class="text-xl font-bold text-gray-900 dark:text-white flex items-center gap-2">
                <span class="material-symbols-outlined text-constructor-blue dark:text-blue-400">shopping_bag</span>
                Shopping Cart
              </h2>
              <button @click="cart.isOpen = false" class="text-gray-400 hover:text-gray-500 dark:hover:text-white transition-colors p-2 rounded-full hover:bg-gray-200 dark:hover:bg-gray-800">
                <span class="material-symbols-outlined">close</span>
              </button>
            </div>

            <div class="flex-1 overflow-y-auto px-4 py-6 sm:px-6">
              <div v-if="cart.items.length === 0" class="flex flex-col items-center justify-center h-full text-center opacity-50">
                <span class="material-symbols-outlined text-6xl mb-4 text-gray-300 dark:text-gray-600">remove_shopping_cart</span>
                <p class="text-gray-500 dark:text-gray-400 text-lg font-medium">Your cart is empty.</p>
                <p class="text-sm text-gray-400">Start adding items from the catalog.</p>
              </div>

              <ul v-else role="list" class="-my-6 divide-y divide-gray-200 dark:divide-gray-800">
                <li v-for="item in cart.items" :key="item.id" class="flex py-6 animate-fade-in">
                  
                  <div class="h-24 w-24 flex-shrink-0 overflow-hidden rounded-xl border border-gray-200 dark:border-gray-700 bg-white dark:bg-gray-800">
                    <img :src="item.image || '/placeholder.svg'" class="h-full w-full object-cover object-center">
                  </div>

                  <div class="ml-4 flex flex-1 flex-col justify-between">
                    <div>
                      <div class="flex justify-between text-base font-bold text-gray-900 dark:text-white">
                        <h3>{{ item.name }}</h3>
                        <p class="ml-4 text-constructor-blue dark:text-blue-400">${{ (item.price * item.quantity).toFixed(2) }}</p>
                      </div>
                      <p class="mt-1 text-sm text-gray-500 dark:text-gray-400">${{ item.price }} each</p>
                    </div>
                    
                    <div class="flex flex-1 items-end justify-between text-sm">
                      
                      <div class="flex items-center rounded-lg border border-gray-300 dark:border-gray-600 bg-gray-50 dark:bg-gray-800">
                        <button @click="cart.decreaseItem(item.id)" class="px-3 py-1 hover:bg-gray-200 dark:hover:bg-gray-700 text-gray-600 dark:text-gray-300 transition rounded-l-lg">
                          -
                        </button>
                        
                        <span class="px-2 font-bold text-gray-900 dark:text-white min-w-[1.5rem] text-center">{{ item.quantity }}</span>
                        
                        <button @click="cart.increaseItem(item.id)" class="px-3 py-1 hover:bg-gray-200 dark:hover:bg-gray-700 text-gray-600 dark:text-gray-300 transition rounded-r-lg">
                          +
                        </button>
                      </div>

                      <button @click="cart.removeFromCart(item.id)" type="button" class="flex items-center gap-1 font-medium text-red-500 hover:text-red-700 transition-colors p-1 rounded hover:bg-red-50 dark:hover:bg-red-900/20">
                        <span class="material-symbols-outlined text-lg">delete</span>
                        <span class="hidden sm:inline">Remove</span>
                      </button>
                    </div>
                  </div>
                </li>
              </ul>
            </div>

            <div class="border-t border-gray-200 dark:border-gray-800 bg-gray-50 dark:bg-gray-900/50 px-4 py-6 sm:px-6">
              <div class="flex justify-between text-lg font-bold text-gray-900 dark:text-white mb-4">
                <p>Subtotal</p>
                <p class="text-2xl">${{ cart.totalPrice.toFixed(2) }}</p>
              </div>
              
              <button 
                @click="handleCheckout"
                :disabled="isProcessing || cart.items.length === 0"
                class="flex w-full items-center justify-center rounded-xl bg-constructor-blue px-6 py-4 text-base font-bold text-white shadow-lg shadow-blue-900/20 hover:bg-safety-orange hover:shadow-orange-500/30 transition-all transform hover:-translate-y-1 disabled:opacity-50 disabled:cursor-not-allowed disabled:transform-none">
                
                <span v-if="isProcessing" class="animate-spin material-symbols-outlined mr-2">progress_activity</span>
                {{ isProcessing ? 'Processing Order...' : 'Checkout Securely' }}
              </button>
              
              <div class="mt-6 flex justify-center text-center text-sm text-gray-500 dark:text-gray-400">
                <p>
                  or 
                  <button @click="cart.isOpen = false" class="font-medium text-constructor-blue hover:text-safety-orange transition-colors">
                    Continue Shopping
                    <span aria-hidden="true"> &rarr;</span>
                  </button>
                </p>
              </div>
            </div>

          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.animate-fade-in {
  animation: fadeIn 0.3s ease-out;
}
@keyframes fadeIn {
  from { opacity: 0; transform: translateY(10px); }
  to { opacity: 1; transform: translateY(0); }
}
</style>