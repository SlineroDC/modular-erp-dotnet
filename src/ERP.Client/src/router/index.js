import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const routes = [
    {
        path: '/',
        name: 'Home',
        component: () => import('../views/HomeView.vue')
    },
    {
        path: '/login',
        name: 'Login',
        component: () => import('../views/LoginView.vue')
    },
    {
        path: '/register', 
        name: 'Register',
        component: () => import('../views/RegisterView.vue')
    },
    {
        path: '/profile',
        name: 'Profile',
        component: () => import('../views/ProfileView.vue')
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

//Guatd navegations

router.beforeEach((to, from, next) => {
    const authStore = useAuthStore()

    const publicPages = ['/login','/register']
    const authRequired = !publicPages.includes(to.path)

    if (authRequired || !authStore.isAuthenticated){
        return next('/login')
    }
    next()
})


export default router