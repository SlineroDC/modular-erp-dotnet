<script setup>
import { ref, onMounted } from 'vue'; 
import { useCartStore } from '../stores/cart';
import SupportModal from '../components/SupportModal.vue';

const cartStore = useCartStore();
const isDark = ref(false);
const isSupportOpen = ref(false);

function toggleTheme() {
  isDark.value = !isDark.value;
  updateThemeClass();
}

function updateThemeClass() {
  if (isDark.value) {
    document.documentElement.classList.add('dark');
  } else {
    document.documentElement.classList.remove('dark');
  }
}

onMounted(() => {
  if (window.matchMedia('(prefers-color-scheme: dark)').matches) {
    isDark.value = true;
    updateThemeClass();
  }
});
</script>

<template>
  <div class="min-h-screen bg-gray-50 dark:bg-[#0a0a0a] text-gray-900 dark:text-gray-100 flex flex-col relative overflow-x-hidden transition-colors duration-500">
    
    <div class="fixed top-0 left-0 w-full h-full overflow-hidden -z-10 pointer-events-none">
      <div class="absolute top-[-10%] left-[-10%] w-[40%] h-[40%] rounded-full bg-constructor-blue/10 dark:bg-constructor-blue/20 blur-[120px]"></div>
      <div class="absolute bottom-[-10%] right-[-10%] w-[40%] h-[40%] rounded-full bg-safety-orange/10 dark:bg-safety-orange/10 blur-[120px]"></div>
    </div>

    <header class="sticky top-0 z-40 glass glass-dark px-6 py-4 flex justify-between items-center transition-all duration-300">
      <div class="flex items-center gap-3">
        <img src="" class="h-8 w-auto" alt="Logo" /> 
        <span class="font-bold text-xl tracking-tight text-constructor-blue dark:text-white">Firmeza</span>
      </div>
      
      <nav class="flex items-center gap-6 font-medium text-sm">
        <router-link to="/" class="hover:text-safety-orange transition-colors">Catalog</router-link>
        <router-link to="/profile" class="hover:text-safety-orange transition-colors">Profile</router-link>
        
        <button @click="toggleTheme" class="p-2 rounded-full hover:bg-black/5 dark:hover:bg-white/10 transition-colors">
          <span class="material-symbols-outlined text-xl">{{ isDark ? 'light_mode' : 'dark_mode' }}</span>
        </button>

        <button @click="cartStore.isOpen = !cartStore.isOpen" class="relative p-2 group">
          <span class="material-symbols-outlined group-hover:text-constructor-blue dark:group-hover:text-safety-orange transition-colors">shopping_cart</span>
          <span v-if="cartStore.totalItems > 0" class="absolute -top-1 -right-1 h-5 w-5 rounded-full bg-safety-orange text-xs flex items-center justify-center text-white font-bold shadow-sm">
            {{ cartStore.totalItems }}
          </span>
        </button>
      </nav>
    </header>

    <main class="flex-1 container mx-auto px-4 py-8 relative z-0">
      <slot />
    </main>

    <footer class="mt-auto glass glass-dark py-8 text-center relative z-10">
      <div class="container mx-auto">
        <p class="text-sm font-medium opacity-60">© 2025 ERP Firmeza. Crafted with precision.</p>
        <div class="mt-4 flex justify-center gap-6 text-xs font-bold tracking-wider uppercase opacity-50">
          <a href="#" class="hover:text-safety-orange hover:opacity-100 transition-all">Privacy Policy</a>
          <a href="#" class="hover:text-safety-orange hover:opacity-100 transition-all">Terms of Service</a>
        </div>
      </div>
    </footer>

    <button 
      @click="isSupportOpen = true" 
      class="fixed bottom-6 right-6 z-40 h-14 w-14 rounded-full bg-constructor-blue text-white shadow-2xl shadow-blue-900/40 flex items-center justify-center hover:scale-110 transition-all duration-300 hover:rotate-12 hover:bg-safety-orange">
      <span class="material-symbols-outlined text-3xl">support_agent</span>
    </button>

    <SupportModal 
      :isOpen="isSupportOpen" 
      @close="isSupportOpen = false" 
    />

  </div>
</template>