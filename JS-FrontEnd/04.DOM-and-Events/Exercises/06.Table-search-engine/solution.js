function solve() {
   document.querySelector('#searchBtn').addEventListener('click', onClick);

   function onClick() {
      const searchedTextInput = document.getElementById('searchField');
      let searchedCharacters = searchedTextInput.value;
      const tableCells = Array.from(document.querySelectorAll('.container tbody tr'));
      
      for (const cell of tableCells) {
         const parentElement = cell.classList;
         if (parentElement.contains('select')) {
            parentElement.remove('select');
         }
         if(cell.textContent.includes(searchedCharacters) && searchedCharacters !== ''){
            parentElement.add('select');
         }
      }
      searchedTextInput.value = '';
   }
}
