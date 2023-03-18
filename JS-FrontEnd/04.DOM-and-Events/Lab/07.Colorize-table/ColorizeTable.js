function colorize() {
    const tableRowsArray = Array.from(document.querySelectorAll('table tr:nth-child(even)'));
    for (const tableRow of tableRowsArray) {
        tableRow.style.backgroundColor = 'Teal';
    }
}
