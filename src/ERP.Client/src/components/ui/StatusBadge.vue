<script setup>
defineProps({
  color: {
    type: String,
    default: 'emerald' 
  },
  pulse: {
    type: Boolean,
    default: false
  }
});

const colorClasses = {
  emerald: 'text-emerald-400 border-emerald-500/20 bg-emerald-500/5',
  blue: 'text-blue-400 border-blue-500/20 bg-blue-500/5',
  orange: 'text-orange-400 border-orange-500/20 bg-orange-500/5',
  red: 'text-red-400 border-red-500/20 bg-red-500/5',
  gray: 'text-gray-400 border-white/10 bg-white/5'
};

const pulseColors = {
  emerald: 'bg-emerald-500',
  blue: 'bg-blue-500',
  orange: 'bg-orange-500',
  red: 'bg-red-500',
  gray: 'bg-gray-400'
};
</script>

<template>
  <div 
    class="inline-flex items-center gap-2 px-3 py-1 text-[10px] font-bold tracking-widest uppercase rounded-full border transition-colors"
    :class="colorClasses[color]"
  >
    <span v-if="pulse" class="w-1.5 h-1.5 rounded-full animate-pulse" :class="pulseColors[color]"></span>
    <slot />
  </div>
</template>