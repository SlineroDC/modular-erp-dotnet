<script setup>
import { ref } from 'vue';
import { useAuthStore } from '../stores/auth';
import { useTheme } from '../composables/useTheme';

const { isDark, toggleTheme } = useTheme();

const authStore = useAuthStore();
const email = ref('');
const password = ref('');
const isLoading = ref(false);
const errorMessage = ref('');

async function handleLogin() {

  if (!email.value || !password.value) return;
  isLoading.value = true;
  errorMessage.value = '';
  const result = await authStore.login(email.value, password.value);
  if (!result.success) errorMessage.value = result.message;
  isLoading.value = false;
}
</script>

<template>
  <div class="min-h-screen flex items-center justify-center bg-gray-100 dark:bg-[#0a0a0a] relative overflow-hidden px-4 transition-colors duration-500">
       
<button @click="toggleTheme" >
   <span class="material-symbols-outlined">{{ isDark ? 'light_mode' : 'dark_mode' }}</span>
</button>

    <div class="absolute top-[-20%] left-[-10%] w-[50%] h-[50%] rounded-full bg-constructor-blue/20 blur-[120px] opacity-0 dark:opacity-100 transition-opacity duration-500"></div>
    <div class="absolute bottom-[-20%] right-[-10%] w-[50%] h-[50%] rounded-full bg-safety-orange/10 blur-[120px] opacity-0 dark:opacity-100 transition-opacity duration-500"></div>

    <div class="w-full max-w-md p-8 rounded-3xl shadow-2xl relative z-10 transition-all duration-300
                bg-white border border-gray-200
                dark:glass dark:glass-dark dark:border-white/10">
      
      <div class="text-center mb-8">
        <img src="/svg/logo1.png" class="h-12 mx-auto mb-4" alt="Logo" />
        <h1 class="text-3xl font-bold text-gray-900 dark:text-white tracking-tight">Welcome Back</h1>
        <p class="text-gray-500 dark:text-gray-400 mt-2">Sign in to start shopping</p>
      </div>

      <form @submit.prevent="handleLogin" class="space-y-6">
        
        <div v-if="errorMessage" class="p-3 rounded-lg bg-red-50 text-red-600 border border-red-200 text-sm text-center">
          {{ errorMessage }}
        </div>

        <div class="space-y-2">
          <label class="text-sm font-medium text-gray-700 dark:text-gray-300">Email</label>
          <input v-model="email" type="email" required 
                 class="w-full p-3 rounded-xl outline-none transition border focus:ring-2
                        bg-gray-50 border-gray-300 text-gray-900 focus:border-constructor-blue focus:ring-constructor-blue/20
                        dark:bg-black/20 dark:border-white/10 dark:text-white dark:focus:border-constructor-blue"
                 placeholder="you@example.com" />
        </div>

        <div class="space-y-2">
          <div class="flex justify-between">
            <label class="text-sm font-medium text-gray-700 dark:text-gray-300">Password</label>
            <a href="#" class="text-xs text-constructor-blue hover:text-safety-orange transition">Forgot password?</a>
          </div>
          <input v-model="password" type="password" required 
                 class="w-full p-3 rounded-xl outline-none transition border focus:ring-2
                        bg-gray-50 border-gray-300 text-gray-900 focus:border-constructor-blue focus:ring-constructor-blue/20
                        dark:bg-black/20 dark:border-white/10 dark:text-white dark:focus:border-constructor-blue"
                 placeholder="••••••••" />
        </div>

        <button type="submit" :disabled="isLoading" 
                class="w-full bg-constructor-blue hover:bg-safety-orange text-white font-bold py-3.5 rounded-xl shadow-lg transition-all transform hover:-translate-y-0.5 disabled:opacity-50 disabled:cursor-not-allowed flex justify-center">
          <span v-if="isLoading" class="animate-spin material-symbols-outlined">progress_activity</span>
          <span v-else>Sign In</span>
        </button>
      </form>

      <p class="text-center mt-8 text-gray-500 text-sm">
        Don't have an account? 
        <router-link to="/Register" class="text-constructor-blue font-bold hover:text-safety-orange transition">Sign up</router-link>
      </p>
    </div>
  </div>
</template>