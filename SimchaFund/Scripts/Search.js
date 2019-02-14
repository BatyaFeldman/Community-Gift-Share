$(function () {
    $("#searchBar").on('keyup', function () {
        const text = $(this).val();
        $("#contribTable tr:gt(0)").each(function () {
            const tr = $(this);
            const name = tr.find('td:eq(1)').text();
            if (name.toLowerCase().indexOf(text.toLowerCase()) !== -1) {
                tr.show();
            } else {
                tr.hide();
            }
        });
    });

    $("#clear").on('click', function () {
        $("#searchBar").val('');
        $("tr").show();
    })
})