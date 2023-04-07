window.addEventListener('load', solution);

function solution() {
  inputDOMSelectors = {
    fullName: document.getElementById('fname'),
    email: document.getElementById('email'),
    phone: document.getElementById('phone'),
    address:document.getElementById('address'),
    code: document.getElementById('code')
  }

  otherDOMSelectors = {
    submitBtn: document.getElementById('submitBTN'),
    infoPreviewContainer: document.getElementById('infoPreview'),
    editBtn: document.getElementById('editBTN'),
    continueBtn: document.getElementById('continueBTN'),
    blockDiv: document.getElementById('block')
  }

  let inputsObj = {};
  
  otherDOMSelectors.submitBtn.addEventListener('click', loadInfoInPreviewList);
  otherDOMSelectors.editBtn.addEventListener('click', loadInfoInInputFields);
  otherDOMSelectors.continueBtn.addEventListener('click', completeReservation);

  function completeReservation(){
    otherDOMSelectors.blockDiv.innerHTML = '';
    createElement('h3', 'Thank you for your reservation!', otherDOMSelectors.blockDiv);
  }

  function loadInfoInInputFields(){
    for (const key in inputsObj) {
      inputDOMSelectors[key].value = inputsObj[key];
    }
    otherDOMSelectors.submitBtn.removeAttribute('disabled');
    otherDOMSelectors.editBtn.setAttribute('disabled', true);
    otherDOMSelectors.continueBtn.setAttribute('disabled', true);
    const liElements = Array.from(otherDOMSelectors.infoPreviewContainer.children);
    liElements.forEach((x) => x.remove());
  }

  function loadInfoInPreviewList(event){
    if (event) {
      event.preventDefault();
    }
    if (inputDOMSelectors.fullName.value.trim() === '' || inputDOMSelectors.email.value.trim() === '') {
      return;
    }
    for (const key in inputDOMSelectors) {
      inputsObj[key] = inputDOMSelectors[key].value;
    }
    const {fullName, email, phone, address, code} = inputDOMSelectors;
    createElement('li', `Full Name: ${fullName.value}`, otherDOMSelectors.infoPreviewContainer);
    createElement('li', `Email: ${email.value}`, otherDOMSelectors.infoPreviewContainer);
    createElement('li', `Phone Number: ${phone.value}`, otherDOMSelectors.infoPreviewContainer);
    createElement('li', `Address: ${address.value}`, otherDOMSelectors.infoPreviewContainer);
    createElement('li', `Postal Code: ${code.value}`, otherDOMSelectors.infoPreviewContainer);
    
    otherDOMSelectors.submitBtn.setAttribute('disabled', true);
    otherDOMSelectors.editBtn.removeAttribute('disabled');
    otherDOMSelectors.continueBtn.removeAttribute('disabled');
    for (const field of Object.values(inputDOMSelectors)) {
      field.value = '';
    }
  }

  function inputsValidator(obj){
    return Object.values(obj).every((inp) => inp.value.trim() !== '');
  }

  function createElement(type, content, parentNode, id, classes, attributes){
    const htmlElement = document.createElement(type);
  
    if (content && type === 'input') {
      htmlElement.value = content;
    }
    if (content && type !== 'input') {
      htmlElement.textContent = content;
    }
    if (id) {
      htmlElement.id = id;
    }
    if (classes) {
      htmlElement.classList.add(...classes)
    }
    if (attributes) {
      for (const key in attributes) {
        htmlElement.setAttribute(key, attributes[key])
      }
    }
    if (parentNode) {
      parentNode.appendChild(htmlElement);
    }
    return htmlElement;
  }
}
