$(document).ready(function () {
    // Chamada da função que traz o plugin DataTable
    getDatatableClass('.table-style');
});

/* Função de configuração do plugin DataTable e aplicação do plugin de acordo com a classe capturada */
function getDatatableClass(tableClass) {
    $(tableClass).DataTable({
        "ordering": true,
        "paging": true,
        "searching": true,
        "language": {
            "emptyTable": "Nenhum registro encontrado na tabela",
            "info": "Mostrar _START_ até _END_ de _TOTAL_ registros",
            "infoEmpty": "Mostrar 0 até 0 de 0 Registros",
            "infoFiltered": "(Filtrar de um total de _MAX_ registros)",
            "infoPostFix": "",
            "infoThousands": ".",
            "lengthMenu": "Mostrar _MENU_ registros por página",
            "loadingRecords": "Carregando...",
            "processing": "Processando...",
            "zeroRecords": "Nenhum registro encontrado",
            "search": "Pesquisar",
            "paginate": {
                "next": "Proximo",
                "previous": "Anterior",
                "first": "Primeiro",
                "last": "Ultimo"
            },
            "aria": {
                "sortAscending": ": Ordenar colunas de forma ascendente",
                "sortDescending": ": Ordenar colunas de forma descendente"
            }
        }
    });
}