let buttonElements = document.querySelectorAll('button');
let inputElement = document.querySelector('#rating-input');
let rateButtonElement = document.querySelector('#rate');
let rateDivElement = document.querySelector('.rate-div');

buttonElements.forEach(b => b.addEventListener('click', () => {
    let value = b.value;
    inputElement.value = value;
    rateButtonElement.disabled = false;
}))

rateButtonElement.addEventListener('click', () => {
    rateDivElement.hidden='true';
})
