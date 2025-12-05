<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useCartStore } from '../stores/cart';
import { useAuthStore } from '../stores/auth';
import { useTheme } from '../composables/useTheme'; 


import CartSidebar from '../components/CartSideBar.vue';
import SupportModal from '../components/SupportModal.vue';


const cartStore = useCartStore();
const authStore = useAuthStore();
const router = useRouter();
const { isDark, toggleTheme } = useTheme(); 

const isSupportOpen = ref(false);

function handleLogout() {
  authStore.logout();
  router.push('/login');
}
</script>

<template>
  <div class="min-h-screen bg-gray-50 dark:bg-[#0a0a0a] text-gray-900 dark:text-gray-100 flex flex-col relative overflow-x-hidden transition-colors duration-500">
    
    <div class="fixed top-0 left-0 w-full h-full overflow-hidden -z-10 pointer-events-none">
      <div class="absolute top-[-10%] left-[-10%] w-[40%] h-[40%] rounded-full bg-constructor-blue/10 dark:bg-constructor-blue/20 blur-[120px] transition-colors duration-500"></div>
      <div class="absolute bottom-[-10%] right-[-10%] w-[40%] h-[40%] rounded-full bg-safety-orange/10 dark:bg-safety-orange/10 blur-[120px] transition-colors duration-500"></div>
    </div>

    <header class="sticky top-0 z-40 glass glass-dark px-6 py-4 flex justify-between items-center transition-all duration-300">
      
      <router-link to="/" class="flex items-center gap-3 group">
        <img src="/svg/logo1.png" class="h-8 w-auto group-hover:scale-110 transition-transform" alt="Logo" /> 
        <span class="font-bold text-xl tracking-tight text-constructor-blue dark:text-white">Firmeza</span>
      </router-link>
      
      <nav class="flex items-center gap-4 font-medium text-sm">
        
        <router-link to="/" class="flex items-center gap-2 px-3 py-2 rounded-lg hover:bg-black/5 dark:hover:bg-white/10 transition-colors" title="Catalog">
          <span class="material-symbols-outlined text-xl text-gray-600 dark:text-gray-300">storefront</span>
          <span class="hidden sm:block">Catalog</span>
        </router-link>

        <router-link to="/profile" class="flex items-center gap-2 px-3 py-2 rounded-lg hover:bg-black/5 dark:hover:bg-white/10 transition-colors" title="Profile">
          <span class="material-symbols-outlined text-xl text-gray-600 dark:text-gray-300">person</span>
          <span class="hidden sm:block">Profile</span>
        </router-link>
        
        <div class="h-6 w-px bg-gray-300 dark:bg-gray-700 mx-1"></div>

        <button @click="toggleTheme" class="p-2 rounded-full hover:bg-black/5 dark:hover:bg-white/10 transition-colors" title="Toggle Theme">
          <span v-if="isDark" class="material-symbols-outlined text-xl">light_mode</span>
          <span v-else class="material-symbols-outlined text-xl">dark_mode</span>
        </button>

        <button @click="handleLogout" class="p-2 rounded-full hover:bg-red-50 text-gray-600 hover:text-red-500 dark:text-gray-300 dark:hover:bg-red-900/30 transition-colors" title="Logout">
           <span class="material-symbols-outlined text-xl">logout</span>
        </button>

        <button @click="cartStore.isOpen = !cartStore.isOpen" class="relative p-2 ml-2 group">
          <div class="p-2 rounded-full bg-constructor-blue/10 group-hover:bg-constructor-blue group-hover:text-white dark:bg-white/10 transition-colors text-constructor-blue dark:text-white">
            <span class="material-symbols-outlined text-xl">shopping_cart</span>
          </div>
          <span v-if="cartStore.totalItems > 0" class="absolute top-0 right-0 h-5 w-5 rounded-full bg-safety-orange text-[10px] flex items-center justify-center text-white font-bold shadow-sm border-2 border-white dark:border-[#0a0a0a] animate-bounce">
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

    <SupportModal :isOpen="isSupportOpen" @close="isSupportOpen = false" />
    <CartSidebar /> </div>
</template>