<script setup>
import { ref } from 'vue';

defineProps({
  isOpen: Boolean
});

const emit = defineEmits(['close']);

const form = ref({
  subject: '',
  message: ''
});

const isSending = ref(false);

function sendMessage() {
  if (!form.value.message) return;
  
  isSending.value = true;
  
  // Simulate From API
  setTimeout(() => {
    alert(`Message sent: ${form.value.subject}`);
    isSending.value = false;
    form.value = { subject: '', message: '' };
    emit('close');
  }, 1500);
}
</script>

<template>
  <Transition name="fade">
    <div v-if="isOpen" class="fixed inset-0 z-60 flex items-center justify-center p-4">
      
      <div class="absolute inset-0 bg-black/40 backdrop-blur-sm transition-opacity" @click="$emit('close')"></div>

      <div class="relative w-full max-w-md overflow-hidden rounded-2xl shadow-2xl transform transition-all scale-100 glass-card !bg-white/95 dark:!bg-gray-900/90">
        
        <div class="bg-constructor-blue p-4 text-white flex justify-between items-center">
          <div class="flex items-center gap-2">
            <span class="material-symbols-outlined">support_agent</span>
            <h3 class="font-bold text-lg">Contact Support</h3>
          </div>
          <button @click="$emit('close')" class="hover:bg-white/20 p-1 rounded-full transition">
            <span class="material-symbols-outlined text-sm">close</span>
          </button>
        </div>

        <div class="p-6 space-y-4">
          <p class="text-sm text-gray-600 dark:text-gray-300">
            Need help? Send us a message.
          </p>

          <div class="space-y-1">
            <label class="text-xs font-bold text-gray-500 dark:text-gray-400 uppercase tracking-wide">Subject</label>
            <select v-model="form.subject" class="w-full p-3 rounded-xl bg-gray-50 dark:bg-black/30 border border-gray-200 dark:border-white/10 text-gray-900 dark:text-white focus:ring-2 focus:ring-constructor-blue outline-none transition">
              <option value="" disabled selected>Select an issue...</option>
              <option>Order Status</option>
              <option>Product Inquiry</option>
              <option>Other</option>
            </select>
          </div>

          <div class="space-y-1">
            <label class="text-xs font-bold text-gray-500 dark:text-gray-400 uppercase tracking-wide">Message</label>
            <textarea v-model="form.message" rows="4" class="w-full p-3 rounded-xl bg-gray-50 dark:bg-black/30 border border-gray-200 dark:border-white/10 text-gray-900 dark:text-white focus:ring-2 focus:ring-constructor-blue outline-none transition resize-none" placeholder="Describe your problem..."></textarea>
          </div>

          <button @click="sendMessage" :disabled="isSending" 
                  class="w-full py-3.5 bg-safety-orange text-white font-bold rounded-xl shadow-lg hover:shadow-orange-500/30 hover:-translate-y-0.5 transition-all disabled:opacity-70 flex justify-center items-center gap-2">
            <span v-if="isSending" class="animate-spin material-symbols-outlined text-sm">progress_activity</span>
            <span v-else>Send Message</span>
          </button>
        </div>

      </div>
    </div>
  </Transition>
</template>