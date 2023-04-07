function solve(){
   inputDOMSelectors = {
      author: document.getElementById('creator'), 
      title: document.getElementById('title'),
      category: document.getElementById('category'),
      content: document.getElementById('content')
   }
   otherDOMSelectors = {
      form: document.querySelector('.site-content aside form'),
      createBlogBtn: document.querySelector('.site-content aside form button'),
      postsListContainer: document.querySelector('.site-content section'),
      archiveList: document.querySelector('.archive-section ol'),
   }
   
   let archivedBlogs = [];
   otherDOMSelectors.createBlogBtn.addEventListener('click', loadPostInPostsList);

   function loadPostInPostsList(event){
      if (event) {
         event.preventDefault();
      }
      if (!inputsValidator(inputDOMSelectors)) {
         return;
      }
      const {author, title, category, content} = inputDOMSelectors;

      const article = createElement('article', '', otherDOMSelectors.postsListContainer);
      createElement('h1', title.value, article);
      const blogCategory = createElement('p', 'Category: ', article);
      createElement('strong', category.value, blogCategory);
      const blogAuthor = createElement('p', 'Creator: ', article);
      createElement('strong', author.value, blogAuthor);
      createElement('p', content.value, article);
      const buttonsDiv = createElement('div', '', article, '', ['buttons']);
      const deleteBtn = createElement('button', 'Delete', buttonsDiv, '', ['btn', 'delete']);
      deleteBtn.addEventListener('click', deletePost);
      const archiveBtn = createElement('button', 'Archive', buttonsDiv, '', ['btn', 'archive']);
      archiveBtn.addEventListener('click', moveBlogToArchive);
      otherDOMSelectors.form.reset();
   }

   function moveBlogToArchive(){
      const article = this.parentNode.parentNode;
      const blogTitle = article.querySelector('h1');
      archivedBlogs.push(blogTitle.textContent);
      archivedBlogs.sort((aName, bName) => aName.localeCompare(bName));
      otherDOMSelectors.archiveList.innerHTML = '';
      for (const blog of archivedBlogs) {
         createElement('li', blog, otherDOMSelectors.archiveList);
      }
      article.remove();
   }
   function deletePost(){
      const article = this.parentNode.parentNode;
      article.remove();
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
