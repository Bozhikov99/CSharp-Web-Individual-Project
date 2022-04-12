let form = document.querySelector('.language-form');
let select = document.querySelector('#language-select');
let flagElements = document.querySelectorAll('.language-flag');

flagElements.forEach(fe => fe.addEventListener('click', () => {
    let value = fe.dataset.value;
    select.value = value;
    form.submit();
}))