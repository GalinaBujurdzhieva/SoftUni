function focused() {
    const inputsArray = Array.from(document.querySelectorAll('div input'));
    inputsArray.map((inp) => inp.addEventListener('focus', focusFunc));
    inputsArray.map((inp) => inp.addEventListener('blur', blurFunc))
   
    function focusFunc(e) {
        e.currentTarget.parentElement.classList.add('focused');
    }
    function blurFunc(e) {
        e.currentTarget.parentElement.classList.remove('focused');
    }
}
