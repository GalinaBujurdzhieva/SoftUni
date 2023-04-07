function solve() {
    inputDOMSelectors = {
        task: document.getElementById('task'),
        description: document.getElementById('description'),
        dueDate: document.getElementById('date')
    }

    otherDOMSelectors ={
        form: document.querySelector('form'),
        addBtn: document.getElementById('add'),
        openTasksDiv: document.querySelector('main section:nth-child(2) div:nth-child(2)'),
        inProgressTaskDiv: document.getElementById('in-progress'),
        completedTasksDiv: document.querySelector('main section:nth-child(4) div:nth-child(2)'),
    }
    otherDOMSelectors.addBtn.addEventListener('click', addTaskInOpenTasksSection);

    function addTaskInOpenTasksSection(event){
        if (event) {
            event.preventDefault();
        }
        if (!inputsValidator(inputDOMSelectors)) {
            return;
        }
        const {task, description, dueDate} = inputDOMSelectors;
        const article = createElement('article', '', otherDOMSelectors.openTasksDiv);
        createElement('h3', task.value, article);
        createElement('p', `Description: ${description.value}`, article);
        createElement('p', `Due Date: ${dueDate.value}`, article);
        const buttonsDiv = createElement('div', '', article, '', ['flex']);
        const startBtn = createElement('button', 'Start', buttonsDiv, '', ['green']);
        startBtn.addEventListener('click', moveTaskInProgressTasksSection);
        const deleteBtn = createElement('button', 'Delete', buttonsDiv, '', ['red']);
        deleteBtn.addEventListener('click', deleteTask);
        otherDOMSelectors.form.reset();
    }

    function moveTaskInProgressTasksSection(){
        const article = this.parentNode.parentNode;
        otherDOMSelectors.inProgressTaskDiv.appendChild(article);
        const buttonsDiv = article.querySelector('.flex');
        buttonsDiv.innerHTML = '';
        const deleteBtn = createElement('button', 'Delete', buttonsDiv, '', ['red']);
        deleteBtn.addEventListener('click', deleteTask);
        const finishBtn = createElement('button', 'Finish', buttonsDiv, '', ['orange']);
        finishBtn.addEventListener('click', moveTaskInCompletedTasksSection);
    }

    function moveTaskInCompletedTasksSection(){
        const article = this.parentNode.parentNode;
        otherDOMSelectors.completedTasksDiv.appendChild(article);
        const buttonsDiv = article.querySelector('.flex');
        buttonsDiv.remove();
    }

    function deleteTask(){
        const article = this.parentNode.parentNode;
        article.remove();
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
