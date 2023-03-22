function solve() {
   document.querySelector('#btnSend').addEventListener('click', onClick);
   const textArea = document.getElementsByTagName('textarea')[0];
   const bestRestaurantDiv = document.querySelector('#bestRestaurant > p');
   const bestRestaurantWorkersDiv = document.querySelector('#workers > p');

   function onClick () {
      let restaurants = {};
      let inputArray = Array.from(JSON.parse(textArea.value));
      for (const input of inputArray) {
         let [restaurantName, workers] = input.split(' - ');
         if(!restaurants.hasOwnProperty(restaurantName)){
            restaurants[restaurantName] = {};
            restaurants[restaurantName].workers = [];
            restaurants[restaurantName].bestSalary = 0;
            restaurants[restaurantName].totalSalary = 0;
            restaurants[restaurantName].averageSalary = 0;
         }
         let splittedInput = workers.split(', ');
         for (let i = 0; i < splittedInput.length; i++) {
            let [workerName, workerSalary ] = splittedInput[i].split(' ');
            restaurants[restaurantName].workers.push({name: workerName, salary: workerSalary});
            restaurants[restaurantName].totalSalary += Number(workerSalary);
            if (restaurants[restaurantName].bestSalary < Number(workerSalary)) {
               restaurants[restaurantName].bestSalary = Number(workerSalary);
            }
         }
         restaurants[restaurantName].averageSalary = restaurants[restaurantName].totalSalary / restaurants[restaurantName].workers.length;
      
      }
      for (const [restaurantName, value] of Object.entries(restaurants).sort(([,restA],[,restB]) => restB.averageSalary - restA.averageSalary).splice(0, 1)) {
         bestRestaurantDiv.textContent = `Name: ${restaurantName} Average Salary: ${restaurants[restaurantName].averageSalary.toFixed(2)} Best Salary: ${restaurants[restaurantName].bestSalary.toFixed(2)}`;
         for (const {name, salary} of restaurants[restaurantName].workers.sort((workerA, workerB) => workerB.salary - workerA.salary)) {
            bestRestaurantWorkersDiv.textContent += `Name: ${name} With Salary: ${salary} `;
         }
         bestRestaurantWorkersDiv.textContent.trimEnd();
      }
   }
}

