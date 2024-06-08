$.each(['mousemove', 'click'], function (k, v) {
    $('#MasteryR').on(v, function () {
        $('#MasteryL').text("- " + $('#MasteryR').val() + "%");
    });
})

