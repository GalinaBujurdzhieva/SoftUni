function city(city){
    let keyValuePairs = Object.entries(city);
    
    for (const keyValuePair of keyValuePairs) {
        console.log(`${keyValuePair[0]} -> ${keyValuePair[1]}`);
    }
}

city({
    name: "Sofia",
    area: 492,
    population: 1238438,
    country: "Bulgaria",
    postCode: "1000"
});
city({
    name: "Plovdiv",
    area: 389,
    population: 1162358,
    country: "Bulgaria",
    postCode: "4000"
});