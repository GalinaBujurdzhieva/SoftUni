function deleteByEmail() {
   const tableRowsWithEmails = document.querySelectorAll('table tr td:nth-child(Even)');
   let arrayWithTableRows = Array.from(tableRowsWithEmails);
   const input = document.querySelector('label input');
   let currentInputValue = input.value;
   const result = document.getElementById('result');
   let writtenEmail = arrayWithTableRows.find((email) => email.textContent === currentInputValue);

   if (writtenEmail) {
    result.textContent = 'Deleted.';
    writtenEmail.parentElement.remove();
   }
   else {
    result.textContent = 'Not found.';
   }
   input.value = '';
}