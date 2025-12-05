<script setup>
import { onMounted, ref } from 'vue';
import MainLayout from '../layouts/MainLayout.vue';
import ProductCard from '../components/ProductCard.vue';
import ProductModal from '../components/ProductModal.vue';
import { useCartStore } from '../stores/cart';
import api from '../api/axios';

const products = ref([]);
const isLoading = ref(true);
const isModalOpen = ref(false);
const selectedProduct = ref({});
const cartStore = useCartStore();

const constructionImages = [
  "https://images.unsplash.com/photo-1504148455328-c376907d081c?auto=format&fit=crop&q=80&w=800",
  "https://images.unsplash.com/photo-1584715653473-71069a99c15d?auto=format&fit=crop&q=80&w=800",
  "https://images.unsplash.com/photo-1581092795360-fd1ca04f0952?auto=format&fit=crop&q=80&w=800",
  "https://images.unsplash.com/photo-1530124566582-a618bc2615dc?auto=format&fit=crop&q=80&w=800",
  "https://images.unsplash.com/photo-1612469223343-5e751751288f?auto=format&fit=crop&q=80&w=800"
];

async function fetchProducts() {
  try {
    const response = await api.get('/Products?page=1&pageSize=100'); 
    
  
    products.value = response.data.items.map((p, index) => ({
        ...p,
        image: constructionImages[index % constructionImages.length] 
    }));

  } catch (error) { 
    console.error("Error load products:", error);
  } finally {
    isLoading.value = false;
  }
}

function openModal(product) {
  selectedProduct.value = product;
  isModalOpen.value = true;
}

onMounted(() => {
  fetchProducts();
})
</script>

<template>
  <MainLayout>
    <div class="mb-10 text-center">
      <h1 class="text-4xl font-black text-gray-900 dark:text-white drop-shadow-sm">
        Our Catalog
      </h1>
      <p class="mt-2 text-lg text-gray-500 dark:text-gray-400">
        Premium materials for your construction needs.
      </p>
    </div>

    <div v-if="isLoading" class="flex justify-center py-20">
       <span class="material-symbols-outlined text-4xl animate-spin text-constructor-blue">progress_activity</span>
    </div>

    <div v-else class="grid grid-cols-1 gap-8 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
      <ProductCard 
        v-for="p in products" 
        :key="p.id" 
        :product="p" 
        @open-modal="openModal"
        @add-to-cart="(prod) => cartStore.addToCart(prod)"
      />
    </div>

    <ProductModal 
      :isOpen="isModalOpen" 
      :product="selectedProduct" 
      @close="isModalOpen = false" 
    />
  </MainLayout>
</template>