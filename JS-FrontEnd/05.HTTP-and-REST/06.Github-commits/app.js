function loadCommits() {
    const username = document.getElementById('username').value;
    const repo = document.getElementById('repo').value;
    const url = 'https://api.github.com/repos/' + username + '/' + repo + '/commits';
    const unorderedList = document.getElementById('commits');
    
    fetch(url)
    .then((res) => res.json())
    .then((data) => {
        unorderedList.innerHTML ='';
        data.forEach(element => {
            const liTextContent = `${element.commit.author.name}: ${element.commit.message}`;
            createElement('li', liTextContent, unorderedList);
        });
    })
    .catch((err) =>{
        unorderedList.innerHTML ='';
        const errorText = `Error: ${err.message} (Not Found)"`;
        createElement('li', errorText, unorderedList);
    })

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
