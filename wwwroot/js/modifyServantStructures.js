function EditServantStructures() {
    let properties = {
        Id: $("#edit-servant-structure-Id").val(),
        InstituitionId: $(".servant-instituition").val(),
        DivisionId: $(".servant-division").val(),
        SectionId: $(".servant-section").val(),
        SectorId: $(".servant-sector").val(),
        UserEvaluatorId1: $(".servant-evaluator-first").val(),
        UserEvaluatorId2: $(".servant-evaluator-second").val(),
    };
    $.post("/Structure/ModifyServantStructures", properties)

        .done(function (output) {
            if (output.stats == 1) {
                $(location).attr('href', '../Edit/' + properties.Id);
            } else if (output.stats == 2) {

                if (properties.InstituitionId == "") {
                    $(".instituition-validation-error").html('Selecione um órgão!');
                } else {
                    $(".instituition-validation-error").html('');
                }
                if (properties.DivisionId == "") {
                    $(".division-validation-error").html('Selecione uma divisão!');
                } else {
                    $(".division-validation-error").html('');
                }
                if (properties.SectionId == "") {
                    $(".section-validation-error").html('Selecione uma seção!');
                } else {
                    $(".section-validation-error").html('');
                }
                if (properties.SectorId == "") {
                    $(".sector-validation-error").html('Selecione um setor!');
                } else {
                    $(".sector-validation-error").html('');
                }if (properties.UserEvaluatorId1 == "") {
                    $(".first-eval-validation-error").html('Selecione o avaliador 1!');
                } else {
                    $(".first-eval-validation-error").html('');
                }if (properties.UserEvaluatorId2 == "") {
                    $(".second-eval-validation-error").html('Selecione o avaliador 2!');
                } else {
                    $(".second-eval-validation-error").html('');
                }

            } else if (output.stats == 3) {
                return false;
            }
        })

        .fail(function () {
            alert("Ocorreu um erro!");
        });
}

$(document).ready(function () {
    $("#edit-servant-structures-form").submit(function (e) {
        e.preventDefault();
        EditServantStructures();
    });
});