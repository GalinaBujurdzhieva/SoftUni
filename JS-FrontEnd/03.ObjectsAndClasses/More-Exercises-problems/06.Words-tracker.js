function wordsTracker(inputData){
    let wordsToLookFor = inputData.shift().split(' ');
    const wordsAsObject = {};

    for (const word of wordsToLookFor) {
        wordsAsObject[word] = inputData.filter(w => w === word).length;
    }
    for (const foundWord of Object.entries(wordsAsObject).sort((wordA, wordB) => wordB[1] - wordA[1])) {
        console.log(`${foundWord[0]} - ${foundWord[1]}`);
    }
}
wordsTracker([
    'this sentence', 
    'In', 'this', 'sentence', 'you', 'have', 'to', 'count', 'the', 'occurrences', 'of', 'the', 'words', 'this', 'and', 'sentence', 'because', 'this', 'is', 'your', 'task']);
wordsTracker([
    'is the', 
    'first', 'sentence', 'Here', 'is', 'another', 'the', 'And', 'finally', 'the', 'the', 'sentence']);