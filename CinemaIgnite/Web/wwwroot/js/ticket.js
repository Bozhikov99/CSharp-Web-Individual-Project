let seatCountElement = document.querySelector('#count');
let priceElement = document.querySelector('#price');
let priceCalcElement = document.querySelector('#total');

let seatWrapperElement = document.querySelector('.seats-wrapper');
let seatElements = seatWrapperElement.querySelectorAll('.seat');
let submitButtonElement = document.querySelector('#buy-button');

seatElements.forEach(se => se.addEventListener('click', () => {
    let currentElement = se;
    let checkbox = currentElement.querySelector('input');

    if (se.classList.contains('free')) {
        checkbox.checked = true;
    } else if (se.classList.contains('selected')) {
        checkbox.checked = false;
    }
    toggleClass(se);
    updateInfo();
    checkButton();
}));

function toggleClass(element) {
    if (element.classList.contains('free')) {
        element.classList.remove('free');
        element.classList.add('selected');
    } else {
        element.classList.remove('selected');
        element.classList.add('free');
    }
}

function updateInfo() {
    let seatsSelectedArray = seatWrapperElement.querySelectorAll('.selected');
    let priceNum = Number(priceElement.textContent.replace(',', '.'));

    let seatsSelectedCount = seatsSelectedArray.length;
    let sum = seatsSelectedCount * priceNum;

    if (sum!=0) {
        sum = sum.toFixed(2)
    }

    seatCountElement.textContent = seatsSelectedCount;
    priceCalcElement.textContent = sum;
}

function checkButton() {
    let seatsSelectedArray = seatWrapperElement.querySelectorAll('.selected');


    if (seatsSelectedArray.length > 0) {
        submitButtonElement.style.display = 'block';
    } else {
        submitButtonElement.style.display='none';
    }
}