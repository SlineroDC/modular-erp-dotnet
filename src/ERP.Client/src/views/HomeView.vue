<script setup>
import { onMounted, ref } from 'vue';
import MainLayout from '../layouts/MainLayout.vue';
import ProductCard from '../components/ProductCard.vue';
import ProductModal from '../components/ProductModal.vue';
import { useCartStore } from '../stores/cart';

const products = ref([]);
const isLoading = ref(true);
const isModalOpen = ref(false);
const selectedProduct = ref({});
const cartStore = useCartStore();

async function fetchProducts() {
  try {
    const response = await api.get('/Products?page=1&pageSize=100'); 
    products.value = response.data.items;
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

    <div class="grid grid-cols-1 gap-8 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
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