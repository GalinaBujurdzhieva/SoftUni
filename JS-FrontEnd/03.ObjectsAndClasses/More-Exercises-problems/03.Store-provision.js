//I начин
function storeProvision(productsInStock, orderedProducts){
    productsInStock = productsInStock.map((currentNumber, index) => {
        if(index % 2 === 1){
           return Number(currentNumber);
        }
        else{
            return currentNumber;
        }
    })
    for (let i = 0; i < orderedProducts.length; i+=2){
        let productName = orderedProducts[i];
        let productQuantity = Number(orderedProducts[i+1]);
        if (productsInStock.includes(productName)) {
            let indexOfQuantity = productsInStock.indexOf(productName) + 1;
            productsInStock[indexOfQuantity] += productQuantity;    
        }
        else{
            productsInStock.push(productName);
            productsInStock.push(productQuantity);
        }
    }
   for (let i = 0; i < productsInStock.length; i+=2) {
    console.log(`${productsInStock[i]} -> ${productsInStock[i+1]}`)
   }
}

//II начин
function storeProvision(productsOnStock, orderedProducts){
    let combined = [...productsOnStock, ...orderedProducts];
    let productsObj = {}

    for (let i = 0; i < combined.length; i+=2) {
        let currentProduct = combined[i];
        let currentQuantity = Number(combined[i + 1]);
        if (!productsObj.hasOwnProperty(currentProduct)) {
            productsObj[currentProduct] = currentQuantity;
        }
        else{
            productsObj[currentProduct] += currentQuantity;
        }
    }
    for (const [key, value] of Object.entries(productsObj)) {
        console.log(`${key} -> ${value}`)
    }
}

storeProvision(['Chips', '5', 'CocaCola', '9', 'Bananas', '14', 'Pasta', '4', 'Beer', '2'],
    ['Flour', '44', 'Oil', '12', 'Pasta', '7', 'Tomatoes', '70', 'Bananas', '30']);
storeProvision(['Salt', '2', 'Fanta', '4', 'Apple', '14', 'Water', '4', 'Juice', '5'],
    ['Sugar', '44', 'Oil', '12', 'Apple', '7', 'Tomatoes', '7', 'Bananas', '30']);