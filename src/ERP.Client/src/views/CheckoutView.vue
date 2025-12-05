<script setup>
import MainLayout from '../layouts/MainLayout.vue';
import { useCartStore } from '../stores/cart';
import { useAuthStore } from '../stores/auth';
import { useRouter } from 'vue-router';
import { ref } from 'vue';

const cart = useCartStore();
const auth = useAuthStore();
const router = useRouter();
const isProcessing = ref(false);

// Datos del usuario para mostrar envío (Simulados o del store)
const user = auth.user || {};

async function confirmOrder() {
    isProcessing.value = true;
    const success = await cart.checkout(); // Reutilizamos la lógica del store
    isProcessing.value = false;
    
    if (success) {
        router.push('/'); // O a una página de "Gracias"
    }
}
</script>

<template>
    <MainLayout>
        <div class="max-w-4xl mx-auto">
            <h1 class="text-3xl font-bold text-constructor-blue mb-8">Finalizar Compra</h1>
            
            <div class="grid grid-cols-1 md:grid-cols-2 gap-8">
                <div class="glass p-6 rounded-xl text-gray-800 dark:text-white">
                    <h2 class="text-xl font-bold mb-4">Datos de Envío</h2>
                    <p><strong>Cliente:</strong> {{ user.userName }}</p>
                    <p><strong>Email:</strong> {{ user.email }}</p>
                    <p class="text-sm text-gray-500 mt-2">Si estos datos son incorrectos, por favor actualiza tu perfil.</p>
                    <router-link to="/profile" class="text-safety-orange hover:underline text-sm">Editar Perfil</router-link>
                </div>

                <div class="glass p-6 rounded-xl text-gray-800 dark:text-white">
                    <h2 class="text-xl font-bold mb-4">Resumen del Pedido</h2>
                    <div class="space-y-2 mb-4">
                        <div v-for="item in cart.items" :key="item.id" class="flex justify-between">
                            <span>{{ item.name }} (x{{ item.quantity }})</span>
                            <span>${{ (item.price * item.quantity).toFixed(2) }}</span>
                        </div>
                    </div>
                    <div class="border-t pt-4 flex justify-between font-bold text-xl">
                        <span>Total</span>
                        <span>${{ cart.totalPrice.toFixed(2) }}</span>
                    </div>
                    
                    <button @click="confirmOrder" :disabled="isProcessing"
                        class="w-full mt-6 bg-constructor-blue text-white py-3 rounded-lg hover:bg-safety-orange transition font-bold disabled:opacity-50">
                        {{ isProcessing ? 'Procesando...' : 'Confirmar y Pagar' }}
                    </button>
                </div>
            </div>
        </div>
    </MainLayout>
</template>