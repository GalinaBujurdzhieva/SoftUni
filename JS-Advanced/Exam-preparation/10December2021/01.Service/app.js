window.addEventListener('load', solve);

function solve() {
    const inputDOMSelectors = {
        productType: document.getElementById('type-product'),
        description: document.getElementById('description'),
        clientName: document.getElementById('client-name'),
        clientPhone: document.getElementById('client-phone')
    }

    const otherDOMSelectors = {
        sendFormButton: document.querySelector('#right > form > button'),
        form: document.querySelector('#right > form'),
        receivedOrdersContainer: document.getElementById('received-orders'),
        completedOrdersContainer: document.getElementById('completed-orders'),
        clearOrdersBtn: document.querySelector('#completed-orders > button')
    }

    otherDOMSelectors.sendFormButton.addEventListener('click', moveToReceivedOrdersContainer);
    otherDOMSelectors.clearOrdersBtn.addEventListener('click', clearCompletedOrdersContainer);

    function clearCompletedOrdersContainer(){
        const parentSectionElement = this.parentNode;
        const divsToBeRemoved = Array.from(parentSectionElement.getElementsByTagName('div'));
        divsToBeRemoved.forEach((div) => div.remove());
    }

    function moveToReceivedOrdersContainer(event){
        if (event) {
            event.preventDefault();
        }
        if (!inputsValidator(inputDOMSelectors)) {
            return;
        }
        const {productType, description, clientName, clientPhone} = inputDOMSelectors;
        const containerDiv = createElement('div', '', otherDOMSelectors.receivedOrdersContainer, '', ['container']);
        createElement('h2', `Product type for repair: ${productType.value}`, containerDiv);
        createElement('h3', `Client information: ${clientName.value}, ${clientPhone.value}`, containerDiv);
        createElement('h4', `Description of the problem: ${description.value}`, containerDiv);
        const startRepairBtn = createElement('button', 'Start repair', containerDiv, '', ['start-btn']);
        startRepairBtn.addEventListener('click', startingRepairHandler)
        const finishRepairBtn = createElement('button', 'Finish repair', containerDiv, '', ['finish-btn'], {disabled: true});
        finishRepairBtn.addEventListener('click', moveToCompletedOrdersHandler);
        otherDOMSelectors.form.reset();
    }

    function moveToCompletedOrdersHandler(){
        const containerDiv = this.parentNode;
        otherDOMSelectors.completedOrdersContainer.appendChild(containerDiv);
        const [startRepairBtn, finishRepairBtn] = containerDiv.getElementsByTagName('button'); 
        startRepairBtn.remove();
        finishRepairBtn.remove();
    }

    function startingRepairHandler(){
        this.setAttribute('disabled', true);
        const finishRepairBtn = Array.from(this.parentNode.children)[4];
        finishRepairBtn.removeAttribute('disabled');
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