function attachEvents() {
const BASE_URL = 'http://localhost:3030/jsonstore/tasks/';
const tasksContainer = document.getElementById('todo-list');
const input = document.getElementById('title');

const loadBtn = document.getElementById('load-button');
loadBtn.addEventListener('click', loadAllTasksHandler);

const addBtn = document.getElementById('add-button');
addBtn.addEventListener('click', addNewTaskHandler);

function loadAllTasksHandler(event){
   if(event){
    event.preventDefault();
  }

  tasksContainer.innerHTML = '';

  fetch(BASE_URL)
  .then((res) => res.json())
  .then((data) => {
    for (const task of Object.values(data)) {
      const liElement = createElement('li', '', tasksContainer, task._id);
      createElement('span', task.name, liElement);
      const removeBtn = createElement('button', 'Remove', liElement);
      removeBtn.addEventListener('click', removeTaskHandler);
      const editBtn = createElement('button', 'Edit', liElement);
      editBtn.addEventListener('click', editTaskHandler);
    }
  })
  .catch((err) =>{
    console.error(err);
  });
}

function addNewTaskHandler(event){
  if(event){
    event.preventDefault();
  }

  httpHeaders = {
    method: 'POST',
    body: JSON.stringify({name: input.value})
  }

  fetch(BASE_URL, httpHeaders)
  .then((res) => res.json())
  .then((data) => {
    loadAllTasksHandler();
    input.value = ''
  })
  .catch((err) =>{
    console.error(err);
  });
}

function removeTaskHandler(){
  const id = this.parentNode.id;

  httpHeaders = {
    method: 'DELETE',
  }

  fetch(`${BASE_URL}${id}`, httpHeaders)
  .then((data) =>{
    loadAllTasksHandler();
  })
  .catch((err) => {
    console.log(err);
  })
}

function editTaskHandler(){
  const liElement = this.parentNode;
  const liChildren = Array.from(liElement.children)
  const span = liChildren[0];
  const editBtn = liChildren[2];
  const inputForTheTask = createElement('input', '', liElement);
  inputForTheTask.value = span.textContent;
  liElement.prepend(inputForTheTask);
  const submitBtn = createElement('button', 'Submit', liElement);
  submitBtn.addEventListener('click', sendEditedTaskToDBHandler);
  liElement.appendChild(submitBtn);
  span.remove();
  editBtn.remove();
}

function sendEditedTaskToDBHandler(event){
  const liElement = this.parentNode;
  const input = Array.from(liElement.children)[0];
  const id = this.parentNode.id;

  if(event){
    event.preventDefault();
  }
  
  httpHeaders =  {
    method: 'PATCH', 
    body: JSON.stringify({name: input.value})
  }
  fetch(`${BASE_URL}${id}`, httpHeaders)
  .then((data) =>{
    loadAllTasksHandler();
  })
  .catch((err) => {
    console.log(err);
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
attachEvents();