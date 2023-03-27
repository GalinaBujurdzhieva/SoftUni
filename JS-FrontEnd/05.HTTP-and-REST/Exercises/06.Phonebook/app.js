function attachEvents() {
    const BASE_URL = 'http://localhost:3030/jsonstore/phonebook';
    const phoneBookContainer = document.getElementById('phonebook');
    const personInput = document.getElementById('person');
    const phoneInput = document.getElementById('phone');

    const loadBtn = document.getElementById('btnLoad');
    loadBtn.addEventListener('click', loadPhones);

    const createBtn = document.getElementById('btnCreate');
    createBtn.addEventListener('click', createNewPhone);

    function loadPhones(){
        fetch(BASE_URL)
        .then((res) => res.json())
        .then((data) => {
            for (const key in data) {
                const listItem = createElement('li', `${data[key].person}: ${data[key].phone}`, phoneBookContainer);
                const deleteBtn = createElement('button', 'Delete', listItem);
                deleteBtn.id = data[key]._id;
                deleteBtn.addEventListener('click', deletePhone);
            }
        })
        .catch((err) =>{
            console.error(err);
        })
        phoneBookContainer.innerHTML = '';
    }

    async function deletePhone(){
        const id = this.id;
        fetch(`${BASE_URL}/${id}`, {
            method: 'DELETE'
        })
        .then(loadPhones)
        .catch((err) => {
            console.error(err)
        })
    }

    function createNewPhone(){
        const newPhoneNumber = {
            person: personInput.value,
            phone: phoneInput.value
        }
        fetch(BASE_URL, {
            mode: 'no-cors',
            method: 'POST',
            headers: {'contentType': 'application/json'},
            body: JSON.stringify(newPhoneNumber)
        })
        .then(loadPhones)
        .catch((err) =>{
            console.error(err)
        });
        personInput.value ='';
        phoneInput.value = '';
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
attachEvents();

