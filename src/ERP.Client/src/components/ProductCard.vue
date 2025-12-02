<script setup>
import { computed } from 'vue';
import GlassCard from './ui/GlassCard.vue';
import StatusBadge from './ui/StatusBadge.vue';

const props = defineProps({ product: Object });
const emit = defineEmits(['open-modal', 'add-to-cart']);

// Lógica para el badge de stock
const stockStatus = computed(() => {
  if (props.product.stock === 0) return { color: 'red', text: 'Out of Stock', pulse: false };
  if (props.product.stock < 10) return { color: 'orange', text: 'Low Stock', pulse: true };
  return { color: 'emerald', text: 'In Stock', pulse: false };
});
</script>

<template>
  <GlassCard @click="$emit('open-modal', product)" class="cursor-pointer group h-full flex flex-col">
    
    <div class="relative aspect-[4/3] overflow-hidden">
      <img :src="product.image || '/placeholder.svg'" 
           class="absolute inset-0 h-full w-full object-cover transition-transform duration-700 group-hover:scale-110" 
           alt="Product" />
      
      <div class="absolute top-4 right-4">
        <StatusBadge :color="stockStatus.color" :pulse="stockStatus.pulse">
          {{ stockStatus.text }}
        </StatusBadge>
      </div>
    </div>

    <div class="flex flex-1 flex-col p-6">
      <h3 class="text-xl font-bold text-gray-800 dark:text-white mb-2 group-hover:text-constructor-blue transition-colors">
        {{ product.name }}
      </h3>
      
      <p class="text-sm text-gray-500 dark:text-gray-400 line-clamp-2 flex-1 mb-4">
        {{ product.description }}
      </p>

      <div class="flex items-center justify-between mt-auto pt-4 border-t border-gray-100 dark:border-white/5">
        <div class="flex flex-col">
          <span class="text-xs text-gray-400 uppercase tracking-wider">Price</span>
          <span class="text-2xl font-black text-constructor-blue dark:text-blue-400">
            ${{ product.price }}
          </span>
        </div>
        
        <button @click.stop="$emit('add-to-cart', product)"
                class="bg-constructor-blue text-white p-3 rounded-xl hover:bg-safety-orange hover:shadow-lg hover:shadow-orange-500/30 hover:-translate-y-0.5 transition-all active:scale-95">
          <span class="material-symbols-outlined text-xl">add_shopping_cart</span>
        </button>
      </div>
    </div>
  </GlassCard>
</template>