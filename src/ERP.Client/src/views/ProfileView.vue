<script setup>
import { ref, onMounted } from 'vue';
import MainLayout from '../layouts/MainLayout.vue';
import { useAuthStore } from '../stores/auth';
import api from '../api/axios';

const authStore = useAuthStore();
const user = authStore.user; // Datos básicos del store (para el header)

const isLoading = ref(false);

const form = ref({
  name: '',
  lastName: '',
  phone: '',
  address: '',
  currentPassword: '',
  newPassword: ''
});

async function fetchProfile() {
  try {
    const response = await api.get('/Auth/profile');
    const data = response.data;
    
    form.value.name = data.name || '';
    form.value.lastName = data.lastName || '';
    form.value.phone = data.phone || '';
    form.value.address = data.address || '';
  } catch (error) {
    console.error("Error loading profile:", error);
  }
}

async function updateProfile() {
  isLoading.value = true;
  try {
    await api.put('/Auth/profile', form.value);
    alert('Profile updated successfully!');
    
   
    form.value.currentPassword = '';
    form.value.newPassword = '';
    
    await fetchProfile();
  } catch (error) {
    console.error(error);
    alert('Error updating profile: ' + (error.response?.data?.message || error.message));
  } finally {
    isLoading.value = false;
  }
}

onMounted(() => {
  fetchProfile();
});
</script>

<template>
  <MainLayout>
    <div class="mx-auto max-w-3xl mt-8">
      <h1 class="mb-8 text-3xl font-bold text-gray-900 dark:text-white">My Profile</h1>

      <div class="glass-card rounded-2xl p-8">
        
        <div class="flex items-center gap-6 mb-8 border-b border-gray-200 dark:border-white/10 pb-8">
          <div class="h-24 w-24 rounded-full bg-gradient-to-br from-constructor-blue to-blue-500 flex items-center justify-center text-white text-4xl font-bold shadow-lg uppercase">
            {{ user?.userName?.substring(0, 2) || 'JD' }}
          </div>
          <div>
            <h2 class="text-2xl font-bold text-gray-900 dark:text-white">
              {{ user?.userName || 'User' }}
            </h2>
            <p class="text-safety-orange font-medium">Client Account</p>
          </div>
        </div>

        <form class="grid gap-6" @submit.prevent="updateProfile">
          
          <h3 class="text-lg font-bold text-gray-900 dark:text-white border-b border-gray-100 dark:border-white/5 pb-2">Personal Info</h3>
          
          <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
            
            <div class="group">
               <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">First Name</label>
               <input v-model="form.name" type="text" class="input-field" placeholder="John" />
            </div>
            
            <div class="group">
               <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">Last Name</label>
               <input v-model="form.lastName" type="text" class="input-field" placeholder="Doe" />
            </div>

            <div class="group">
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">Email</label>
              <input type="email" 
                     class="w-full rounded-xl border-gray-300 bg-gray-100 p-3 text-gray-500 cursor-not-allowed dark:bg-black/30 dark:border-white/10 dark:text-gray-400"
                     :value="user?.email" readonly />
            </div>

            <div class="group">
               <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">Phone</label>
               <input v-model="form.phone" type="tel" class="input-field" placeholder="+1 234 567" />
            </div>

            <div class="group md:col-span-2">
               <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">Address</label>
               <input v-model="form.address" type="text" class="input-field" placeholder="123 Main St, City" />
            </div>

          </div>

          <h3 class="text-lg font-bold text-gray-900 dark:text-white border-b border-gray-100 dark:border-white/5 pb-2 mt-4">Security</h3>
          
          <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div class="group">
               <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">Current Password</label>
               <input v-model="form.currentPassword" type="password" class="input-field" placeholder="••••••••" />
            </div>
            <div class="group">
               <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">New Password</label>
               <input v-model="form.newPassword" type="password" class="input-field" placeholder="••••••••" />
            </div>
          </div>

          <div class="flex justify-end gap-4 mt-8 pt-4 border-t border-gray-200 dark:border-white/10">
            <button type="button" class="px-6 py-2.5 text-gray-600 hover:bg-gray-100 rounded-xl transition-colors dark:text-gray-300 dark:hover:bg-white/5">
              Cancel
            </button>

            <button type="submit" :disabled="isLoading" 
                    class="px-8 py-2.5 rounded-xl font-bold transition-all transform hover:-translate-y-0.5 active:scale-95 shadow-lg
                           bg-constructor-blue text-white hover:bg-safety-orange
                           dark:bg-blue-600 dark:hover:bg-blue-500 flex items-center gap-2 disabled:opacity-50">
              <span v-if="isLoading" class="material-symbols-outlined animate-spin text-sm">progress_activity</span>
              Save Changes
            </button>
          </div>

        </form>
      </div>
    </div>
  </MainLayout>
</template>

