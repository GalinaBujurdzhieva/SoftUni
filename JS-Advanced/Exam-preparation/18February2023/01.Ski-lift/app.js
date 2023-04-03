window.addEventListener('load', solve);

function solve() {
   const inputDOMSelectors = {
    firstName: document.getElementById('first-name'),
    lastName: document.getElementById('last-name'),
    peopleCount: document.getElementById('people-count'),
    date: document.getElementById('from-date'),
    daysCount: document.getElementById('days-count')
   }

   const otherDOMSelectors = {
    nextBtn: document.getElementById('next-btn'),
    ticketInfoListContainer: document.querySelector('#info-ticket .first-container .ticket-info-list'),
    confirmTicketContainer: document.querySelector('#confirm-ticket-section .first-container .confirm-ticket'),
    form: document.querySelector('.container-text form'),
    main: document.getElementById('main'),
    body: document.getElementById('body')
   } 

   let inputObj ={};

   function inputValidator(obj){
    return Object.values(obj).every((inp) => inp.value !== '');
   }

   otherDOMSelectors.nextBtn.addEventListener('click', addTicketInfoToInfoContainer)

   function addTicketInfoToInfoContainer(event){
    if (event){
        event.preventDefault();
    }
    if(!inputValidator(inputDOMSelectors)){
        return;
   }
   for (const key in inputDOMSelectors) {
    inputObj[key] = inputDOMSelectors[key].value;
   }
   const {firstName, lastName, peopleCount, date, daysCount} = inputDOMSelectors;
   const liElement = createElement('li', '', otherDOMSelectors.ticketInfoListContainer, '', ['ticket']);
   const article = createElement('article', '', liElement);
   createElement('h3', `Name: ${firstName.value} ${lastName.value}`, article);
   createElement('p', `From date: ${date.value}`, article);
   createElement('p', `For ${daysCount.value} days`, article);
   createElement('p', `For ${peopleCount.value} people`, article);
   const editBtn = createElement('button', 'Edit', liElement, '', ['edit-btn']);
   editBtn.addEventListener('click', loadBackInputForm);
   const continueBtn = createElement('button', 'Continue', liElement, '', ['continue-btn']);
   continueBtn.addEventListener('click', transferInfoToConfirmSection)
   otherDOMSelectors.form.reset();
   otherDOMSelectors.nextBtn.setAttribute('disabled', true);
}

function transferInfoToConfirmSection(){
    const liElement = this.parentNode;
    otherDOMSelectors.confirmTicketContainer.appendChild(liElement);
    liElement.classList.remove('ticket');
    liElement.classList.add('ticket-content')
    const [_article, editBtn, continueBtn] = Array.from(liElement.children);
    editBtn.remove();
    continueBtn.remove();
    const confirmBtn = createElement('button', 'Confirm', liElement, '', ['confirm-btn']);
    confirmBtn.addEventListener('click', confirmTicket)
    const cancelBtn = createElement('button', 'Cancel', liElement, '', ['cancel-btn']);
    cancelBtn.addEventListener('click', cancelTicket);
}

function cancelTicket(){
    const liElement = this.parentNode;
    liElement.remove();
    otherDOMSelectors.nextBtn.removeAttribute('disabled');
}

function confirmTicket(){
    otherDOMSelectors.main.remove();
    createElement('h1', 'Thank you, have a nice day!', otherDOMSelectors.body, 'thank-you');
    const backBtn = createElement('button', 'Back', otherDOMSelectors.body, 'back-btn');
    backBtn.addEventListener('click', reloadPage);
}

function reloadPage(){
    window.location.reload();
}

function loadBackInputForm(){
    for (const key in inputObj) {
        inputDOMSelectors[key].value = inputObj[key];
       }
    otherDOMSelectors.ticketInfoListContainer.innerHTML = '';
    otherDOMSelectors.nextBtn.removeAttribute('disabled');
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
    
