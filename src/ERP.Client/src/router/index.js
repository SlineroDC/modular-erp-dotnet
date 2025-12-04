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

//Guard negations

router.beforeEach((to, from, next) => {
    const authStore = useAuthStore()

    const publicPages = ['Login','Register']
    const isPublic = publicPages.includes(to.name)

    if(!isPublic && !authStore.isAuthenticated){
        return next({name: 'Login'})
    }

    if (isPublic && authStore.isAuthenticated){
        return next({name: 'Home'})
    }
    next()
})


export default router