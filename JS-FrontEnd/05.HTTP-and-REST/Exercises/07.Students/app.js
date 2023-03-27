function attachEvents() {
  
  const BASE_URL = 'http://localhost:3030/jsonstore/collections/students';
  const tableBody = document.querySelector('#results > tbody');
  loadStudents();

  const [firstNameInput, lastNameInput, facultyNumberInput, gradeInput] = Array.from(document.querySelectorAll('.inputs > input'));

  const submitBtn = document.getElementById('submit');
  submitBtn.addEventListener('click', createStudent);

  function createStudent(){
    const newStudent = {
      firstName: firstNameInput.value,
      lastName: lastNameInput.value,
      facultyNumber: facultyNumberInput.value,
      grade: gradeInput.value,
    }
    if (studentValidation(newStudent) === false){
      return;
    }

    fetch(BASE_URL, {
      mode:'no-cors',
      method: 'post', 
      headers: {'contentType' : 'application/json'},
      body: JSON.stringify(newStudent)
    })
    .then(()=>{
      firstNameInput.value = '';
      lastNameInput.value = '';
      facultyNumberInput.value = '';
      gradeInput.value = '';
      tableBody.innerHTML = '';
      loadStudents();
    })
    .catch((err) =>{
      console.error(err);
    })
  }

  function studentValidation(student){
    for (const key in student) {
      let validKey = student[key].trim();
      if (validKey.length === 0){
        return false;
      }
    }
  }
  
  function loadStudents(){
    fetch(BASE_URL)
  .then((res) => res.json())
  .then((data) => {
    for (const studentId in data) {
      const tableRow = createElement('tr', '', tableBody);
      createElement('td', data[studentId].firstName, tableRow);
      createElement('td', data[studentId].lastName, tableRow);
      createElement('td', data[studentId].facultyNumber, tableRow);
      createElement('td', data[studentId].grade, tableRow);
    }
  })
  .catch((err) =>{
    console.error(err);
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