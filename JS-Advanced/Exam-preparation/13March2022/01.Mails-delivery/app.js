function solve() {
    const inputDOMSelectors = {
        recipientName: document.getElementById('recipientName'),
        title: document.getElementById('title'),
        message: document.getElementById('message')
    }
    const otherDOMSelectors = {
        form: document.querySelector('.message > .addMails > form'),
        addBtn: document.getElementById('add'),
        resetBtn: document.getElementById('reset'),
        listOfMailsContainer: document.getElementById('list'),
        sentMailsContainer: document.querySelector('.sent-mails .sent-list'),
        deletedMailsContainer: document.querySelector('.trash .delete-list')
    }
    otherDOMSelectors.addBtn.addEventListener('click', addMailToListOfMails);
    otherDOMSelectors.resetBtn.addEventListener('click', resetForm);

    function resetForm(event){
        if (event) {
            event.preventDefault();
        }
        for (const key in inputDOMSelectors) {
            inputDOMSelectors[key].value = '';
        }
    }

    function addMailToListOfMails(event){
        if (event) {
            event.preventDefault();
        }
        if (!inputsValidator(inputDOMSelectors)) {
            return;
        }
        const {recipientName, title, message} = inputDOMSelectors;
        const liElement = createElement('li', '', otherDOMSelectors.listOfMailsContainer);
        createElement('h4', `Title: ${title.value}`, liElement);
        createElement('h4', `Recipient Name: ${recipientName.value}`, liElement);
        createElement('span', message.value, liElement);
        const buttonsDiv = createElement('div', '', liElement, 'list-action');
        const sendBtn = createElement('button', 'Send', buttonsDiv, 'send', '', {type: 'submit'});
        const deleteBtn = createElement('button', 'Delete', buttonsDiv, 'delete', '', {type: 'submit'});
        sendBtn.addEventListener('click', transferMailToSentMails);
        deleteBtn.addEventListener('click', sendInfoFromListOfMailsToDeletedMails);
        for (const key in inputDOMSelectors) {
            inputDOMSelectors[key].value = '';
        }
    }

    function sendInfoFromListOfMailsToDeletedMails(){
        const liToBeRemoved = this.parentNode.parentNode;
        console.log(liToBeRemoved);
        const children = Array.from(this.parentNode.parentNode.children);
        const title = children[0].textContent;
        const to = children[1].textContent.replace('Recipient Name', 'To');
        const liElement = createElement('li', '', otherDOMSelectors.deletedMailsContainer);
        createElement('span', to, liElement);
        createElement('span', title, liElement);
        liToBeRemoved.remove();
    }

    function transferMailToSentMails(){
        const liToBeRemoved = this.parentNode.parentNode;
        const children = Array.from(this.parentNode.parentNode.children);
        const title = children[0].textContent;
        const to = children[1].textContent.replace('Recipient Name', 'To');
        const liElement = createElement('li', '', otherDOMSelectors.sentMailsContainer);
        createElement('span', to, liElement);
        createElement('span', title, liElement);
        const deleteBtnDiv = createElement('div', '', liElement, '', ['btn']);
        const deleteBtn = createElement('button', 'Delete', deleteBtnDiv, '', ['delete'], {type: 'submit'});
        deleteBtn.addEventListener('click', sendInfoFromSentToDeletedMails);
        liToBeRemoved.remove();
    }

    function sendInfoFromSentToDeletedMails(){
        const liElement = this.parentNode.parentNode;
        otherDOMSelectors.deletedMailsContainer.appendChild(liElement);
        const deleteBtn = Array.from(liElement.children)[2];
        deleteBtn.remove();
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
solve()
