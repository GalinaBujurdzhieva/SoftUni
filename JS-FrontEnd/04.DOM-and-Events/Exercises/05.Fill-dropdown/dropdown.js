function addItem() {
    const textField = document.getElementById('newItemText');
    const valueField = document.getElementById('newItemValue');
    const parentSelectElement = document.getElementById('menu');
    console.log(parentSelectElement);
    const newOptionElement = document.createElement('option');
    newOptionElement.textContent = textField.value;
    newOptionElement.value = valueField.value;
    parentSelectElement.appendChild(newOptionElement);
    textField.value = '';
    valueField.value = '';
}
