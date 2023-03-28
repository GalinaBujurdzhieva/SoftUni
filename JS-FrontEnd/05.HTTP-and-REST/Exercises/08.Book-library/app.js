function attachEvents() {
  const BASE_URL = 'http://localhost:3030/jsonstore/collections/books/';
  const tableBody = document.querySelector('table > tbody');

  const loadBookBtn = document.getElementById('loadBooks');
  loadBookBtn.addEventListener('click', loadBooks);

  const formTitle = document.querySelector('#form > h3');
  const [titleInput, authorInput] = Array.from(document.querySelectorAll('#form > input'));
  const submitOrSaveBtn = document.querySelector('#form > button');
  submitOrSaveBtn.addEventListener('click', createOrEditBook);

  let booksData = {};
  let bookToBeUpdatedId = null;

  function createOrEditBook(){
    const newBook = {
      title: titleInput.value,
      author: authorInput.value
    };

    if (bookValidation(newBook) === false){
      return;
    }

    let url = BASE_URL;
    const httpHeaders ={
      method: 'post',
      body: JSON.stringify(newBook)
    }

    if (formTitle === 'Edit FORM') {
      httpHeaders.method = 'put';
      url += bookToBeUpdatedId;
    }

    fetch(url, httpHeaders)
    .then(() =>{
      if (formTitle === 'Edit FORM') {
        formTitle.textContent = 'FORM';
        submitOrSaveBtn.textContent = 'Submit';
      }
      loadBooks();
      titleInput.value = '';
      authorInput.value = '';
    })
    .catch((err) =>{
      console.error(err);
    })
  }

  // Validate New Book
  function bookValidation(book){
    for (const key in book) {
      let validKey = book[key].trim();
      if (validKey.length === 0){
        return false;
      }
    }
  }

  // Load All Books
  function loadBooks(){
    tableBody.innerHTML = '';

    fetch(BASE_URL)
    .then((res) => res.json())
    .then((data) =>{
      for (const bookId in data) {
        booksData = data;
        const tableRow = createElement('tr', '', tableBody);
        createElement('td', data[bookId].title, tableRow);
        createElement('td', data[bookId].author, tableRow);
        const thirdColumn = createElement('td', '', tableRow);
        const editBtn = createElement('button', 'Edit', thirdColumn);
        editBtn.id = bookId;
        editBtn.addEventListener('click', loadEditBookDetails)
        const deleteBtn = createElement('button', 'Delete', thirdColumn);
        deleteBtn.id = bookId;
        deleteBtn.addEventListener('click', deleteBook)
      }
    })
    .catch((err) =>{
      console.error(err);
    })
  }

  // Edit Book
  function loadEditBookDetails(){
    bookToBeUpdatedId = this.id;
    formTitle.textContent = 'Edit FORM';
    submitOrSaveBtn.textContent = 'Save';
    const bookToBeUpdated = booksData[this.id];
    titleInput.value = bookToBeUpdated.title;
    authorInput.value = bookToBeUpdated.author;
  }

  // Delete Book
  function deleteBook(){
    const bookToBeDeletedId = this.id;
    fetch(`${BASE_URL}/${bookToBeDeletedId}`,{
      method: 'delete'
    })
    .then(loadBooks)
    .catch((err) => {
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