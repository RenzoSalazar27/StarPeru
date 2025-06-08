function openModal(title, conditions) {
    document.getElementById('promoModalLabel').innerText = title;
    document.getElementById('promoDescription').innerText = conditions;
    $('#promoModal').modal('show');
}
