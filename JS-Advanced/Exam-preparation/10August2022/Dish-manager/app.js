window.addEventListener("load", solve);

function solve() {
  const inputDOMSelectors = {
    firstName: document.getElementById('first-name'),
    lastName: document.getElementById('last-name'),
    age: document.getElementById('age'),
    gender: document.getElementById('genderSelect'),
    information: document.getElementById('task')
  }
  const otherDOMSelectors = {
    submitBtn: document.getElementById('form-btn'),
    form: document.querySelector('#main form'),
    inProgressContainer: document.getElementById('in-progress'),
    finishedDishesContainer: document.getElementById('finished'),
    clearBtn: document.getElementById('clear-btn'),
    progressCounter: document.getElementById('progress-count')
  }
  let inputObj = {};
  let counter = 0;

  otherDOMSelectors.submitBtn.addEventListener('click', loadDishInProgressSection);

  function loadDishInProgressSection(event){
    if(event){
      event.preventDefault();
    }
    if(!inputValidator(inputDOMSelectors)){
      return;
    }
    for (const key in inputDOMSelectors) {
      inputObj[key] = inputDOMSelectors[key].value;
    }
    const {firstName, lastName, gender, age, information} = inputDOMSelectors;
    const liElement = createElement('li', '', otherDOMSelectors.inProgressContainer, '', ['each-line']);
    const article = createElement('article', '', liElement);
    createElement('h4', `${firstName.value} ${lastName.value}`, article);
    createElement('p', `${gender.value}, ${age.value}`, article);
    createElement('p', `Dish description: ${information.value}`, article);
    const editBtn = createElement('button', 'Edit', liElement, '', ['edit-btn']);
    editBtn.addEventListener('click', sendInfoToInputFields)
    const completeBtn = createElement('button', 'Mark as complete', liElement, '', ['complete-btn']);
    completeBtn.addEventListener('click', completeDish)
    counter++;
    otherDOMSelectors.progressCounter.textContent = `${counter}`;
    otherDOMSelectors.form.reset();
  }

  function completeDish(){
    const liElement = this.parentNode;
    otherDOMSelectors.finishedDishesContainer.appendChild(liElement);
    counter--;
    otherDOMSelectors.progressCounter.textContent = `${counter}`;
    const editBtn = Array.from(liElement.children)[1];
    const completeBtn = Array.from(liElement.children)[2];
    otherDOMSelectors.clearBtn.addEventListener('click', clearDish)
    editBtn.remove();
    completeBtn.remove();
  }

  function clearDish(){
    const liElement = Array.from(otherDOMSelectors.finishedDishesContainer.children)[0];
    liElement.remove();
  }

  function sendInfoToInputFields(){
    for (const key in inputObj) {
     inputDOMSelectors[key].value = inputObj[key];
    }
    const liElement = this.parentNode;
    liElement.remove();
    counter--;
    otherDOMSelectors.progressCounter.textContent = `${counter}`;
  }

  function inputValidator(obj){
    return Object.values(obj).every ((inp) => inp.value !== '');
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
