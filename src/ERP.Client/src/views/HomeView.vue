<script setup>
import { ref } from 'vue';
import MainLayout from '../layouts/MainLayout.vue';
import ProductCard from '../components/ProductCard.vue';
import ProductModal from '../components/ProductModal.vue';
import { useCartStore } from '../stores/cart';

// Datos de ejemplo (Mock Data) - Luego conectaremos esto a tu API .NET
const products = ref([
  { 
    id: 1, 
    name: "Industrial Drill 20V", 
    price: 150.00, 
    stock: 25, 
    description: "High power cordless drill for concrete and metal.", 
    image: "https://images.unsplash.com/photo-1504148455328-c376907d081c?auto=format&fit=crop&q=80&w=800",
    images: [
        "https://images.unsplash.com/photo-1504148455328-c376907d081c?auto=format&fit=crop&q=80&w=800",
        "https://images.unsplash.com/photo-1622033879291-6004c3820a72?auto=format&fit=crop&q=80&w=800" 
    ]
  },
  { 
    id: 2, 
    name: "Safety Helmet", 
    price: 25.00, 
    stock: 5, 
    description: "Yellow protective helmet, impact resistant.", 
    image: "https://images.unsplash.com/photo-1584715653473-71069a99c15d?auto=format&fit=crop&q=80&w=800" 
  },
  { 
    id: 3, 
    name: "Concrete Mixer", 
    price: 450.00, 
    stock: 0, 
    description: "Portable cement mixer for small to medium projects.", 
    image: "https://images.unsplash.com/photo-1581092795360-fd1ca04f0952?auto=format&fit=crop&q=80&w=800" 
  },
  { 
    id: 4, 
    name: "Tool Set Pro", 
    price: 89.99, 
    stock: 50, 
    description: "Complete set of wrenches, screwdrivers and pliers.", 
    image: "https://images.unsplash.com/photo-1530124566582-a618bc2615dc?auto=format&fit=crop&q=80&w=800" 
  },
  { 
    id: 5, 
    name: "Work Gloves", 
    price: 12.50, 
    stock: 200, 
    description: "Durable leather gloves for heavy duty work.", 
    image: "https://images.unsplash.com/photo-1612469223343-5e751751288f?auto=format&fit=crop&q=80&w=800" 
  }
]);

const isModalOpen = ref(false);
const selectedProduct = ref({});
const cartStore = useCartStore();

function openModal(product) {
  selectedProduct.value = product;
  isModalOpen.value = true;
}
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