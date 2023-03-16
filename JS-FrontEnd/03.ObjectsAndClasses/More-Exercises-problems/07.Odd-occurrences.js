function oddOccurrences(input){
    let splittedInput = input.split(' ')
    let wordWithOddCountObject = {};
    let arrayWithKeys = []

    for (const word of splittedInput) {
        newWord = word.toLowerCase();
        if (!wordWithOddCountObject.hasOwnProperty(newWord)) {
            wordWithOddCountObject[newWord] = 1;
        }
        else{
            wordWithOddCountObject[newWord]++;
        }
    }
    for (const [key, value] of Object.entries(wordWithOddCountObject)) {
        if (value % 2 === 1) {
            arrayWithKeys.push(key);
        }
    }
    console.log(arrayWithKeys.join(' '));
}
oddOccurrences('Java C# Php PHP Java PhP 3 C# 3 1 5 C#');
oddOccurrences('Cake IS SWEET is Soft CAKE sweet Food');