function toggleContactModal(show) {
    const modal = document.getElementById('contactModal');
    if (show) modal.classList.remove('hidden');
    else modal.classList.add('hidden');
}

async function sendSupportEmail() {
    const messageInput = document.getElementById('supportMessage');
    const statusDiv = document.getElementById('supportStatus');
    const msg = messageInput.value;

    if (!msg.trim()) {
        alert("Please write a message.");
        return;
    }

    statusDiv.textContent = "Sending...";
    statusDiv.className = "mt-2 text-sm text-blue-500 block";

    try {
        const res = await fetch('/api/support/send', { 
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ message: msg })
        });
        
        if (res.ok) {
            statusDiv.className = "mt-2 text-sm text-green-500 block";
            statusDiv.textContent = "Message sent successfully!";
            setTimeout(() => {
                toggleContactModal(false);
                messageInput.value = "";
                statusDiv.classList.add('hidden');
            }, 2000);
        } else {
            throw new Error("Failed to send");
        }
    } catch (err) {
        statusDiv.className = "mt-2 text-sm text-red-500 block";
        statusDiv.textContent = "Error sending message. Please try again later.";
    }
}