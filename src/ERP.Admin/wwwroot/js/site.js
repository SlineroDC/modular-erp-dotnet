// 1. Preferences mood the user
if (localStorage.theme === 'dark' || (!('theme' in localStorage) && window.matchMedia('(prefers-color-scheme: dark)').matches)) {
    document.documentElement.classList.add('dark');
} else {
    document.documentElement.classList.remove('dark');
}

// 2. Manager click to bottom
const themeToggleBtn = document.getElementById('theme-toggle');
// Save preference 
if (themeToggleBtn) {
    themeToggleBtn.addEventListener('click', function() {
        if (document.documentElement.classList.contains('dark')) {
            document.documentElement.classList.remove('dark');
            localStorage.theme = 'light'; 
        } else {
            document.documentElement.classList.add('dark');
            localStorage.theme = 'dark'; 
        }
    });
    
    
    
    //Feature Global for modal delete
    function toggleDeleteModal(show, id = null, itemName = 'item') {
        const modal = document.getElementById('deleteModal');
        const input = document.getElementById('deleteIdInput');
        const message = document.getElementById('modalMessage');

        if (show) {
            // Set the ID
            if (input) input.value = id;

            
            if (message) message.textContent = `Are you sure you want to delete "${itemName}"? This action cannot be undone.`;

            // View modal
            if (modal) modal.classList.remove('hidden');
        } else {
            // Occult modal
            if (modal) modal.classList.add('hidden');
        }
    }
    // Lógic import Modal 

    function openImportModal() {
        const modal = document.getElementById('importModal');
        if (modal) {
            modal.classList.remove('hidden');
        } else {
            console.error('No se encontró el elemento con id "importModal"');
        }
    }

    function closeImportModal() {
        const modal = document.getElementById('importModal');
        if (modal) {
            modal.classList.add('hidden');
        }
    }

// Lógic optional for view a name the file selected
    function showFileName(input) {
        const fileNameDisplay = document.getElementById('file-name');
        if (input.files && input.files.length > 0) {
            fileNameDisplay.textContent = "Selected: " + input.files[0].name;
            fileNameDisplay.classList.remove('hidden');
        } else {
            fileNameDisplay.classList.add('hidden');
        }
    }
}