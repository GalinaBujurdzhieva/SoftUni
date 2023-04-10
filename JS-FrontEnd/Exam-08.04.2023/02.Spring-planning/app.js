window.addEventListener('load', solve);

function solve() {
    const inputDOMSelectors = {
        title: document.getElementById('title'),
        description: document.getElementById('description'),
        label: document.getElementById('label'),
        points: document.getElementById('points'),
        assignee: document.getElementById('assignee')
    }
    const taskId =  document.getElementById('task-id');

    const otherDOMSelectors = {
        form: document.getElementById('create-task-form'),
        createTaskBtn: document.getElementById('create-task-btn'),
        deleteTaskBtn: document.getElementById('delete-task-btn'),
        tasksContainer: document.getElementById('tasks-section'),
        totalPointsContainer: document.getElementById('total-sprint-points')
    }
    otherDOMSelectors.createTaskBtn.addEventListener('click', createNewSprintHandler);
    otherDOMSelectors.deleteTaskBtn.addEventListener('click', deleteTaskHandler)

    let totalPoints = 0;
    let allTasksObj = {};
    let id = 0;
    let idOfTheTaskToBeDeleted = 0;

    function additionalClass(obj){
        switch (obj) {
            case 'Feature':
                return 'feature';
                break;
            case 'Low Priority Bug':
                return 'low-priority';
                break;   
            default:
                return 'high-priority'
                break;
        }
    }

    function additionalCodeIcon(obj){
        switch (obj) {
            case 'Feature':
                return 'Feature &#8865';
                break;
            case 'Low Priority Bug':
                return 'Low Priority Bug &#9737';
                break;   
            default:
                return 'High Priority Bug &#9888'
                break;
        }
    }

    function deleteTaskHandler(event){
        if (event) {
            event.preventDefault();
        }
        const articleToBeDeleted = document.getElementById(`task-${idOfTheTaskToBeDeleted}`);
        articleToBeDeleted.remove();
        const taskToBeEdited = allTasksObj[idOfTheTaskToBeDeleted];
        taskId.value = '';
        totalPoints -= Number(taskToBeEdited.points);
        otherDOMSelectors.totalPointsContainer.textContent = `Total Points ${totalPoints}pts`;
        for (const key in inputDOMSelectors) {
            inputDOMSelectors[key].removeAttribute('disabled');
          }
        otherDOMSelectors.form.reset(); 
        inputDOMSelectors.label.value = '';
        otherDOMSelectors.deleteTaskBtn.setAttribute('disabled', true);
        otherDOMSelectors.createTaskBtn.removeAttribute('disabled');
    }

    function createNewSprintHandler(event){
        if (event){
            event.preventDefault();
        };

        if (!inputsValidator(inputDOMSelectors)) {
            return;
        }
        const {title, description, label, points, assignee} = inputDOMSelectors;
        id++;
        allTasksObj[id] = {title: title.value, description: description.value, label: label.value, points: points.value, assignee: assignee.value};
        const article = createElement('article', '', otherDOMSelectors.tasksContainer, `task-${id}`, ['task-card']);
        const divWithIcon = createElement('div', '', article, '', ['task-card-label', additionalClass(inputDOMSelectors.label.value)]);
        divWithIcon.innerHTML = additionalCodeIcon(inputDOMSelectors.label.value)
        createElement('h3', title.value, article, '', ['task-card-title']);
        createElement('p', description.value, article, '', ['task-card-description']);
        createElement('div', `Estimated at ${points.value} pts`, article, '', ['task-card-points']);
        createElement('div', `Assigned to: ${assignee.value}`, article, '', ['task-card-assignee']);
        const buttonDiv = createElement('div', '', article, '', ['task-card-actions']);
        const deleteBtn = createElement('button', 'Delete', buttonDiv);
        deleteBtn.addEventListener('click', loadConfirmDeleteHandler);
        totalPoints += Number(points.value);
        otherDOMSelectors.totalPointsContainer.textContent = `Total Points ${totalPoints}pts`;
        otherDOMSelectors.form.reset(); 
        inputDOMSelectors.label.value = '';
    }

    function loadConfirmDeleteHandler(){
      const neededTaskId = this.parentNode.parentNode.id;
      idOfTheTaskToBeDeleted = Number(neededTaskId.substring(5, neededTaskId.length));
      const taskToBeEdited = allTasksObj[idOfTheTaskToBeDeleted];
      for (const key in inputDOMSelectors) {
       inputDOMSelectors[key].value = taskToBeEdited[key];
       inputDOMSelectors[key].setAttribute('disabled', true);
      }
      otherDOMSelectors.createTaskBtn.setAttribute('disabled', true);
      otherDOMSelectors.deleteTaskBtn.removeAttribute('disabled');
      taskId.value = idOfTheTaskToBeDeleted;
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