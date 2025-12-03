import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import api from '../api/axios';
import router from '../router';

export const useAuthStore = defineStore('auth', () => {
  
    const token = ref(localStorage.getItem('jwt_token') || null);
    const user = ref(JSON.parse(localStorage.getItem('user_data')) || null);

    const isAuthenticated = computed(() => !!token.value);

    async function login(email, password) {
        try {
            const response = await api.post('/auth/login', { email, password });
            
            const { token: newToken, user: newUser } = response.data;
            
            token.value = newToken;
            user.value = newUser;

            localStorage.setItem('jwt_token', newToken);
            localStorage.setItem('user_data', JSON.stringify(newUser));

            router.push('/');
            return { success: true };
        } catch (error) {
            console.error("Login error:", error);
            return { success: false, message: error.response?.data?.message || "Login failed" };
        }
    }

    function logout() {
        token.value = null;
        user.value = null;
        localStorage.removeItem('jwt_token');
        localStorage.removeItem('user_data');
        router.push('/login');
    }

    return { token, user, isAuthenticated, login, logout };
});