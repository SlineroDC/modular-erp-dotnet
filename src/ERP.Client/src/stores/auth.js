import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import api from '../api/axios';
import router from '../router';

export const useAuthStore = defineStore('auth', () => {
  
    const token = ref(localStorage.getItem('jwt_token') || null);
    const storedUser = localStorage.getItem('user_data');

    let parsedUser = null;
    try {
        if (storedUser && storedUser !== "undefined"){
            parsedUser = JSON.parse(storedUser);
        }
    } catch (e) {
        console.error("Error read to User in storage",e);
        localStorage.removeItem('user_data');
    }

    const user = ref(parsedUser);

    const isAuthenticated = computed(() => !!token.value);

    async function login(email, password) {
        try {
            const response = await api.post('/auth/login', { email, password });
            console.log("response API:",response.data);

            const { token: newToken, username: newUser } = response.data;

            if(!newToken) throw new Error("Token don't received")
            
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