function generateReport() {
    const columnHeadersInputFields = Array.from(document.querySelectorAll('table > thead > tr > th > input'));
    const tableRows = Array.from(document.querySelectorAll('table > tbody > tr'));
    const textArea = document.getElementById('output');
    let resultArray = [];
    
    for (const tableRow of tableRows) {
        let currentObject = {};
       for (let i = 0; i < columnHeadersInputFields.length; i++) {
        let checked = columnHeadersInputFields[i].checked === true;
        if(checked){
                
                let currentKey = columnHeadersInputFields[i].parentElement.textContent.toLowerCase().trimEnd();
                let currentValue = tableRow.children[i].textContent;
                currentObject[currentKey] = currentValue;
            }
        }
        resultArray.push(currentObject);
    }
        textArea.textContent += JSON.stringify(resultArray, null, 2);
}
