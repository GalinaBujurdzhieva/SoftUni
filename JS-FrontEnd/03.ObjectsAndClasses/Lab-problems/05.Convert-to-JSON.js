function convertToJSON(firstName, lastName, hairColor){
    let person = {
        name: firstName,
        lastName: lastName, 
        hairColor: hairColor,
    }
    let personAsJSON = JSON.stringify(person);
    console.log(personAsJSON);
}