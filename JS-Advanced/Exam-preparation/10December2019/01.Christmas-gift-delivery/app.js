function solution() {
    addGiftInput = document.querySelector('.card input');

    DOMSelectors = {
        addGiftBtn: document.querySelector('.card button'),
        listOfGiftsContainer: Array.from(document.querySelectorAll('.card ul'))[0],
        sentGiftsContainer: Array.from(document.querySelectorAll('.card ul'))[1],
        discardedGiftsContainer: Array.from(document.querySelectorAll('.card ul'))[2]
    }
    let giftsArray = [];
    DOMSelectors.addGiftBtn.addEventListener('click', loadListOfGiftsContainer);

    function loadListOfGiftsContainer(){
        if (addGiftInput.value.trim() === '') {
            return;
        }
        giftsArray.push(addGiftInput.value);
        giftsArray.sort((aName, bName) => aName.localeCompare(bName));
        DOMSelectors.listOfGiftsContainer.innerHTML = '';
        for (const gift of giftsArray) {
            const liElement = createElement('li', gift, DOMSelectors.listOfGiftsContainer, '', ['gift']);
            const sendBtn = createElement('button', 'Send', liElement, '', ['sendButton']);
            sendBtn.addEventListener('click', sendGiftToSentGiftsContainer);
            const discardBtn = createElement('button', 'Discard', liElement, '', ['discardButton']);
            discardBtn.addEventListener('click', sendGiftToDiscardedGiftsContainer);
            }
        addGiftInput.value = '';
    }
    
    function sendGiftToSentGiftsContainer(){
        const liElement = this.parentNode;
        const liElementChildren = Array.from(liElement.children);
        liElementChildren.forEach((ch) => ch.remove());
        const gift = liElement.textContent;
        const index = giftsArray.indexOf(liElement.textContent);
        giftsArray.splice(index, 1);
        liElement.remove();
        createElement('li', gift, DOMSelectors.sentGiftsContainer, '', ['gift']);
        
    }

    function sendGiftToDiscardedGiftsContainer(){
       const liElement = this.parentNode;
        const liElementChildren = Array.from(liElement.children);
        liElementChildren.forEach((ch) => ch.remove());
        const gift = liElement.textContent;
        const index = giftsArray.indexOf(liElement.textContent);
        giftsArray.splice(index, 1);
        liElement.remove();
        createElement('li', gift, DOMSelectors.discardedGiftsContainer, '', ['gift']);
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
