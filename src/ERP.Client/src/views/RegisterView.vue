<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import api from '../api/axios';
import { useTheme } from '../composables/useTheme'; 


const { isDark, toggleTheme } = useTheme();

const router = useRouter();
const isLoading = ref(false);
const errorMessage = ref('');


const inputClass = "w-full bg-gray-50 dark:bg-black/20 border border-gray-300 dark:border-white/10 rounded-xl p-3 text-gray-900 dark:text-white focus:border-constructor-blue focus:ring-1 focus:ring-constructor-blue outline-none transition placeholder-gray-500 dark:placeholder-gray-400";

const form = ref({
  email: '',
  password: '',
  name: '',
  lastName: '',
  document: '',
  phone: '',
  address: ''
});

async function handleRegister() {
  isLoading.value = true;
  errorMessage.value = '';

  try {
    await api.post('/Auth/register', form.value);
    alert('Registration successful! Please log in.');
    router.push('/login');
  } catch (error) {
    console.error(error);
    errorMessage.value = error.response?.data?.message || "Registration failed. Try again.";
  } finally {
    isLoading.value = false;
  }
}
</script>

<template>
  <div class="min-h-screen flex items-center justify-center bg-gray-100 dark:bg-[#0a0a0a] relative overflow-hidden px-4 py-10 transition-colors duration-500">
    
    <button @click="toggleTheme" class="absolute top-6 right-6 p-3 rounded-full bg-white/10 backdrop-blur-md border border-black/10 dark:border-white/20 text-gray-600 dark:text-white hover:scale-110 transition shadow-lg z-50">
       <span class="material-symbols-outlined">{{ isDark ? 'light_mode' : 'dark_mode' }}</span>
    </button>

    <div class="absolute top-[-10%] right-[-10%] w-[40%] h-[40%] rounded-full bg-constructor-blue/20 blur-[120px] opacity-0 dark:opacity-100 transition-opacity duration-500"></div>
    <div class="absolute bottom-[-10%] left-[-10%] w-[40%] h-[40%] rounded-full bg-safety-orange/10 blur-[120px] opacity-0 dark:opacity-100 transition-opacity duration-500"></div>

    <div class="w-full max-w-2xl p-8 rounded-3xl shadow-2xl relative z-10 transition-all duration-300
                bg-white border border-gray-200
                dark:glass dark:glass-dark dark:border-white/10">
      
      <div class="text-center mb-8">
        <h1 class="text-3xl font-bold text-gray-900 dark:text-white tracking-tight">Create Account</h1>
        <p class="text-gray-500 dark:text-gray-400 mt-2">Join Firmeza today</p>
      </div>

      <form @submit.prevent="handleRegister" class="space-y-6">
        
        <div v-if="errorMessage" class="p-3 rounded-lg bg-red-50 border border-red-200 text-red-600 text-sm text-center">
          {{ errorMessage }}
        </div>

        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
          
          <div class="space-y-2">
            <label class="text-sm font-medium text-gray-700 dark:text-gray-300">First Name</label>
            <input v-model="form.name" type="text" required :class="inputClass" placeholder="John" />
          </div>

          <div class="space-y-2">
            <label class="text-sm font-medium text-gray-700 dark:text-gray-300">Last Name</label>
            <input v-model="form.lastName" type="text" required :class="inputClass" placeholder="Doe" />
          </div>

          <div class="space-y-2">
            <label class="text-sm font-medium text-gray-700 dark:text-gray-300">Document ID</label>
            <input v-model="form.document" type="text" required :class="inputClass" placeholder="123456789" />
          </div>

          <div class="space-y-2">
            <label class="text-sm font-medium text-gray-700 dark:text-gray-300">Phone</label>
            <input v-model="form.phone" type="tel" :class="inputClass" placeholder="+1 234 567" />
          </div>

          <div class="space-y-2 md:col-span-2">
            <label class="text-sm font-medium text-gray-700 dark:text-gray-300">Address</label>
            <input v-model="form.address" type="text" :class="inputClass" placeholder="123 Main St" />
          </div>

          <div class="space-y-2 md:col-span-2">
            <label class="text-sm font-medium text-gray-700 dark:text-gray-300">Email</label>
            <input v-model="form.email" type="email" required :class="inputClass" placeholder="john@example.com" />
          </div>

          <div class="space-y-2 md:col-span-2">
            <label class="text-sm font-medium text-gray-700 dark:text-gray-300">Password</label>
            <input v-model="form.password" type="password" required :class="inputClass" placeholder="••••••••" />
          </div>
        </div>

        <button type="submit" :disabled="isLoading" 
                class="w-full bg-constructor-blue hover:bg-safety-orange text-white font-bold py-3.5 rounded-xl shadow-lg hover:shadow-orange-500/20 transition-all transform hover:-translate-y-0.5 disabled:opacity-50 disabled:cursor-not-allowed flex justify-center mt-4">
          <span v-if="isLoading" class="animate-spin material-symbols-outlined">progress_activity</span>
          <span v-else>Sign Up</span>
        </button>
      </form>

      <p class="text-center mt-6 text-gray-500 dark:text-gray-400 text-sm">
        Already have an account? 
        <router-link to="/login" class="text-constructor-blue font-bold hover:text-safety-orange transition">Log In</router-link>
      </p>
    </div>
  </div>
</template>