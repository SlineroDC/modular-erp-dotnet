<script setup>
import MainLayout from '../layouts/MainLayout.vue';
</script>

<template>
  <MainLayout>
    <div class="mx-auto max-w-3xl mt-8">
      
      <h1 class="mb-8 text-3xl font-bold text-gray-900 dark:text-white">My Profile</h1>
      
      <div class="glass glass-dark rounded-2xl p-8 bg-white dark:bg-gray-800/50">
        
        <div class="flex items-center gap-6 mb-8 border-b border-gray-200 dark:border-gray-700 pb-8">
          <div class="h-24 w-24 rounded-full bg-gradient-to-br from-constructor-blue to-blue-500 flex items-center justify-center text-white text-4xl font-bold shadow-lg">
            JD
          </div>
          <div>
            <h2 class="text-2xl font-bold text-gray-900 dark:text-white">John Doe</h2>
            <p class="text-safety-orange font-medium">Client Account</p>
            <p class="text-sm text-gray-500 dark:text-gray-400 mt-1">Member since Jan 2025</p>
          </div>
        </div>

        <form class="grid gap-6" @submit.prevent>
          <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div class="group">
               <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2 group-focus-within:text-constructor-blue transition-colors">Username</label>
               <input type="text" class="w-full rounded-xl border-gray-300 bg-gray-50 p-3 focus:ring-2 focus:ring-constructor-blue focus:border-constructor-blue dark:bg-gray-900/50 dark:border-gray-600 dark:text-white transition-all outline-none" value="johndoe" />
            </div>
            <div class="group">
               <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2 group-focus-within:text-constructor-blue transition-colors">Email</label>
               <input type="email" class="w-full rounded-xl border-gray-300 bg-gray-100 p-3 text-gray-500 cursor-not-allowed dark:bg-gray-900/30 dark:border-gray-700" value="john@example.com" readonly />
            </div>
          </div>
          
          <div class="mt-4">
             <h3 class="text-lg font-semibold text-gray-900 dark:text-white mb-4">Security</h3>
             <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
               <div class="group">
                  <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2 group-focus-within:text-constructor-blue transition-colors">New Password</label>
                  <input type="password" class="w-full rounded-xl border-gray-300 bg-gray-50 p-3 focus:ring-2 focus:ring-constructor-blue dark:bg-gray-900/50 dark:border-gray-600 dark:text-white transition-all outline-none" placeholder="••••••••" />
               </div>
               <div class="group">
                  <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2 group-focus-within:text-constructor-blue transition-colors">Confirm Password</label>
                  <input type="password" class="w-full rounded-xl border-gray-300 bg-gray-50 p-3 focus:ring-2 focus:ring-constructor-blue dark:bg-gray-900/50 dark:border-gray-600 dark:text-white transition-all outline-none" placeholder="••••••••" />
               </div>
             </div>
          </div>

          <div class="flex justify-end gap-4 mt-8 pt-4 border-t border-gray-200 dark:border-gray-700">
            <button type="button" class="px-6 py-2.5 text-gray-600 hover:bg-gray-100 rounded-xl transition-colors dark:text-gray-300 dark:hover:bg-white/5">
              Cancel
            </button>
            <button type="submit" class="bg-constructor-blue text-white px-8 py-2.5 rounded-xl font-bold hover:bg-safety-orange hover:shadow-lg hover:shadow-orange-500/20 transition-all transform hover:-translate-y-0.5 active:scale-95">
              Save Changes
            </button>
          </div>
        </form>
      </div>
    </div>
  </MainLayout>
</template>