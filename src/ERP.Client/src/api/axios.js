import axios from 'axios';
import { useAuthStore } from '../stores/auth';

// Create instance axios
const api = axios.create({
    baseURL: import.meta.env.VITE_API_URL || 'http://localhost:5000/api/', 
    headers: {
        'Content-Type': 'application/json'
    }
});

// Interceptor: Inject token before petitions
api.interceptors.request.use(config => {
    const authStore = useAuthStore();
    const token = authStore.token;
    
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

export default api;