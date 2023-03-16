function convertToObject(JSONString){
    let myObject = JSON.parse(JSONString);
    let keyValuePairs = Object.entries(myObject);
    for (const keyValuePair of keyValuePairs) {
        console.log(`${keyValuePair[0]}: ${keyValuePair[1]}`);
    }
}

convertToObject('{"name": "George", "age": 40, "town": "Sofia"}');
convertToObject('{"name": "Peter", "age": 35, "town": "Plovdiv"}')