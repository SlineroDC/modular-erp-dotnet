<script setup>
import { ref } from 'vue';
import { useAuthStore } from '../stores/auth';

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
  
  if (!result.success) {
    errorMessage.value = result.message;
  }
  
  isLoading.value = false;
}
</script>

<template>
  <div class="min-h-screen flex items-center justify-center bg-[#0a0a0a] relative overflow-hidden px-4">
    
    <div class="absolute top-[-20%] left-[-10%] w-[50%] h-[50%] rounded-full bg-constructor-blue/20 blur-[120px]"></div>
    <div class="absolute bottom-[-20%] right-[-10%] w-[50%] h-[50%] rounded-full bg-safety-orange/10 blur-[120px]"></div>

    <div class="w-full max-w-md glass glass-dark p-8 rounded-3xl border border-white/10 shadow-2xl relative z-10">
      
      <div class="text-center mb-8">
        <img src="./public/svg/logo1.png" class="h-12 mx-auto mb-4" alt="Logo" />
        <h1 class="text-3xl font-bold text-white tracking-tight">Welcome Back</h1>
        <p class="text-gray-400 mt-2">Sign in to start shopping</p>
      </div>

      <form @submit.prevent="handleLogin" class="space-y-6">
        
        <div v-if="errorMessage" class="p-3 rounded-lg bg-red-500/10 border border-red-500/20 text-red-400 text-sm text-center">
          {{ errorMessage }}
        </div>

        <div class="space-y-2">
          <label class="text-sm font-medium text-gray-300">Email</label>
          <input v-model="email" type="email" required 
                 class="w-full bg-black/20 border border-white/10 rounded-xl p-3 text-white focus:border-constructor-blue focus:ring-1 focus:ring-constructor-blue outline-none transition"
                 placeholder="you@example.com" />
        </div>

        <div class="space-y-2">
          <div class="flex justify-between">
            <label class="text-sm font-medium text-gray-300">Password</label>
            <a href="#" class="text-xs text-constructor-blue hover:text-safety-orange transition">Forgot password?</a>
          </div>
          <input v-model="password" type="password" required 
                 class="w-full bg-black/20 border border-white/10 rounded-xl p-3 text-white focus:border-constructor-blue focus:ring-1 focus:ring-constructor-blue outline-none transition"
                 placeholder="••••••••" />
        </div>

        <button type="submit" :disabled="isLoading" 
                class="w-full bg-constructor-blue hover:bg-safety-orange text-white font-bold py-3.5 rounded-xl shadow-lg hover:shadow-orange-500/20 transition-all transform hover:-translate-y-0.5 disabled:opacity-50 disabled:cursor-not-allowed flex justify-center">
          <span v-if="isLoading" class="animate-spin material-symbols-outlined">progress_activity</span>
          <span v-else>Sign In</span>
        </button>
      </form>

      <p class="text-center mt-8 text-gray-500 text-sm">
        Don't have an account? 
        <router-link to="/register" class="text-white hover:text-safety-orange font-medium transition">Sign up</router-link>
      </p>
    </div>
  </div>
</template>