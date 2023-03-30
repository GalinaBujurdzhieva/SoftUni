window.addEventListener("load", solve);

function solve() {
  let inputObj = {
    firstName: null,
    lastName: null,
    age: null,
    storyTitle: null,
    genre: null,
    storyContent: null
  }

  const inputSelectors = {
     firstName: document.getElementById('first-name'),
     lastName: document.getElementById('last-name'),
     age: document.getElementById('age'),
     storyTitle: document.getElementById('story-title'),
     genre: document.getElementById('genre'),
     storyContent: document.getElementById('story')
  }

  const otherSelectors = {
    publishBtn: document.getElementById('form-btn'),
    preview: document.getElementById('preview-list'),
    main: document.getElementById('main')
  }

  otherSelectors.publishBtn.addEventListener('click', publishStory)

  function publishStory(){
    if(!(validator(inputSelectors))){
      return;
    }

    const {firstName, lastName, age, storyTitle, genre, storyContent} = inputSelectors;
    const liElement = createElement('li', '', otherSelectors.preview, '', 'story-info');
    const article = createElement('article', '', liElement);
    createElement('h4', `Name: ${firstName.value} ${lastName.value}`, article);
    createElement('p', `Age: ${age.value}`, article);
    createElement('p', `Title: ${storyTitle.value}`, article);
    createElement('p', `Genre: ${genre.value}`, article);
    createElement('p', storyContent.value, article);
    const saveBtn = createElement('button', 'Save Story', liElement, '', 'save-btn');
    const editBtn = createElement('button', 'Edit Story', liElement, '', 'edit-btn');
    const deleteBtn = createElement('button', 'Delete Story', liElement, '', 'delete-btn');
    otherSelectors.publishBtn.disabled = true;

    for (const inputField in inputSelectors) {
      inputObj[inputField] =  inputSelectors[inputField].value;
      inputSelectors[inputField].value = '';
    }
    saveBtn.addEventListener('click', saveStory);
    editBtn.addEventListener('click', editStory);
    deleteBtn.addEventListener('click', deleteStory);
  }

  function saveStory(){
    otherSelectors.main.innerHTML ='';
    createElement('h1', 'Your scary story is saved!', otherSelectors.main);
  }

  function editStory(e){
    for (const inputField in inputObj) {
      inputSelectors[inputField].value = inputObj[inputField];
    }
    otherSelectors.publishBtn.disabled = false;
    e.currentTarget.disabled = true;
    const [saveBtn, _editBtn, deleteBtn] = Array.from(e.currentTarget.parentNode.children).splice(1, 3);
    saveBtn.setAttribute('disabled', true);
    deleteBtn.setAttribute('disabled', true);
    e.currentTarget.parentNode.remove();
  }

  function deleteStory(e){
    e.currentTarget.parentNode.remove();
    otherSelectors.publishBtn.disabled = false;
  }

  function validator (inputObj){
    return Object.values(inputObj).every((input) => input.value !== '');
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
