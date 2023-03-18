function create(words) {
   for (const word of words) {
      const parentElement = document.getElementById('content');
      const divElement = document.createElement('div');
      const paragraphElement = document.createElement('p');
      paragraphElement.style.display = 'none';
      divElement.addEventListener('click', showText)
      paragraphElement.textContent = word;
      divElement.appendChild(paragraphElement);
      parentElement.appendChild(divElement);

      function showText(){
         paragraphElement.style.display = 'block';
      }
   }
}