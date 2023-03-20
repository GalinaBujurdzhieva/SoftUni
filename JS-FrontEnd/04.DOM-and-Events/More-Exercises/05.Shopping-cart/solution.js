function solve() {
   const addButtons = Array.from(document.getElementsByClassName('add-product'));
   const checkoutButton = Array.from(document.getElementsByClassName('checkout'))[0];
   const textArea = Array.from(document.getElementsByTagName('textarea'))[0];
   let productNamesSet = new Set();
   let totalPrice = 0;

   for (const button of addButtons) {
      button.addEventListener('click', addProductFunc);
   }
   checkoutButton.addEventListener('click', checkoutFunc);

   function addProductFunc(e){
   const productDiv = e.currentTarget.parentElement.parentElement;
   const productName = productDiv.children[1].children[0].textContent;
   const productPrice = productDiv.children[3].textContent;
   productNamesSet.add(productName);
   totalPrice += Number(productPrice);
   textArea.value += `Added ${productName} for ${Number(productPrice).toFixed(2)} to the cart.\n`;
   }

   function checkoutFunc(e){
      textArea.value += `You bought ${Array.from(productNamesSet).join(', ')} for ${totalPrice.toFixed(2)}.`;
      e.currentTarget.disabled = true;
      addButtons.forEach((btn) => btn.disabled = true);
   }
}
