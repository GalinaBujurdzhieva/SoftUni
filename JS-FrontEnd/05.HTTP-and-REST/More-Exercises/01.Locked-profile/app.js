
function lockedProfile() {
    const BASE_URL = 'http://localhost:3030/jsonstore/advanced/profiles';
    const main = document.getElementById('main');
    const divProfile = document.querySelector('#main .profile')

    fetch(BASE_URL)
    .then((res) => res.json())
    .then((data) =>{
     for (const key in data) {
            const newProfile = divProfile.cloneNode(true);
            main.appendChild(newProfile);
			const children = Array.from(newProfile.children);
			const userNameInput = children[8];
			userNameInput.value = data[key].username;
			const showMoreBtn = children[10];
			showMoreBtn.id = data[key]._id;			
			showMoreBtn.addEventListener('click', showInfoHandler);
			const divWithHiddenInfo = children[9];
			divWithHiddenInfo.style.display = 'none';
			const divChildren = Array.from(divWithHiddenInfo.children);
			const emailInput = divChildren[2];
			emailInput.value = data[key].email;
			const ageInput = divChildren[4];
			ageInput.value = data[key].age;
			ageInput.setAttribute('type', 'email');
     }
	 divProfile.remove();
    })

	function showInfoHandler(){
		const divParent = this.parentNode;
		const children = Array.from(divParent.children);
		const lockedProfileInput = children[2];
		const divWithHiddenInfo = children[9];
		if (!lockedProfileInput.checked & this.textContent === 'Show more') {
			divWithHiddenInfo.style.display = 'block';
			this.textContent = 'Hide it';
		}
		else if (!lockedProfileInput.checked & this.textContent === 'Hide it') {
			divWithHiddenInfo.style.display = 'none';
			this.textContent = 'Show more';
		}
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