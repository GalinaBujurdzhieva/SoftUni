function lockedProfile() {
   const profileInfoArray = Array.from(document.getElementsByClassName('profile'));
   const firstInputs = Array.from(document.querySelectorAll('.profile input[value="lock"]'));
   for (const input of firstInputs) {
      const divParentElement = input.parentElement;
      childrenArray = Array.from(divParentElement.children);
      const divToBeShown = childrenArray[9];
      const button = childrenArray[10];
      button.addEventListener('click', showHiddenDiv);
      function showHiddenDiv(e){
         if (!input.checked) {
            if(e.currentTarget.textContent === 'Show more'){
               divToBeShown.style.display = 'block';
               e.currentTarget.textContent = 'Hide it';
            }
            else if(e.currentTarget.textContent === 'Hide it'){
               divToBeShown.style.display = 'none';
               e.currentTarget.textContent = 'Show more';
            }
      }
      }
   }
}
