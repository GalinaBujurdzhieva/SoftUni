function sumTable() {
    const tableCostColumn = Array.from(document.querySelectorAll('table tbody tr td:nth-child(2)'));
    let sum = 0;
    for (const cost of tableCostColumn) {
        sum += Number(cost.textContent);
    }
    const result = document.getElementById('sum');
    result.textContent = sum;
}
