function extractText() {
    const listItems = document.getElementsByTagName('li');
    const result = document.getElementById('result');
    let listItemsAsArray = Array.from(listItems);
    for (const ListItem of listItemsAsArray) {
        result.textContent += ListItem.textContent + '\n';
    }
}