let buttonElements = document.querySelectorAll('button');
let inputElement = document.querySelector('#rating-input');
let rateButtonElement = document.querySelector('#rate');
let rateDivElement = document.querySelector('.rate-div');
let rateToggleElement = document.querySelector('#rate-toggle');

rateToggleElement.addEventListener('click', () => {
    rateDivElement.removeAttribute('hidden');
})

buttonElements.forEach(b => b.addEventListener('click', () => {
    let value = b.value;

    inputElement.value = value;
    console.log(inputElement.value);
/*    rateButtonElement.classList.remove('disabled');*/
    rateButtonElement.classList.add('btn-warning');
}))

rateButtonElement.addEventListener('click', () => {
    rateDivElement.hidden = 'true';
})
