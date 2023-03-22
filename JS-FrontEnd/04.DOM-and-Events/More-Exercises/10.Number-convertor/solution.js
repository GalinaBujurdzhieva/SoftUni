function solve() {

    const convertTo = document.getElementById('selectMenuTo');
    createElement('option', 'Binary', convertTo, '', '', {value: 'binary'});
    createElement('option', 'Hexadecimal', convertTo, '', '', {value: 'hexadecimal'});
    const convertButton = document.querySelector('#container button');
    const input = document.querySelector('#container input');
    const result = document.getElementById('result');
    convertButton.addEventListener('click', convertFunc);

    function convertFunc() {
        if (convertTo.value === 'binary') {
            result.value = Number(input.value).toString(2);
        }
        else if(convertTo.value === 'hexadecimal'){
            result.value = Number(input.value).toString(16).toUpperCase();
        }
    }

    function createElement(type, content, parentNode, id, classes, attributes){
        const htmlElement = document.createElement(type);
    
        if (content && type === 'input') {
          htmlElement.value = content;
        }
        if (content && type !== 'input') {
          htmlElement.textContent = content;
        }
        if (id) {
          htmlElement.id = id;
        }
        if (classes) {
          htmlElement.classList.add(...classes)
        }
        if (attributes) {
          for (const key in attributes) {
            htmlElement.setAttribute(key, attributes[key])
          }
        }
        if (parentNode) {
          parentNode.appendChild(htmlElement);
        }
        return htmlElement;
      }
}
