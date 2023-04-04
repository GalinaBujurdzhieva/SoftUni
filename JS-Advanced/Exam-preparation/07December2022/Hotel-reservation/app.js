window.addEventListener('load', solve);

function solve() {
    inputDOMSelectors = {
        firstName: document.getElementById('first-name'),
        lastName: document.getElementById('last-name'),
        checkInDate: document.getElementById('date-in'),
        checkOutDate: document.getElementById('date-out'),
        peopleCount: document.getElementById('people-count'),
    }

    otherDOMSelectors = {
        nextBtn: document.getElementById('next-btn'),
        form: document.querySelector('#append-reservation .container-text form'),
        reservationInfoContainer: document.querySelector('#info-reservations .reservations-info-container .info-list'),
        confirmReservationContainer: document.querySelector('#confirm-reservations .confirm-container .confirm-list'),
        completeReservationHeader: document.getElementById('verification')
    }

    let inputObj = {};
    otherDOMSelectors.nextBtn.addEventListener('click', loadReservationInfo);

    function loadReservationInfo(event){
        if(event){
            event.preventDefault();
        }
        if(!InputValidator(inputDOMSelectors) || !compareDates(inputDOMSelectors.checkInDate.value, inputDOMSelectors.checkOutDate.value)){
           return;
        }
        for (const key in inputDOMSelectors) {
            inputObj[key]= inputDOMSelectors[key].value;
        }
        const liElement = createElement('li', '', otherDOMSelectors.reservationInfoContainer, '', ['reservation-content']);
        const article = createElement('article', '', liElement);
        createElement('h3', `Name: ${inputDOMSelectors.firstName.value} ${inputDOMSelectors.lastName.value}`, article);
        createElement('p', `From date: ${inputDOMSelectors.checkInDate.value}`, article);
        createElement('p', `To date: ${inputDOMSelectors.checkOutDate.value}`, article);
        createElement('p', `For ${inputDOMSelectors.peopleCount.value} people`, article);
        const editBtn = createElement('button', 'Edit', liElement, '', ['edit-btn']);
        editBtn.addEventListener('click', loadInfoInInputFields);
        const continueBtn = createElement('button', 'Continue', liElement, '', ['continue-btn']);
        continueBtn.addEventListener('click', transferInfoInConfirmSection)
        otherDOMSelectors.nextBtn.setAttribute('disabled', true);
        otherDOMSelectors.form.reset();
    }

    function transferInfoInConfirmSection(){
        const liElement = this.parentNode;
        otherDOMSelectors.confirmReservationContainer.appendChild(liElement);
        const confirmBtn = createElement('button', 'Confirm', liElement, '', ['confirm-btn']);
        confirmBtn.addEventListener('click', confirmReservation);
        const cancelBtn = createElement('button', 'Cancel', liElement, '', ['cancel-btn']);
        cancelBtn.addEventListener('click', cancelReservation);
        const editBtn = Array.from(liElement.children)[1];
        const continueBtn = Array.from(liElement.children)[2];
        editBtn.remove();
        continueBtn.remove();
        console.log(children);
    }

    function confirmReservation(){
      const liElement = this.parentNode;
      liElement.remove();
      otherDOMSelectors.nextBtn.removeAttribute('disabled');
      otherDOMSelectors.completeReservationHeader.textContent = 'Confirmed.';
      otherDOMSelectors.completeReservationHeader.classList.remove('reservation-cancelled')
      otherDOMSelectors.completeReservationHeader.classList.add('reservation-confirmed');
    }

    function cancelReservation(){
      const liElement = this.parentNode;
      liElement.remove();
      otherDOMSelectors.nextBtn.removeAttribute('disabled');
      otherDOMSelectors.completeReservationHeader.textContent = 'Cancelled.';
      otherDOMSelectors.completeReservationHeader.classList.remove('reservation-confirmed')
      otherDOMSelectors.completeReservationHeader.classList.add('reservation-cancelled')
    }

    function loadInfoInInputFields(){
        const liElement = this.parentNode;
        liElement.remove();
        for (const key in inputObj) {
           inputDOMSelectors[key].value = inputObj[key];
        }
        otherDOMSelectors.nextBtn.removeAttribute('disabled');
    }

    function InputValidator(obj){
        return areInputsNonEmptyStrings = Object.values(obj).every((inp) => inp.value !== '');
    }
    function compareDates(d1, d2){
        let date1 = new Date(d1);
        let date2 = new Date(d2);
      
        if (date1 < date2) {
          return true;
        } else {
          return false;
        }
      };

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



    
    
