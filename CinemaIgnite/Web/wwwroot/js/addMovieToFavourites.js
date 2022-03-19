let favButtonElement = document.querySelector('#fav-button');
let movieId = favButtonElement.dataset.movieId;
console.log(favButtonElement.textContent);
console.log(movieId);


favButtonElement.addEventListener('click', () => {
    let favButtonElement = document.querySelector('#fav-button');
    console.log(favButtonElement.textContent);

    favButtonElement.addEventListener('click', () => {
        console.log('test')
        $.ajax({
            type: "POST",
            url: "/Movie/AddMovieToFavourites",
            data: { id: movieId }
        })
            .then((result) => {
                console.log('YES');
                favButtonElement.textContent = 'Unfav it';
                favButtonElement.classList.remove('btn-danger');
                favButtonElement.classList.add('btn-warning');
            })
    });
});
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