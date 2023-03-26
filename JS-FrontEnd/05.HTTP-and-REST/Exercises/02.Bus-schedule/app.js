function solve() {
    const BASE_URL = 'http://localhost:3030/jsonstore/bus/schedule/';
    const infoContainer = document.querySelector('#info > span');
    const arriveBtn = document.getElementById('arrive');
    const departBtn = document.getElementById('depart');
    let [departId, departLocation] = ['depot', null]

    function depart() {
        departBtn.disabled = true;
        arriveBtn.disabled = false;

        fetch(`${BASE_URL}${departId}`)
        .then((res) => res.json())
        .then((data) =>{
            departLocation = data.name;
            infoContainer.textContent = `Next stop ${departLocation}`;
            departId = data.next;
        })
        .catch((err) =>{
            infoContainer.textContent = "Error";
            departBtn.disabled = true;
            arriveBtn.disabled = true;
        })
    }

    async function arrive() {
        departBtn.disabled = false;
        arriveBtn.disabled = true;
        infoContainer.textContent = `Arriving at ${departLocation}`;
    }

    return {
        depart,
        arrive
    };
}

let result = solve();
