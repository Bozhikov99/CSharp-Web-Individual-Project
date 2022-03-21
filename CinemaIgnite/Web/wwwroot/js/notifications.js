let titleElements = document.querySelectorAll('.notification-title');

titleElements.forEach(te => te.addEventListener('click', (e) => {
    let targetElement = e.target;
    let notificationTextElement = targetElement.parentElement
        .querySelector('.notification-text');

    targetElement.style.setProperty('font-weight', 'normal');

    document.querySelectorAll('.notification-text')
        .forEach(nt => nt.setAttribute('hidden', true));

    notificationTextElement.removeAttribute('hidden');
}))