<script setup>
import { ref } from 'vue';
import { useCartStore } from '../stores/cart';

const props = defineProps({
  isOpen: Boolean,
  product: Object
});

const emit = defineEmits(['close']);
const cartStore = useCartStore();
const quantity = ref(1);

function addToCart() {
  cartStore.addToCart(props.product, quantity.value);
  quantity.value = 1;
  emit('close');
}
</script>

<template>
  <Transition name="fade">
    <div v-if="isOpen" class="fixed inset-0 z-50 flex items-center justify-center p-4">
      
      <div class="absolute inset-0 bg-black/40 backdrop-blur-sm transition-opacity" @click="$emit('close')"></div>

      <div class="relative w-full max-w-2xl overflow-hidden rounded-2xl bg-white dark:bg-gray-800 shadow-2xl transform transition-all">
        
        <button @click="$emit('close')" class="absolute top-4 right-4 z-10 p-2 text-gray-500 hover:text-gray-900 dark:text-gray-400 dark:hover:text-white">
          <span class="material-symbols-outlined">close</span>
        </button>

        <div class="grid grid-cols-1 md:grid-cols-2">
          <div class="h-64 md:h-full bg-gray-100 dark:bg-gray-700">
             <img :src="product.image || '/placeholder.svg'" class="h-full w-full object-cover" />
          </div>

          <div class="p-8 flex flex-col justify-between">
            <div>
              <h2 class="text-3xl font-bold text-gray-900 dark:text-white">{{ product.name }}</h2>
              <p class="mt-4 text-gray-600 dark:text-gray-300">{{ product.description }}</p>
              <div class="mt-6 text-4xl font-black text-constructor-blue dark:text-blue-400">
                ${{ product.price.toFixed(2) }}
              </div>
            </div>

            <div class="mt-8">
              <div class="flex items-center gap-4 mb-4">
                <label class="text-sm font-medium text-gray-700 dark:text-gray-300">Quantity:</label>
                <div class="flex items-center rounded-lg border border-gray-300 dark:border-gray-600">
                  <button @click="quantity > 1 && quantity--" class="px-3 py-1 hover:bg-gray-100 dark:hover:bg-gray-700">-</button>
                  <span class="px-3 font-bold dark:text-white">{{ quantity }}</span>
                  <button @click="quantity++" class="px-3 py-1 hover:bg-gray-100 dark:hover:bg-gray-700">+</button>
                </div>
              </div>

              <button @click="addToCart" class="w-full rounded-xl bg-safety-orange py-3.5 text-center font-bold text-white shadow-lg transition-transform hover:scale-[1.02] hover:shadow-orange-500/30">
                Add to Cart
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </Transition>
</template>