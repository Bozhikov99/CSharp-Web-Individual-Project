$("#image-input").focusout(function () {
    let imageUrl = $("#image-input").val();

    if (imageUrl.length != 0) {
        $('#movie-image').attr('src', imageUrl);
    }
})