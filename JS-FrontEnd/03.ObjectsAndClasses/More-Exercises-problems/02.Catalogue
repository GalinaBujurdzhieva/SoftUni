function catalogFunc(input){
let catalog = {};
for (const line of input) {
   let letter = line.substring(0, 1);
   let [name, price] = line.split(' : ')
   if (!catalog.hasOwnProperty(letter)) {
      catalog[letter] = [];
   }
   catalog[letter].push({name, price})
}
for (const [key, value] of Object.entries(catalog).sort(([keyA,], [keyB,]) => keyA.localeCompare(keyB))) {
   console.log(key);
   for (const product of value.sort((prodA, prodB) => prodA.name.localeCompare(prodB.name))) {
      console.log(` ${product.name}: ${product.price}`)
   }
}
}

catalogFunc([
   'Appricot : 20.4', 
   'Fridge : 1500', 
   'TV : 1499', 
   'Deodorant : 10', 
   'Boiler : 300', 
   'Apple : 1.25', 
   'Anti-Bug Spray : 15', 
   'T-Shirt : 10' 
   ])

   catalogFunc([ 
      'Omlet : 5.4', 
      'Shirt : 15', 
      'Cake : 59' 
      ])
