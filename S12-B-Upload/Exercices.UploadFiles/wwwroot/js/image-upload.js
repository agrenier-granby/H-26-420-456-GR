document.addEventListener('DOMContentLoaded', function () {
    let imageCount = 1;
    const maxImages = 5;

    const addImageBtn = document.getElementById('add-image-btn');
    const imageUploadsContainer = document.getElementById('image-uploads-container');

    addImageBtn.addEventListener('click', async function () {
        if (imageCount >= maxImages) {
            alert('Vous ne pouvez téléverser que 5 images maximum.');
            return;
        }

        try {
            const response = await fetch('/Image/GetImageUploadPartial', {
                method: 'GET',
                headers: {
                    'Content-Type': 'text/html'
                }
            });

            if (!response.ok) {
                throw new Error('Erreur lors de la récupération du partial.');
            }

            const data = await response.text();
            imageUploadsContainer.insertAdjacentHTML('beforeend', data);
            imageCount++;

            if (imageCount >= maxImages) {
                addImageBtn.disabled = true;
            }
        } catch (error) {
            console.error('Erreur:', error);
            alert('Erreur lors de l\'ajout d\'un champ d\'image.');
        }
    });

    // Délégation d'événements pour les boutons de suppression
    document.addEventListener('click', function (event) {
        if (event.target.classList.contains('remove-image-btn')) {
            const imageUploadItem = event.target.closest('.image-upload-item');
            if (imageUploadItem) {
                imageUploadItem.remove();
                imageCount--;

                if (imageCount < maxImages) {
                    addImageBtn.disabled = false;
                }
            }
        }
    });
});
