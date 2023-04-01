window.addEventListener('load', solve);

function solve() {
    const inputDOMSelectors ={
        genre: document.getElementById('genre'),
        name: document.getElementById('name'),
        author: document.getElementById('author'),
        date: document.getElementById('date'),
    }
    const otherDOMSelectors = {
        addBtn: document.getElementById('add-btn'),
        form: document.querySelector('form'),
        allSongsContainer: document.querySelector('#all-hits .all-hits-container'),
        savedSongsContainer: document.querySelector('#saved-hits .saved-container'),
        totalLikes: document.querySelector('#total-likes p'),
    }
    let totalLikes = 0;

    otherDOMSelectors.addBtn.addEventListener('click', addNewSongInCollection);

    function addNewSongInCollection(event){
        if (event) {
            event.preventDefault();
        }
        if(!inputsValidator()){
            return;
        }

        const {genre, name, author, date} = inputDOMSelectors;
        const divParent = createElement('div', '', otherDOMSelectors.allSongsContainer, '', 'hits-info');
        createElement('img', '', divParent, '', '', {src: './static/img/img.png'})
        createElement('h2', `Genre: ${genre.value}`, divParent);
        createElement('h2', `Name: ${name.value}`, divParent);
        createElement('h2', `Author: ${author.value}`, divParent);
        createElement('h3', `Date: ${date.value}`, divParent);
        const saveBtn = createElement('button', 'Save song', divParent, '', 'save-btn');
        saveBtn.addEventListener('click', saveSongHandler);
        const likeBtn = createElement('button', 'Like song', divParent, '', 'like-btn');
        likeBtn.addEventListener('click', likeSongHandler)
        const deleteBtn = createElement('button', 'Delete', divParent, '', 'delete-btn');
        deleteBtn.addEventListener('click', deleteSongHandler);

        for (const input of Object.values(inputDOMSelectors)) {
            input.value = '';
        }
    }

    function deleteSongHandler(){
        this.parentNode.remove();
    }

    function saveSongHandler(){
        const divParent = this.parentNode;
        otherDOMSelectors.savedSongsContainer.appendChild(divParent);
        const saveBtn = divParent.querySelector('.save-btn');
        const likeBtn = divParent.querySelector('.like-btn');
        saveBtn.remove();
        likeBtn.remove();
    }

    function likeSongHandler(){
        this.setAttribute('disabled', true);
        totalLikes++;
        otherDOMSelectors.totalLikes.textContent = `Total Likes: ${totalLikes}`;
        }

    function inputsValidator(){
        return Object.values(inputDOMSelectors).every((input) => input.value !== '');
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
          htmlElement.classList.add(classes)
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