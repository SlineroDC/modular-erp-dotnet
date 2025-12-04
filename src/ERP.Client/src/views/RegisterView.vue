<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import api from '../api/axios';

const router = useRouter();
const isLoading = ref(false);
const errorMessage = ref('');

// Modelo del formulario
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
  <div class="min-h-screen flex items-center justify-center bg-[#0a0a0a] relative overflow-hidden px-4 py-10">
    
    <div class="absolute top-[-10%] right-[-10%] w-[40%] h-[40%] rounded-full bg-constructor-blue/20 blur-[120px]"></div>
    <div class="absolute bottom-[-10%] left-[-10%] w-[40%] h-[40%] rounded-full bg-safety-orange/10 blur-[120px]"></div>

    <div class="w-full max-w-2xl glass glass-dark p-8 rounded-3xl border border-white/10 shadow-2xl relative z-10">
      
      <div class="text-center mb-8">
        <h1 class="text-3xl font-bold text-white tracking-tight">Create Account</h1>
        <p class="text-gray-400 mt-2">Join Firmeza today</p>
      </div>

      <form @submit.prevent="handleRegister" class="space-y-6">
        
        <div v-if="errorMessage" class="p-3 rounded-lg bg-red-500/10 border border-red-500/20 text-red-400 text-sm text-center">
          {{ errorMessage }}
        </div>

        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
          
          <div class="space-y-2">
            <label class="text-sm font-medium text-gray-300">First Name</label>
            <input v-model="form.name" type="text" required class="input-field" placeholder="John" />
          </div>

          <div class="space-y-2">
            <label class="text-sm font-medium text-gray-300">Last Name</label>
            <input v-model="form.lastName" type="text" required class="input-field" placeholder="Doe" />
          </div>

          <div class="space-y-2">
            <label class="text-sm font-medium text-gray-300">Document ID</label>
            <input v-model="form.document" type="text" required class="input-field" placeholder="123456789" />
          </div>

          <div class="space-y-2">
            <label class="text-sm font-medium text-gray-300">Phone</label>
            <input v-model="form.phone" type="tel" class="input-field" placeholder="+1 234 567" />
          </div>

          <div class="space-y-2 md:col-span-2">
            <label class="text-sm font-medium text-gray-300">Address</label>
            <input v-model="form.address" type="text" class="input-field" placeholder="123 Main St" />
          </div>

          <div class="space-y-2 md:col-span-2">
            <label class="text-sm font-medium text-gray-300">Email</label>
            <input v-model="form.email" type="email" required class="input-field" placeholder="john@example.com" />
          </div>

          <div class="space-y-2 md:col-span-2">
            <label class="text-sm font-medium text-gray-300">Password</label>
            <input v-model="form.password" type="password" required class="input-field" placeholder="••••••••" />
          </div>
        </div>

        <button type="submit" :disabled="isLoading" 
                class="w-full bg-constructor-blue hover:bg-safety-orange text-white font-bold py-3.5 rounded-xl shadow-lg hover:shadow-orange-500/20 transition-all transform hover:-translate-y-0.5 disabled:opacity-50 disabled:cursor-not-allowed flex justify-center mt-4">
          <span v-if="isLoading" class="animate-spin material-symbols-outlined">progress_activity</span>
          <span v-else>Sign Up</span>
        </button>
      </form>

      <p class="text-center mt-6 text-gray-500 text-sm">
        Already have an account? 
        <router-link to="/login" class="text-white hover:text-safety-orange font-medium transition">Log In</router-link>
      </p>
    </div>
  </div>
</template>

<style scoped>
.input-field {
  @apply w-full bg-black/20 border border-white/10 rounded-xl p-3 text-white focus:border-constructor-blue focus:ring-1 focus:ring-constructor-blue outline-none transition;
}
</style>