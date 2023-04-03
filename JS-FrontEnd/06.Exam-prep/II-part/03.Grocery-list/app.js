function attachEvents(){
    const BASE_URL = 'http://localhost:3030/jsonstore/grocery/';
    let inputsObj = [];
    let productToBeUpdated = {};

    const inputDOMSelectors = {
        product: document.getElementById('product'),
        count: document.getElementById('count'),
        price: document.getElementById('price'),
    }

    const otherDOMSelectors = {
        addBtn: document.getElementById('add-product'),
        updateBtn: document.getElementById('update-product'),
        loadAllBtn: document.getElementById('load-product'),
        tableBody: document.getElementById('tbody'),
        form: document.querySelector('#addForm .list')
    }
    otherDOMSelectors.loadAllBtn.addEventListener('click', loadAllProducts);
    otherDOMSelectors.addBtn.addEventListener('click', addNewProduct);
    otherDOMSelectors.updateBtn.addEventListener('click', updateProduct);

    function addNewProduct(event){
        event.preventDefault();
        const newProduct = {
            product: inputDOMSelectors.product.value,
            count: inputDOMSelectors.count.value,
            price: inputDOMSelectors.price.value
        }
        httpHeaders = {
            method: 'POST',
            body: JSON.stringify(newProduct)
        }
        fetch(BASE_URL, httpHeaders)
        .then((data) =>{
            loadAllProducts();
            otherDOMSelectors.form.reset();
        })
        .catch((err) => {
            console.error(err);
        })
    }

    function loadAllProducts(event){
        if(event){
            event.preventDefault();
        }
       
        otherDOMSelectors.tableBody.innerHTML = '';
        fetch(BASE_URL)
        .then((res) => res.json())
        .then((data) =>{
            inputsObj = Object.values(data);
            for (const product of Object.values(data)) {
                const tableRow = createElement('tr', '', otherDOMSelectors.tableBody, '', '', {id: product._id});
                createElement('td', product.product, tableRow, '', ['name']);
                createElement('td', product.count, tableRow, '', ['count-product']);
                createElement('td', product.price, tableRow, '', ['product-price']);
                const buttons = createElement('td', '', tableRow, '', ['btn']);
                const updateBtn = createElement('button', 'Update', buttons, '', ['update']);
                updateBtn.addEventListener('click', loadDataOfTheProduct)
                const deleteBtn = createElement('button', 'Delete', buttons, '', ['delete']);
                deleteBtn.addEventListener('click', deleteProduct);
            }
        })
        .catch((err) => {
            console.error(err);
        })
    }

    function loadDataOfTheProduct(){
        const id = this.parentNode.parentNode.id;
        productToBeUpdated = inputsObj.find((inp) => inp._id === id);
        for (const key in inputDOMSelectors) {
            inputDOMSelectors[key].value = productToBeUpdated[key];
        }
        otherDOMSelectors.updateBtn.removeAttribute('disabled');
        otherDOMSelectors.addBtn.setAttribute('disabled', true);
    }

    function updateProduct(event){
        event.preventDefault();

        const payload = {
            product: inputDOMSelectors.product.value,
            count: inputDOMSelectors.count.value,
            price: inputDOMSelectors.price.value
        }

        httpHeaders ={
            method: 'PATCH',
            body: JSON.stringify(payload)
        }

        fetch(`${BASE_URL}${productToBeUpdated._id}`, httpHeaders)
        .then(() =>{
            loadAllProducts();
            otherDOMSelectors.updateBtn.setAttribute('disabled', true);
            otherDOMSelectors.addBtn.removeAttribute('disabled');
            otherDOMSelectors.form.reset();
        })
        .catch((err) =>{
            console.error(err);
        })
    }

    function deleteProduct(){
        const id = this.parentNode.parentNode.id;
        
        httpHeaders ={
            method: 'DELETE'
        }

        fetch(`${BASE_URL}${id}`, httpHeaders)
        .then(() =>{
            loadAllProducts();
        })
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

attachEvents();