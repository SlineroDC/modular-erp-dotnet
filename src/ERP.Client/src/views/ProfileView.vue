<script setup>
import MainLayout from '../layouts/MainLayout.vue';
import { useAuthStore } from '../stores/auth';
import api from '../api/axios';
import { onMounted } from 'vue';

const authStore = useAuthStore();
const user = authStore.user;

const form = ref({
  name: '',
  lastName: '',
  phone: '',
  address: '',
  currentPassword: '',
  newPassword: ''
});

onMounted(() => {
  
});
</script>

<template>
  <MainLayout>
    <div class="mx-auto max-w-3xl mt-8">
      <h1 class="mb-8 text-3xl font-bold text-gray-900 dark:text-white">My Profile</h1>

      <div class="glass glass-dark rounded-2xl p-8 bg-white dark:bg-gray-800/50">
        <div class="flex items-center gap-6 mb-8 border-b border-gray-200 dark:border-gray-700 pb-8">
          <div
            class="h-24 w-24 rounded-full bg-linear-to-br from-constructor-blue to-blue-500 flex items-center justify-center text-white text-4xl font-bold shadow-lg uppercase">
            {{ user?.userName?.substring(0, 2) || 'JD' }}
          </div>
          <div>
            <h2 class="text-2xl font-bold text-gray-900 dark:text-white">
              {{ user?.userName || 'User' }}
            </h2>
            <p class="text-safety-orange font-medium">Client Account</p>
          </div>
        </div>

        <form class="grid gap-6" @submit.prevent>
          <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div class="group">
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">Email</label>
              <input type="email"
                class="w-full rounded-xl border-gray-300 bg-gray-100 p-3 text-gray-500 cursor-not-allowed dark:bg-gray-900/30 dark:border-gray-700"
                :value="user?.email" readonly />
            </div>
          </div>

          <div class="flex justify-end gap-4 mt-8 pt-4 border-t border-gray-200 dark:border-gray-700">
            <button type="button"
              class="px-6 py-2.5 text-gray-600 hover:bg-gray-100 rounded-xl transition-colors dark:text-gray-300 dark:hover:bg-white/5">
              Cancel
            </button>

            <button type="submit" class="px-8 py-2.5 rounded-xl font-bold transition-all transform hover:-translate-y-0.5 active:scale-95 shadow-lg
                           bg-constructor-blue text-white hover:bg-safety-orange
                           dark:bg-blue-600 dark:hover:bg-blue-500">
              Save Changes
            </button>
          </div>
        </form>
      </div>
    </div>
  </MainLayout>
</template>