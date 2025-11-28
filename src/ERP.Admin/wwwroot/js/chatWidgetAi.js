function toggleChat() {
    const chat = document.getElementById('chatWindow');
    chat.classList.toggle('hidden');
    if (!chat.classList.contains('hidden')) {
        setTimeout(() => document.getElementById('userQuestion').focus(), 100);
    }
}

async function sendMessage(e) {
    e.preventDefault();
    const input = document.getElementById('userQuestion');
    const btn = e.target.querySelector('button');
    const msgs = document.getElementById('chatMessages');
    const q = input.value.trim();

    if (!q) return;

    // Disable input while thinking
    input.disabled = true;
    btn.disabled = true;

    // 1. User Message
    msgs.innerHTML += `
        <div class="self-end rounded-lg rounded-tr-none bg-[#003366] text-white px-4 py-2 max-w-[85%] shadow-sm animate-fade-in">
            ${q}
        </div>
    `;
    input.value = '';
    scrollToBottom();

    // 2. Loading Indicator
    const loadId = 'load-' + Date.now();
    msgs.innerHTML += `
        <div id="${loadId}" class="self-start text-gray-400 text-xs italic ml-2 flex items-center gap-1">
            <span class="animate-pulse">●</span><span class="animate-pulse delay-75">●</span><span class="animate-pulse delay-150">●</span>
        </div>
    `;
    scrollToBottom();

    try {
        // 3. Backend Call
        const res = await fetch('/api/ai/ask', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ question: q })
        });

        const data = await res.json();
        document.getElementById(loadId).remove();

        // 4. AI Response
        msgs.innerHTML += `
            <div class="self-start rounded-lg rounded-tl-none bg-white border border-gray-200 px-4 py-3 text-gray-800 dark:bg-gray-700 dark:border-gray-600 dark:text-gray-200 max-w-[90%] shadow-sm animate-fade-in prose dark:prose-invert text-sm">
                ${data.answer.replace(/\n/g, '<br>')}
            </div>
        `;

    } catch (err) {
        document.getElementById(loadId).remove();
        msgs.innerHTML += `<div class="self-start text-red-500 text-xs bg-red-50 p-2 rounded border border-red-100">Connection error with AI service.</div>`;
    }

    input.disabled = false;
    btn.disabled = false;
    input.focus();
    scrollToBottom();
}

function scrollToBottom() {
    const msgs = document.getElementById('chatMessages');
    msgs.scrollTop = msgs.scrollHeight;
}