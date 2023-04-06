window.addEventListener('load', solve);

function solve() {
    const inputDOMSelectors = {
        model: document.getElementById('model'),
        year: document.getElementById('year'),
        description: document.getElementById('description'),
        price: document.getElementById('price')
    }

    otherDOMSelectors = {
        form: document.querySelector('.store > form'),
        addBtn: document.getElementById('add'),
        tbody: document.getElementById('furniture-list'),
        totalPrice: document.querySelector('#information .total-price')
    }
    let totalPrice = 0;
    otherDOMSelectors.addBtn.addEventListener('click', loadTableInfoHandler);

    function loadTableInfoHandler(event){
        if (event) {
            event.preventDefault();
        }
        const {model, year, description, price} = inputDOMSelectors;
        let correctYear = Number(year.value) > 0;
        let correctPrice = Number(price.value) > 0;

        if (!inputsValidator(inputDOMSelectors) || !correctYear || !correctPrice) {
            return;
        }
        const mainFurnitureInfoRow = createElement('tr', '', otherDOMSelectors.tbody, '', ['info']);
        createElement('td', model.value, mainFurnitureInfoRow);
        createElement('td', `${Number(price.value).toFixed(2)}`, mainFurnitureInfoRow);
        const buttonsTd = createElement('td', '', mainFurnitureInfoRow);
        const moreInfoBtn = createElement('button', 'More Info', buttonsTd, '', ['moreBtn']);
        moreInfoBtn.addEventListener('click', showMoreOrLessInfo);
        const buyBtn = createElement('button', 'Buy it', buttonsTd, '', ['buyBtn']);
        buyBtn.addEventListener('click', buyFurniture)
        const hiddenFurnitureInfoRow = createElement('tr', '', otherDOMSelectors.tbody, '', ['hide']);
        createElement('td', `Year: ${year.value}`, hiddenFurnitureInfoRow);
        createElement('td', `Description: ${description.value}`, hiddenFurnitureInfoRow, '', '', {colspan: '3'}),
        otherDOMSelectors.form.reset();
    }

    function buyFurniture(){
        const tableRowWithClassInfo = this.parentNode.parentNode;
        const price = Array.from(tableRowWithClassInfo.children)[1];
        totalPrice += Number(price.textContent);
        otherDOMSelectors.totalPrice.textContent = totalPrice.toFixed(2);
        const allTableRowsArray = Array.from(otherDOMSelectors.tbody.children);
        const index = allTableRowsArray.indexOf(tableRowWithClassInfo);
        const tableRowWithClassHidden = allTableRowsArray[index + 1];
        tableRowWithClassInfo.remove();
        tableRowWithClassHidden.remove();
    }

    function showMoreOrLessInfo(){
        const additionalInfoSection = Array.from(otherDOMSelectors.tbody.children)[1];
        if (this.textContent === 'More Info') {
            additionalInfoSection.style.display = 'contents';
            this.textContent = 'Less Info';
        }
        else if (this.textContent === 'Less Info'){
            this.textContent = 'More Info';
            additionalInfoSection.style.display = 'none';
        }
    }
    function inputsValidator(obj){
       return Object.values(obj).every((inp) => inp.value.trim() !== '');
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
