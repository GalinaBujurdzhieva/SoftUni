function getInfo() {
    const stopID = document.getElementById('stopId').value;
    const stopNameContainer = document.getElementById('stopName');
    const unorderedList = document.getElementById('buses');
    const BASE_URL = 'http://localhost:3030/jsonstore/bus/businfo/';
    const url = BASE_URL + stopID;

    fetch(url)
    .then((res) => res.json())
    .then((data) =>{
        unorderedList.innerHTML ='';
        stopNameContainer.textContent = data.name;
        console.log(data.buses);

        for (const busId in data.buses) {
            const listElementTextContent = `Bus ${busId} arrives in ${data.buses[busId]} minutes`;
            createElement('li', listElementTextContent, unorderedList);
        }
    })
    .catch((err) =>{
        unorderedList.innerHTML ='';
        stopNameContainer.textContent = 'Error';
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