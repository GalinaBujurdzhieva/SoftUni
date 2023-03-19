function solve() {
  const textarea = document.getElementById('input');
  const textArray = textarea.value.toString().split('.').filter(x => x !== '');
  const resultDiv = document.getElementById('output');

  while (textArray.length > 0){
    let newlyCreatedParagraph = textArray.splice(0,3).map(x => x.trimStart()).join('. ') + '.';
    const newParagraph = document.createElement('p');
    newParagraph.textContent = newlyCreatedParagraph;
    resultDiv.appendChild(newParagraph);
  }
}
