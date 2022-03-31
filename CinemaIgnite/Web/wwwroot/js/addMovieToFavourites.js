let favIconElement = document.querySelector('.fav-icon');

favIconElement.addEventListener('mouseover', () => {
    console.log('over');
    if (favIconElement.classList.contains('fa-heart-o')) {
        favIconElement.classList.remove('fa-heart-o');
        favIconElement.classList.add('fa-heart');
    }
    else {
        favIconElement.classList.remove('fa-heart');
        favIconElement.classList.add('fa-heart-o');
    }
});

favIconElement.addEventListener('mouseout', () => {
    if (favIconElement.classList.contains('fa-heart-o')) {
        favIconElement.classList.remove('fa-heart-o');
        favIconElement.classList.add('fa-heart');
    }
    else {
        favIconElement.classList.remove('fa-heart');
        favIconElement.classList.add('fa-heart-o');
    }
})

favIconElement.addEventListener('click', () => {
    if (favIconElement.classList.contains('fa-heart-o')) {
        favIconElement.classList.remove('fa-heart-o');
        favIconElement.classList.add('fa-heart');
    }
    else {
        favIconElement.classList.remove('fa-heart');
        favIconElement.classList.add('fa-heart-o');
    }
})
//var data = { id: 1 };

//$.ajax({
//    type: "POST",
//    url: 'https://localhost:44395/Movie/AddMovieToFavourites/',
//    data: data,
//    success: function (resultData) {
//        // take the result data and update the div
//        $("#playlistDiv").append(resultData.html)
//    },
//    dataType: dataType
//});