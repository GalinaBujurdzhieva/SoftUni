function loadRepos() {
   const BASE_URL = 'https://api.github.com/users/testnakov/repos';
   const divContent = document.getElementById('res');
   fetch(BASE_URL)
   .then(res => res.text())
   .then((data) => {
      divContent.textContent = data;
   })
   .catch((er) =>
   console.error(er))
}
