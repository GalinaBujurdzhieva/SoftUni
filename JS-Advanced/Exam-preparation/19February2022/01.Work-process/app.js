function solve() {
    inputDOMSelectors = {
        firstName: document.getElementById('fname'),
        lastName: document.getElementById('lname'),
        email: document.getElementById('email'),
        dateOfBirth: document.getElementById('birth'),
        position: document.getElementById('position'),
        salary: document.getElementById('salary')
    }
    otherDOMSelectors = {
        form: document.querySelector('#signup form'),
        hireWorkerBtn: document.getElementById('add-worker'),
        tbody: document.getElementById('tbody'),
        budgetSum: document.getElementById('sum')
    }
    let workersObj = [];
    let budget = 0;

    otherDOMSelectors.hireWorkerBtn.addEventListener('click', hireWorkerHandler);

    function hireWorkerHandler(event){
        if (event) {
            event.preventDefault();
        }
        if (!inputsValidator(inputDOMSelectors)) {
            return;
        }
        let inputsObj = {};
        for (const key in inputDOMSelectors) {
            inputsObj[key] = inputDOMSelectors[key].value;
        }
        let currentWorker = workersObj.find((worker) => worker.email === inputsObj.email);
        if (workersObj.length === 0 || currentWorker === undefined) {
            workersObj.push(inputsObj);
        }
        else{
            let index = workersObj.indexOf(currentWorker);
            workersObj.splice(index,1,inputsObj);
        }
        const {firstName, lastName, email, dateOfBirth, position, salary} = inputDOMSelectors;
        const tableRow = createElement('tr', '', otherDOMSelectors.tbody);
        createElement('td', firstName.value, tableRow);
        createElement('td', lastName.value, tableRow);
        createElement('td', email.value, tableRow);
        createElement('td', dateOfBirth.value, tableRow);
        createElement('td', position.value, tableRow);
        createElement('td', salary.value, tableRow);
        const buttonsTableCell = createElement('td', '', tableRow);
        const fireBtn = createElement('button', 'Fired', buttonsTableCell, '', ['fired']);
        fireBtn.addEventListener('click', fireWorkerHandler);
        const editBtn = createElement('button', 'Edit', buttonsTableCell, '', ['edit']);
        editBtn.addEventListener('click', editWorkerInfoHandler);
        budget += Number(salary.value);
        otherDOMSelectors.budgetSum.textContent = budget.toFixed(2);
        otherDOMSelectors.form.reset();
    }

    function fireWorkerHandler(){
        const tableRowElement = this.parentNode.parentNode; 
        const email = Array.from(tableRowElement.children)[2];
        let currentWorker = workersObj.find((worker) => worker.email === email.textContent);
        budget -= Number(currentWorker.salary);    
        otherDOMSelectors.budgetSum.textContent = budget.toFixed(2);
        tableRowElement.remove();
    }

    function editWorkerInfoHandler(){
        const tableRowElement = this.parentNode.parentNode; 
        const email = Array.from(tableRowElement.children)[2];
        let currentWorker = workersObj.find((worker) => worker.email === email.textContent);
        for (const key in currentWorker) {
            inputDOMSelectors[key].value = currentWorker[key];
        }
        budget -= Number(currentWorker.salary);    
        otherDOMSelectors.budgetSum.textContent = budget.toFixed(2);
        tableRowElement.remove();
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