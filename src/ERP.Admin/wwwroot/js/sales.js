// Estado del Carrito
let cart = [];

// Actualiza el precio en el input readonly al cambiar el dropdown
function updatePrice() {
    const select = document.getElementById('productSelect');
    // Obtenemos el precio del atributo data-price
    const price = select.options[select.selectedIndex].getAttribute('data-price');

    document.getElementById('priceInput').value = price ? parseFloat(price).toFixed(2) : "0.00";
}

// Agregar producto al carrito
function addToCart() {
    const select = document.getElementById('productSelect');
    const id = select.value;

    if (!id) {
        alert("Please select a product");
        return;
    }

    const name = select.options[select.selectedIndex].getAttribute('data-name');
    const price = parseFloat(select.options[select.selectedIndex].getAttribute('data-price'));
    const qtyInput = document.getElementById('qtyInput');
    const qty = parseInt(qtyInput.value);

    if (qty <= 0) {
        alert("Quantity must be greater than 0");
        return;
    }

    // Lógica: Si ya existe, sumamos cantidad; si no, agregamos nuevo
    const existing = cart.find(x => x.productId == id);
    if (existing) {
        existing.quantity += qty;
    } else {
        cart.push({ productId: id, name: name, unitPrice: price, quantity: qty });
    }

    renderCart();

    // Resetear inputs para la siguiente agregación
    select.value = "";
    document.getElementById('priceInput').value = "0.00";
    qtyInput.value = 1;
}

// Eliminar producto del carrito
function removeFromCart(index) {
    cart.splice(index, 1);
    renderCart();
}

// Dibujar la tabla HTML del carrito
function renderCart() {
    const tbody = document.getElementById('cartBody');
    const emptyMsg = document.getElementById('emptyCartMsg');
    const table = document.getElementById('cartTable');
    const totalEl = document.getElementById('grandTotal');

    tbody.innerHTML = "";
    let total = 0;

    if (cart.length === 0) {
        emptyMsg.classList.remove('hidden');
        table.classList.add('hidden');
    } else {
        emptyMsg.classList.add('hidden');
        table.classList.remove('hidden');

        cart.forEach((item, index) => {
            const lineTotal = item.quantity * item.unitPrice;
            total += lineTotal;

            const row = `
                <tr class="dark:text-black border-b dark:border-gray-700 last:border-0">
                    <td class="py-3 font-medium">${item.name}</td>
                    <td class="py-3 text-center">${item.quantity}</td>
                    <td class="py-3 text-right">$${lineTotal.toFixed(2)}</td>
                    <td class="py-3 text-right">
                        <button onclick="removeFromCart(${index})" class="p-1 rounded hover:bg-red-50 text-red-500 hover:text-red-700 transition-colors">
                            <span class="material-symbols-outlined text-lg">close</span>
                        </button>
                    </td>
                </tr>
            `;
            tbody.innerHTML += row;
        });
    }
    totalEl.innerText = "$" + total.toFixed(2);
}

// Enviar la venta al servidor (Fetch API)
async function submitSale() {
    const customerSelect = document.getElementById('customerSelect');
    const customerId = customerSelect.value;

    if (!customerId) { alert("Please select a customer"); return; }
    if (cart.length === 0) { alert("Cart is empty"); return; }

    const total = cart.reduce((sum, item) => sum + (item.quantity * item.unitPrice), 0);

    const saleData = {
        customerId: parseInt(customerId),
        total: total,
        details: cart
    };
    
    // Esto busca el input oculto que genera @Html.AntiForgeryToken()
    const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
    const token = tokenInput ? tokenInput.value : '';

    try {
        const response = await fetch('?handler=SaveSale', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': token
            },
            body: JSON.stringify(saleData)
        });

        if (response.ok) {
            const result = await response.json();
            window.location.href = result.redirectUrl;
        } else {
            console.error("Server Error:", response.statusText);
            alert("Error saving sale. Check console for details.");
        }
    } catch (error) {
        console.error('Network Error:', error);
        alert("An unexpected error occurred.");
    }
}