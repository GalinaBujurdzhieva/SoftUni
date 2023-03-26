function attachEvents() {
    const POSTS_URL = 'http://localhost:3030/jsonstore/blog/posts';
    const COMMENTS_URL = 'http://localhost:3030/jsonstore/blog/comments';

    const loadPostsBtn = document.getElementById('btnLoadPosts');
    const selectElement = document.getElementById('posts');
    let postsObj = {};
    loadPostsBtn.addEventListener('click', loadPostsFunc);

    const viewPostBtn = document.getElementById('btnViewPost');
    const postTitleContainer = document.getElementById('post-title');
    const postBodyContainer = document.getElementById('post-body');
    const commentsUnorderedList = document.getElementById('post-comments');
    viewPostBtn.addEventListener('click', showPostDetailsFunc);

    function loadPostsFunc(){
        fetch(POSTS_URL)
        .then((res) => res.json())
        .then((data) =>{
           for (const key in data) {
            postsObj[key] = {};
            postsObj[key].title = data[key].title;
            postsObj[key].content = data[key].body;
            const currentOption = document.createElement('option');
            currentOption.value = key;
            currentOption.textContent = data[key].title;
            selectElement.appendChild(currentOption);
           }
        })

    }

    function showPostDetailsFunc(){
        const arrayWithOptions = Array.from(document.getElementsByTagName('option'));
        let selectedPostId = arrayWithOptions.find((opt) => opt.selected === true).value;

        fetch(COMMENTS_URL)
        .then((res) => res.json())
        .then((data) =>{
            console.log(data);
            postTitleContainer.textContent = postsObj[selectedPostId].title;
            postBodyContainer.textContent = postsObj[selectedPostId].content;
            commentsUnorderedList.innerHTML = '';
            for (const key in data) {
                if (data[key].postId === selectedPostId) {
                    createElement('li', data[key].text, commentsUnorderedList)
                }
            }
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