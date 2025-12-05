import { ref, onMounted } from 'vue';

const isDark = ref(false);

export function useTheme() {

  onMounted(() => {

    const storedTheme = localStorage.getItem('theme');
    

    const systemDark = window.matchMedia('(prefers-color-scheme: dark)').matches;

    if (storedTheme === 'dark' || (!storedTheme && systemDark)) {
      isDark.value = true;
      document.documentElement.classList.add('dark');
    } else {
      isDark.value = false;
      document.documentElement.classList.remove('dark');
    }
  });

  function toggleTheme() {
    isDark.value = !isDark.value;
    
    if (isDark.value) {
      document.documentElement.classList.add('dark');
      localStorage.setItem('theme', 'dark');
    } else {
      document.documentElement.classList.remove('dark');
      localStorage.setItem('theme', 'light');
    }
  }

  return { isDark, toggleTheme };
}