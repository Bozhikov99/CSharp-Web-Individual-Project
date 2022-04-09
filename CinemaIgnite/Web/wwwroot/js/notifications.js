let titleElements = document.querySelectorAll('.notification-title');
let deleteElement = document.querySelector('.fa-trash');
let checkElement = document.querySelector('#check-all');

checkElement.addEventListener('change', () => {
    if (checkElement.checked) {
        let inputElements = document.querySelectorAll('input[type="checkbox"]');
        let inputs = Array.from(inputElements);
        inputs.forEach(i => i.checked = true);
    }
    else {
        let inputElements = document.querySelectorAll('input[type="checkbox"]');
        let inputs = Array.from(inputElements);
        inputs.forEach(i => i.checked = false);
    }
})

deleteElement.addEventListener('click', () => {
    let inputElements = document.querySelectorAll('input[type="checkbox"]');
    let inputs = Array.from(inputElements);

    if (inputs.some(i => i.checked)) {
        let formElement = document.querySelector('#delete-form');
        formElement.submit();
    }
})

titleElements.forEach(te => te.addEventListener('click', (e) => {
    let targetElement = e.target;
    let notificationDivElements = document.querySelectorAll('.notification-div');
    let currentDivElement = targetElement.parentElement.parentElement;
    let notificationTextElement = targetElement.parentElement.parentElement
        .querySelector('.notification-text');

    let isShown = notificationTextElement.hasAttribute('hidden');

    notificationDivElements.forEach(nde => nde.style.backgroundColor = '#333333');
    currentDivElement.style.backgroundColor = '#666666';
    targetElement.style.setProperty('font-weight', 'normal');

    document.querySelectorAll('.notification-text')
        .forEach(nt => nt.setAttribute('hidden', true));

    if (isShown) {
        notificationTextElement.removeAttribute('hidden');
    }
}))