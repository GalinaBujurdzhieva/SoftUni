function encodeAndDecodeMessages() {
    const textAreas = Array.from(document.querySelectorAll('#main textarea'));
    const textToBeCodedArea = textAreas[0];
    const textToBeDecodedArea = textAreas[1];
    const buttons = Array.from(document.querySelectorAll('#main button'));
    const codeBtn = buttons[0];
    const decodeBtn = buttons[1];
    
    codeBtn.addEventListener('click', codeFunc);
    decodeBtn.addEventListener('click', decodeFunc);

    function codeFunc(){
        let currentArray = Array.from(textToBeCodedArea.value);
        currentArray = currentArray.map((char) => char.charCodeAt(0) + 1).map((code) => String.fromCharCode(code));
        textToBeDecodedArea.value = currentArray.join('');
        textToBeCodedArea.value = '';
    }
    function decodeFunc(){
        let currentArray = Array.from(textToBeDecodedArea.value);
        currentArray = currentArray.map((char) => char.charCodeAt(0) - 1).map((code) => String.fromCharCode(code));
        textToBeDecodedArea.value = currentArray.join('');
    }
}
