let buttonElements = document.querySelectorAll('button');
let inputElement = document.querySelector('#rating-input');
let rateButtonElement = document.querySelector('#rate');
let rateDivElement = document.querySelector('.rate-div');
let rateToggleElement = document.querySelector('#rate-toggle');
let rateIconElement = document.querySelector('.rate-icon');
let closeButtonElement = document.querySelector('.close-button');

rateIconElement.addEventListener('mouseover', toggleClass);
rateIconElement.addEventListener('mouseout', toggleClass);

rateIconElement.addEventListener('click', () => {
    rateDivElement.removeAttribute('hidden');
})

buttonElements.forEach(b => b.addEventListener('click', () => {
    let value = b.value;

    inputElement.value = value;
    console.log(inputElement.value);
    rateButtonElement.removeAttribute('disabled');
    rateButtonElement.classList.add('btn-warning');
}))

rateButtonElement.addEventListener('click', () => {
    rateDivElement.hidden = 'true';
})

closeButtonElement.addEventListener('click', () => {
    rateDivElement.hidden = 'true';
})

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