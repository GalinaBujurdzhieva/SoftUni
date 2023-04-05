window.addEventListener("load", solve);

function solve() {
 inputDOMSelectors = {
  title: document.getElementById('post-title'),
  category: document.getElementById('post-category'),
  content: document.getElementById('post-content')
 }

 otherDOMSelectors = {
  form: document.querySelector('#newPost .newPostContent'),
  publishBtn: document.getElementById('publish-btn'),
  reviewListPostsContainer: document.getElementById('review-list'),
  publishedListPostsContainer: document.getElementById('published-list'),
  clearBtn: document.getElementById('clear-btn')
 }

 let inputsObj = {};

 otherDOMSelectors.publishBtn.addEventListener('click', loadPostForReview);
 otherDOMSelectors.clearBtn.addEventListener('click', deletePublishedPost);

 function deletePublishedPost(){
  const liElement = Array.from(otherDOMSelectors.publishedListPostsContainer.children)[0];
  liElement.remove();
 }

 function loadPostForReview(event){
  if (event) {
    event.preventDefault();
  }
  if (!inputsValidator(inputDOMSelectors)) {
    return;
  }
  for (const key in inputDOMSelectors) {
    inputsObj[key] = inputDOMSelectors[key].value;
  }
  const{title, category, content} = inputDOMSelectors;
  const liElement = createElement('li', '', otherDOMSelectors.reviewListPostsContainer, '', ['rpost']);
  const article = createElement('article', '', liElement);
  createElement('h4', `${title.value}`, article);
  createElement('p', `Category: ${category.value}`, article);
  createElement('p', `Content: ${content.value}`, article);
  const editBtn = createElement('button', 'Edit', liElement, '', ['action-btn', 'edit']);
  editBtn.addEventListener('click', loadPostForEdit);
  const approveBtn = createElement('button', 'Approve', liElement, '', ['action-btn', 'approve']);
  approveBtn.addEventListener('click', approvePost);
  otherDOMSelectors.form.reset();
 }

 function approvePost(){
  const liElement = this.parentNode;
  otherDOMSelectors.publishedListPostsContainer.appendChild(liElement);
  const children = Array.from(liElement.children)
  const editBtn = Array.from(liElement.children)[1];
  const approveBtn = Array.from(liElement.children)[2];
  editBtn.remove();
  approveBtn.remove();
 }

 function loadPostForEdit(){
  for (const key in inputsObj) {
    inputDOMSelectors[key].value = inputsObj[key];
  }
  const liElement = this.parentNode;
  liElement.remove();
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
