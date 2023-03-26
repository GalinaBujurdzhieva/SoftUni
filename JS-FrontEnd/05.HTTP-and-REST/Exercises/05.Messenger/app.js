function attachEvents() {
    const BASE_URL = 'http://localhost:3030/jsonstore/messenger';
    const textArea = document.getElementById('messages');
    const [authorInput, contentInput] = document.querySelectorAll('#controls input');

    const submitBtn = document.getElementById('submit');
    submitBtn.addEventListener('click', addNewMessage);

    const refreshBtn = document.getElementById('refresh');
    refreshBtn.addEventListener('click', getAllMessages);

    function addNewMessage(){
        const newMessage = {
            author : authorInput.value,
            content: contentInput.value,
        }
        fetch(BASE_URL, {
            mode: 'no-cors',
            method: 'POST',
            headers: {'contentType': 'application/json'},
            body: JSON.stringify(newMessage)
        })
        authorInput.value ='';
        contentInput.value ='';
    }

    function getAllMessages(){
        let output = [];
        fetch(BASE_URL)
        .then((res) => res.json())
        .then((data) =>{
            for (const key in data) {
                output.push(`${data[key].author}: ${data[key].content}`); 
            }
            textArea.textContent += output.join('\n');
        })
    }
}
attachEvents();