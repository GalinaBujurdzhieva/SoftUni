function loadRepos() {
	const BASE_URL = 'https://api.github.com/users/'
	const userId = document.getElementById('username').value;
	const userReposUrl = BASE_URL + userId + '/repos';
	const unorderedList = document.getElementById('repos');

	fetch(userReposUrl)
	.then((res) => res.json())
	.then((data) => {
		unorderedList.innerHTML = '';
		data.forEach(element => {
			const listItem = createElement('li', '', unorderedList);
			const anchor = createElement('a', element.full_name, listItem, '', '', { href: element.html_url});
		});
	})
	.catch((err) =>{
		unorderedList.innerHTML = '';
		const listItem = createElement('li', err.message, unorderedList);
	});

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
