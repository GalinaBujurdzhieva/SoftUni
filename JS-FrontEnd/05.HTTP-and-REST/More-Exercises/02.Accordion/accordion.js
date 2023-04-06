function solution() {
    const BASE_URL = 'http://localhost:3030/jsonstore/advanced/articles/list';
    const DETAILS_URL = 'http://localhost:3030/jsonstore/advanced/articles/details/';
    const main = document.getElementById('main');

    fetch(BASE_URL)
    .then((res) => res.json())
    .then((data) =>{
        for (const {_id, title} of Object.values(data)) {
            const divAccordion = createElement('div', '', main, '', ['accordion']);
            const divHead = createElement('div', '', divAccordion, '', ['head']);
            createElement('span', title, divHead);
            const moreBtn = createElement('button', 'MORE', divHead, '', ['button'], {id: _id});
            createElement('div', '', divAccordion, '', ['extra'], {style: 'display:none'});
            moreBtn.addEventListener('click', loadHiddenContent);
        }
    })
    .catch((err) =>{
        console.error(err);
    })

    function loadHiddenContent(){
      const id = this.id;
      const divAccordion = this.parentNode.parentNode;
      const divExtra = Array.from(divAccordion.children)[1];

      fetch(`${DETAILS_URL}${id}`)
      .then((res) => res.json())
      .then((data) =>{
        if (this.textContent === 'MORE') {
          divExtra.style.display = 'block';
          this.textContent = 'LESS';
          const paragraph = createElement('p', data.content, divExtra);
        }
        else if (this.textContent === 'LESS') {
          divExtra.style.display = 'none';
          divExtra.innerHTML = '';
          this.textContent = 'MORE';
        }
      })
      .catch((err) =>{
        console.error(err);
    })
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
solution();