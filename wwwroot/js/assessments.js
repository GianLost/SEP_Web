$(document).ready(function() {
    $('#avaliacaoTab a').on('click', function(e) {
        e.preventDefault();
        $(this).tab('show');
        var targetTab = $(this).attr('data-bs-target');
        $(targetTab + ' select').empty(); // Limpa as listas suspensas ao trocar de aba
        // Preencha aqui as listas suspensas com as opções de notas para cada subitem
    });
});