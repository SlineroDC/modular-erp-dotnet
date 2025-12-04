import axios from 'axios';
import { useAuthStore } from '../stores/auth';

// Create instance axios
const api = axios.create({
    baseURL: 'http://localhost:5160/api', 
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