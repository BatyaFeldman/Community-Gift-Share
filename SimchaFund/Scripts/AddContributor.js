
$(function () {

    $(".newContributor").on('click', function () {
        $("#contributorModal").modal();
    });


    $(".deposit").on('click', function () {
        const contribId = $(this).data('contribid');
        const contribName = $(this).data('contribname');
        $("#contributorId").val(contribId);
        $("#depositName").text(contribName);
        $("#depositModal").modal();
    });

    $(".edit-contrib").on('click', function () {

        
        const Id = $(this).data('id');
        const Name = $(this).data('name');
        const PhoneNumber = $(this).data('number');
        const Date = $(this).data('date');
        const AlwaysInclude = $(this).data('alwaysinclude');

        $("#initial-deposit").hide();

        $('#id').val(Id);
        $('#name').val(Name);
        $('#number').val(PhoneNumber);
        $('#date').val(Date);
        $('#alwaysInclude').prop('checked', AlwaysInclude == "True");

        $("#titleText").text("Edit Contributor");

        const form = $("#contribForm");
        const hidden = $(`<input type='hidden' name='Id' value='${Id}' />`);
        form.append(hidden);

        form.attr("action", "/Contributor/EditContributor");


        $('#contributorModal').modal();
    })

});