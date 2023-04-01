function solve (input){
let groceries = input.splice(0, 1);
let groceriesArray = groceries[0].split('!')

let commandPattern = {
    'Urgent': addGrocery,
    'Unnecessary': removeGrocery,
    'Correct': changeGrocery,
    'Rearrange': rearrangeGrocery,
}

for (const commandLine of input) {
    if(commandLine === 'Go Shopping!'){
        break;
    }
    let commandTokens = commandLine.split(' ');
    let command = commandTokens.splice(0, 1);
    commandPattern[command](...commandTokens);
}

function addGrocery(grocery){
let neededGrocery = groceriesArray.find((g) => g === grocery);
if (!neededGrocery){
    groceriesArray.unshift(grocery);
   }
}

function removeGrocery(grocery){
    let neededGrocery = groceriesArray.find((g) => g === grocery);
    if(neededGrocery){
        let index = groceriesArray.indexOf(grocery);
        groceriesArray.splice(index, 1);
    }
}

function changeGrocery(oldGrocery, newGrocery){
    let neededGrocery = groceriesArray.find((g) => g === oldGrocery);
    if(neededGrocery){
        let index = groceriesArray.indexOf(oldGrocery);
        groceriesArray.splice(index, 1, newGrocery);
    }
}

function rearrangeGrocery(grocery){
    let neededGrocery = groceriesArray.find((g) => g === grocery);
    if(neededGrocery){
        let index = groceriesArray.indexOf(grocery);
        let lastGrocery = groceriesArray.splice(index, 1);
        groceriesArray.push(lastGrocery[0]);
    }
}
console.log(groceriesArray.join(', '));
}

solve((["Tomatoes!Potatoes!Bread",
"Unnecessary Milk",
"Urgent Tomatoes",
"Go Shopping!"])
)

solve((["Milk!Pepper!Salt!Water!Banana",
"Urgent Salt",
"Unnecessary Grapes",
"Correct Pepper Onion",
"Rearrange Grapes",
"Correct Tomatoes Potatoes",
"Go Shopping!"])
)
