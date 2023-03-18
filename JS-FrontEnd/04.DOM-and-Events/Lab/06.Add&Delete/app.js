function addItem() {
    const inputField = document.getElementById('newItemText');
    const newListItem = document.createElement('li');
    newListItem.textContent = inputField.value;
    const unorderedList = document.getElementById('items');
    const newAnchor = document.createElement('a');
    newAnchor.textContent = '[Delete]';
    newAnchor.href = '#';
    newListItem.appendChild(newAnchor);
    unorderedList.appendChild(newListItem);
    inputField.value = '';

    newAnchor.addEventListener('click', deleteFunc);

    function deleteFunc(e){
        e.currentTarget.parentElement.remove();
    }
}
