function addItem() {
    const inputElement = document.getElementById('newItemText');
    let inputElementValue = inputElement.value;
    const newList = document.createElement('li');
    newList.textContent = inputElementValue;
    const unorderedList = document.getElementById('items');
    unorderedList.appendChild(newList);
    inputElement.value = '';
}