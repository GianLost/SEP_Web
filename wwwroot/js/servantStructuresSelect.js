$('.servant-register').show(function () {

    $(".servant-instituition").val(function () {
        $('.servant-instituition').append($('<option>').text(' -- selecione o órgão -- ').val(''));
    });

    $('.servant-instituition').change(function () {

        var selectedInstituitionId = $(this).val();
        if (selectedInstituitionId) {
            $.get('/Structure/GetDivisionsByInstituition', { instituitionId: selectedInstituitionId }, function (data) {
                $('.servant-division').empty();
                $('.servant-division').append($('<option>').text(' -- selecione a divisão -- ').val(''));
                $.each(data, function (index, item) {
                    $('.servant-division').append($('<option>').text(item.text).val(item.value));
                });
            });
        } else {
            $('.servant-division').empty();
            $('.servant-division').append($('<option>').text(' -- selecione a divisão -- ').val(''));
        }
    });

    $('.servant-division').change(function () {
        var selectedSectionId = $(this).val();
        if (selectedSectionId) {
            $.get('/Structure/GetSectionsByDivisions', { DivisionId: selectedSectionId }, function (data) {
                $('.servant-section').empty();
                $('.servant-section').append($('<option>').text(' -- selecione a seção -- ').val(''));
                $.each(data, function (index, item) {
                    $('.servant-section').append($('<option>').text(item.text).val(item.value));
                });
            });
        } else {
            $('.servant-section').empty();
            $('.servant-section').append($('<option>').text(' -- selecione a seção -- ').val(''));
        }
    });

    $('.servant-section').change(function () {
        var selectedSectorId = $(this).val();
        if (selectedSectorId) {
            $.get('/Structure/GetSectorsBySections', { SectionId: selectedSectorId }, function (data) {
                $('.servant-sector').empty();
                $('.servant-sector').append($('<option>').text(' -- selecione o setor -- ').val(''));
                $.each(data, function (index, item) {
                    $('.servant-sector').append($('<option>').text(item.text).val(item.value));
                });
            });
        } else {
            $('.servant-sector').empty();
            $('.servant-sector').append($('<option>').text(' -- selecione o setor -- ').val(''));
        }
    });

    $('.servant-sector').change(function () {
        var searchEvaluatorsToInstituitionId = $('.servant-instituition').val();
        if (searchEvaluatorsToInstituitionId) {
            $.get('/Structure/GetEvaluatorsForInstituitionId', { UserEvaluatorId1: searchEvaluatorsToInstituitionId }, function (data) {
                $('.servant-evaluator-first').empty();
                $('.servant-evaluator-first').append($('<option>').text(' -- selecione o avaliador 1 -- ').val(''));
                $.each(data, function (index, item) {
                    $('.servant-evaluator-first').append($('<option>').text(item.text).val(item.value));
                });
            });
        } else {
            $('.servant-evaluator-first').empty();
            $('.servant-evaluator-first').append($('<option>').text(' -- selecione o avaliador 1 -- ').val(''));
        }
    });

    $('.servant-evaluator-first').change(function () {

        var selectedEvaluatorsForInstituition = $('.servant-instituition').val();
        var searchSecondEvaluator = $('.servant-evaluator-first').val();
        if (searchSecondEvaluator) {
            $.get('/Structure/GetEvaluatorsForInstituitionId', { UserEvaluatorId2: searchSecondEvaluator, UserEvaluatorId1: selectedEvaluatorsForInstituition }, function (data) {
                $('.servant-evaluator-second').empty();
                $('.servant-evaluator-second').append($('<option>').text(' -- selecione o avaliador 2 -- ').val(''));
                $.each(data, function (index, item) {
                    $('.servant-evaluator-second').append($('<option>').text(item.text).val(item.value));
                });
            });
        } else {
            $('.servant-evaluator-second').empty();
            $('.servant-evaluator-second').append($('<option>').text(' -- selecione o avaliador 2 -- ').val(''));
        }
    });

});