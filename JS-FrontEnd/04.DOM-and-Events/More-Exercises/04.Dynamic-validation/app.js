function validate() {
    const inputElement = document.getElementById('email');
    inputElement.addEventListener('change', EmailValidationFunc);
    const regex = /^([a-z]+)@([a-z]+)\.([a-z]+)$/;
    
    function EmailValidationFunc(e){
        const inputValue = e.currentTarget.value;
        if (!regex.test(inputValue)) {
            e.currentTarget.classList.add('error');
        }
        else if (regex.test(inputValue)) {
            e.currentTarget.classList.remove('error');
        }
    }
}
