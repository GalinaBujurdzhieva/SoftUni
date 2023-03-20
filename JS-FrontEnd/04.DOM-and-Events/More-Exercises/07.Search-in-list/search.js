function search() {
   const listItems = Array.from(document.getElementsByTagName('li'));
   const searchedText = Array.from(document.getElementsByTagName('input'))[0];
   const result = document.getElementById('result');
   let counter = 0;

   for (const listItem of listItems) {
      if (listItem.textContent.includes(searchedText.value)) {
         counter++;
         listItem.style.textDecoration = 'underline';
         listItem.style.fontWeight = 'bold';
         result.textContent = `${counter} matches found`;
      }
   }
   searchedText.value = '';
}
