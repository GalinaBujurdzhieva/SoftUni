function attachEvents() {
    const BASE_URL = 'http://localhost:3030/jsonstore/tasks/';

    const inputDOMSelectors = {
        title: document.getElementById('title'),
        description: document.getElementById('description')
    };

    const loadBoardBtn = document.getElementById('load-board-btn');
    const addTaskBtn = document.getElementById('create-task-btn');
    const form = document.querySelector('#form-section form');

    const otherDOMSelectors = {
        toDoSectionContainer: document.querySelector('#todo-section .task-list'),
        inProgressSectionContainer: document.querySelector('#in-progress-section .task-list'),
        codeReviewSectionContainer: document.querySelector('#code-review-section .task-list'),
        doneSectionContainer: document.querySelector('#done-section .task-list'),
    }

    loadBoardBtn.addEventListener('click', loadAllTasksHandler);
    addTaskBtn.addEventListener('click', addNewTaskHandler);

    function addNewTaskHandler(event){
        if(event){
            event.preventDefault();
        }
        if(!inputsValidator(inputDOMSelectors)){
            return;
        }
        const newTask = {
        title: inputDOMSelectors.title.value,
        description: inputDOMSelectors.description.value,
        status: 'ToDo'
        }

        const httpHeaders ={
            method: 'POST',
            body: JSON.stringify(newTask)
        }

        fetch(BASE_URL, httpHeaders)
        .then((res) =>res.json())
        .then((data) =>{
            loadAllTasksHandler();
        })
        .catch((err)=>{
            console.log(err);
        })
        inputDOMSelectors.title.value = '';
        inputDOMSelectors.description.value = '';
    }

    function loadAllTasksHandler(event){
        if(event){
            event.preventDefault();
        }
        for (const container in otherDOMSelectors) {
            otherDOMSelectors[container].innerHTML = '';
        };

        fetch(BASE_URL)
        .then((res) => res.json())
        .then((data) =>{
            for (const task of Object.values(data)) {
                taskStatus(task);
            }
        })
        .catch((err) =>{
            console.error(err);
        })

    }

    function taskStatus(task){
        switch (task.status) {
            case 'ToDo':
            const todoListElement = createElement('li', '', otherDOMSelectors.toDoSectionContainer, task._id, ['task']);
            createElement('h3', task.title, todoListElement);
            createElement('p', task.description, todoListElement);
            const moveInProgressContainerBtn = createElement('button', 'Move to In Progress', todoListElement);
            moveInProgressContainerBtn.addEventListener('click', moveInProgressContainerHandler);
            break;
            case 'In Progress':
                const inProgressListElement = createElement('li', '', otherDOMSelectors.inProgressSectionContainer, task._id, ['task']);
                createElement('h3', task.title, inProgressListElement);
                createElement('p', task.description, inProgressListElement);
                const moveToCodeReviewContainerBtn = createElement('button', 'Move to Code Review', inProgressListElement);
                moveToCodeReviewContainerBtn.addEventListener('click', moveToCodeReviewContainerHandler);
            break;
            case 'Code Review':
                const codeReviewListElement = createElement('li', '', otherDOMSelectors.codeReviewSectionContainer, task._id, ['task']);
                createElement('h3', task.title, codeReviewListElement);
                createElement('p', task.description, codeReviewListElement);
                const moveToDoneContainerBtn = createElement('button', 'Move to Done', codeReviewListElement);
                moveToDoneContainerBtn.addEventListener('click', moveToDoneContainerHandler);
            break;
            case 'Done':
                const doneListElement = createElement('li', '', otherDOMSelectors.doneSectionContainer, task._id, ['task']);
                createElement('h3', task.title, doneListElement);
                createElement('p', task.description, doneListElement);
                const closeBtn = createElement('button', 'Close', doneListElement);
                closeBtn.addEventListener('click', closeTaskHandler);
            break;
            default:
                break;
        }
    }

    function moveInProgressContainerHandler(){
        const taskId = this.parentNode.id;
        const parentElement = this.parentNode;
        const payload ={
            status: 'In Progress'
        }
        httpHeaders ={
            method: 'PATCH',
            body: JSON.stringify(payload)
        } 
        fetch(`${BASE_URL}${taskId}`, httpHeaders)
        .then((res) => res.json())
        .then(()=>{

            loadAllTasksHandler()
        })
        parentElement.remove();
    }
    function moveToCodeReviewContainerHandler(){
        const taskId = this.parentNode.id;
        const parentElement = this.parentNode;
        const payload ={
            status: 'Code Review'
        }
        httpHeaders ={
            method: 'PATCH',
            body: JSON.stringify(payload)
        } 
        fetch(`${BASE_URL}${taskId}`, httpHeaders)
        .then((res) => res.json())
        .then(()=>{
            loadAllTasksHandler()
        })
        parentElement.remove();
    }
    function moveToDoneContainerHandler(){
        const taskId = this.parentNode.id;
        const parentElement = this.parentNode;
        const payload ={
            status: 'Done'
        }
        httpHeaders ={
            method: 'PATCH',
            body: JSON.stringify(payload)
        } 
        fetch(`${BASE_URL}${taskId}`, httpHeaders)
        .then((res) => res.json())
        .then(()=>{
            loadAllTasksHandler()
        })
        parentElement.remove();
        
    }
    function closeTaskHandler(){
        const taskId = this.parentNode.id;
        const parentElement = this.parentNode;

        httpHeaders ={
            method: 'DELETE'
        }
        fetch(`${BASE_URL}${taskId}`, httpHeaders)
        .then((res) => res.json())
        .then(()=>{
            loadAllTasksHandler()
        })
        parentElement.remove();
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

attachEvents();