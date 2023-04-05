window.addEventListener("load", solve);

function solve() {
  inputDOMSelectors = {
    make: document.getElementById('make'),
    model: document.getElementById('model'),
    year: document.getElementById('year'),
    fuel: document.getElementById('fuel'),
    originalCost: document.getElementById('original-cost'),
    sellingPrice: document.getElementById('selling-price')
  }
  otherDOMSelectors = {
    form: document.querySelector('.container form'),
    publishBtn: document.getElementById('publish'),
    tableBody: document.getElementById('table-body'),
    soldCarsUnorderedList: document.getElementById('cars-list'),
    profit: document.getElementById('profit')
  }

  let inputsObj = {};
  let totalProfit = 0;

  otherDOMSelectors.publishBtn.addEventListener('click', createCarOffer);
  function createCarOffer(event){
    if(event){
      event.preventDefault();
    }
    if (!inputsValidator(inputDOMSelectors) || (Number(inputDOMSelectors.sellingPrice.value) < Number(inputDOMSelectors.originalCost.value))) {
      return;
    }
    for (const key in inputDOMSelectors) {
      inputsObj[key] = inputDOMSelectors[key].value;
    }
    const {make, model, year, fuel, originalCost, sellingPrice} = inputDOMSelectors;
    const tableRow = createElement('tr', '', otherDOMSelectors.tableBody, '', ['row']);
    createElement('td', `${make.value}`, tableRow);
    createElement('td', `${model.value}`, tableRow);
    createElement('td', `${year.value}`, tableRow);
    createElement('td', `${fuel.value}`, tableRow);
    createElement('td', `${originalCost.value}`, tableRow);
    createElement('td', `${sellingPrice.value}`, tableRow);
    const buttonsTableCell = createElement('td', '', tableRow);
    const editBtn = createElement('button', 'Edit', buttonsTableCell, '', ['action-btn', 'edit']);
    editBtn.addEventListener('click', returnDataInInputFields)
    const sellBtn = createElement('button', 'Sell', buttonsTableCell, '', ['action-btn', 'sell']);
    sellBtn.make = make.value;
    sellBtn.model = model.value;
    sellBtn.year = year.value;
    sellBtn.sellingPrice = sellingPrice.value;
    sellBtn.difference = Number(sellingPrice.value) - Number(originalCost.value);
    sellBtn.addEventListener('click', soldCarsHandler);
    for (const key in inputDOMSelectors) {
      inputDOMSelectors[key].value = '';
    }
  }

  function soldCarsHandler(){
    const tableRow = this.parentNode.parentNode;
    const children = Array.from(tableRow.children);
    console.log(children);
    const liElement = createElement('li', '', otherDOMSelectors.soldCarsUnorderedList, '', ['each-list']);
    createElement('span', `${this.make} ${this.model}`, liElement);
    createElement('span', this.year, liElement);
    createElement('span', this.difference, liElement);
    totalProfit += Number(this.difference);
    otherDOMSelectors.profit.textContent = totalProfit.toFixed(2);
    tableRow.remove();
  }

  function returnDataInInputFields(){
    const tableRow = this.parentNode.parentNode;
    for (const key in inputsObj) {
      inputDOMSelectors[key].value = inputsObj[key];
    }
    tableRow.remove();
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
