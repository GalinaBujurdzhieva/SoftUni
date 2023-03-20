function solve() {
  const textInput = document.getElementById('text');
  const namingConventionInput = document.getElementById('naming-convention');
  const resultSpan = document.getElementById('result');
  const transformButton = document.querySelector('body > form > input');
  
  if (namingConventionInput.value === 'Camel Case') {
    let splittedText = textInput.value.split(' ');
    let firstWord = splittedText[0].toLowerCase();

    splittedText = splittedText.splice(1).map((w) => capitalizeText(w));
    resultSpan.textContent = firstWord + '' + splittedText.join('');
    
  }
  else if (namingConventionInput.value === 'Pascal Case'){
    let splittedText = textInput.value.split(' ');
    splittedText = splittedText.map((w) => capitalizeText(w));
    resultSpan.textContent = splittedText.join('');
  }
  else {
    resultSpan.textContent = 'Error!'
  }

  function capitalizeText(word){
    return word.charAt(0).toUpperCase() + word.slice(1).toLowerCase();
  }
}
