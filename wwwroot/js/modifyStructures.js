function EditStructures() {
    let properties = {
        Id: $(".modify-structure-Id").val(),
        InstituitionId: $(".evaluator-instituition").val(),
        DivisionId: $(".evaluator-division").val(),
        SectionId: $(".evaluator-section").val(),
        SectorId: $(".evaluator-sector").val(),
    };
    $.post("/Structure/ModifyStructures", properties)

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
    $("#edit-structures-form").submit(function (e) {
        e.preventDefault();
        EditStructures();
    });
});