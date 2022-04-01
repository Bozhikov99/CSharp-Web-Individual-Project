let modalContainerElement = document.querySelector('#modal-div');
let modalCloseElement = document.querySelector('#close');
let rateIconElement = document.querySelector('.rate-icon');
let starInputElements = document.querySelectorAll('.star-input');
let ratingVisualizerElement = document.querySelector('#rating-visualizer');
let rateElement = document.querySelector('#modal-rate');
let actualVisualizer = document.querySelector('#rating-visualizer');

rateIconElement.addEventListener('mouseover', toggleClass);
rateIconElement.addEventListener('mouseout', toggleClass);

//open modal
rateIconElement.addEventListener('click', () => {
    modalContainerElement.removeAttribute('hidden');
})

//close modal
modalCloseElement.addEventListener('click', () => {
    modalContainerElement.hidden = 'true';
})

//get rating value
starInputElements.forEach(si => si.addEventListener('click', () => {
    let value = Number(si.dataset.rating);
    let allInputs = Array.from(starInputElements);

    allInputs.forEach((i) => {
        i.classList.remove('fa-star');
        i.classList.add('fa-star-o');
    });

    for (var i = 0; i < value; i++) {
        allInputs[i].classList
            .remove('fa-star-o');

        allInputs[i].classList
            .add('fa-star');
    }

    ratingVisualizerElement.value = value;
    actualVisualizer.value = value;
    rateElement.removeAttribute('hidden');

    if (actualVisualizer.value == 10) {
        actualVisualizer.style.right = '23.9vw';
    } else {
        actualVisualizer.style.right = '23.3vw';
    }

}));

//rate
rateElement.addEventListener('click', () => {
    modalContainerElement.hidden = 'true';
});

function toggleClass(e) {
    let icon = e.target;
    if (icon.classList.contains('fa-star-o')) {
        icon.classList.remove('fa-star-o');
        icon.classList.add('fa-star');
    }
    else {
        icon.classList.remove('fa-star');
        icon.classList.add('fa-star-o');
    }
}