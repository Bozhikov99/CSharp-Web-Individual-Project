let dateInputElement = document.querySelector('#projectionDate');
dateInputElement.addEventListener('change', loadProgram);

function loadProgram(e) {
    let inputElement = e.target;
    let date = inputElement.value;

    let url = `All?date=${date}`;
    window.location.href = url;
}