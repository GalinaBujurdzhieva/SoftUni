function editElement(element, match, replacer) {
    const elementTextContent = element.textContent;
    element.textContent = replaceAll(elementTextContent, match,replacer);

    function replaceAll(string, search, replace){
        return string.split(search).join(replace);
    }
}
