function solve() {
  const [inputTextArea, outputTextArea] = Array.from(document.getElementsByTagName('textarea'));
  const [generateButton, buyButton] = Array.from(document.getElementsByTagName('button'));
  const tableBody = document.querySelector('.table > tbody');

  generateButton.addEventListener('click', addFurniture);
  buyButton.addEventListener('click', buyFurniture);

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

  function addFurniture(){
    const furnitureJsonArray = Array.from(JSON.parse(inputTextArea.value));
    for (const furniture of furnitureJsonArray) {
      let {name, img, price, decFactor} = furniture;
      const tableRow = createElement('tr', '', tableBody);
      const firstTableCell = createElement('td', '', tableRow);
      createElement('img', '', firstTableCell, '', '', { src: img });

      const secondTableCell = createElement('td', '', tableRow);
      createElement('p', name, secondTableCell);

      const thirdTableCell = createElement('td', '', tableRow);
      createElement('p', price, thirdTableCell);

      const fourthTableCell = createElement('td', '', tableRow);
      createElement('p', decFactor, fourthTableCell);

      const fifthTableCell = createElement('td', '', tableRow);
      createElement('input', '', fifthTableCell, '', '', { type: 'checkbox' })
    }
  }

  function buyFurniture(){
    const checkedInputs = Array.from(document.querySelectorAll('input[type="checkbox"]:checked'));
    let names = [];
    let totalPrice = 0;
    let totalDecFactor = 0;

    for (const input of checkedInputs) {
      const parentElem = input.parentElement.parentElement;
      const parentElemChildren = Array.from(parentElem.children);
      const furnitureName = parentElemChildren[1].children[0].textContent;
      names.push(furnitureName);
      const furniturePrice = Number(parentElemChildren[2].children[0].textContent);
      totalPrice += furniturePrice;
      const furnitureDecFactor = Number(parentElemChildren[3].children[0].textContent);
      totalDecFactor += furnitureDecFactor;
    }
    let output = '';
    output += `Bought furniture: ${names.join(', ')}\n`;
    output += `Total price: ${totalPrice.toFixed(2)}\n`;
    let averageDecFactor = totalDecFactor / names.length;
    output += `Average decoration factor: ${averageDecFactor}`;
    outputTextArea.value = output;
  }
}