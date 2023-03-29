function pianist(input){
    let songsData = {};
    let number = Number(input.shift());
    let piecesArray = input.splice(0, number);

  for (const pieceInfo of piecesArray) {
    let [piece, composer, key] = pieceInfo.split('|');
    songsData[piece] = {composer: composer, key: key};
  }

  let commandsObj = {
    'Add': addPiece,
    'Remove': removePiece,
    'ChangeKey': changeKey
  }

  for (const line of input) {
    let pieceParts = line.split('|');
    let command = pieceParts.shift();
    if(command === 'Stop'){
      break;
    }
    commandsObj[command](...pieceParts);
  }

  function addPiece(piece, composer, key){
    if(!songsData.hasOwnProperty(piece)){
      songsData[piece] = {composer, key};
      console.log(`${piece} by ${composer} in ${key} added to the collection!`);
  }
  else{
    console.log(`${piece} is already in the collection!`);
  }
}

  function removePiece(piece){
    if (songsData.hasOwnProperty(piece)) {
      delete songsData[piece];
      console.log(`Successfully removed ${piece}!`);
    }
    else{
      console.log(`Invalid operation! ${piece} does not exist in the collection.`);
    }
  }

  function changeKey(piece, newKey){
    if (songsData.hasOwnProperty(piece)) {
      songsData[piece].key = newKey;
      console.log(`Changed the key of ${piece} to ${newKey}!`);
    }
    else{
      console.log(`Invalid operation! ${piece} does not exist in the collection.`);
    }
  }
  for (piece in songsData) {
    console.log(`${piece} -> Composer: ${songsData[piece].composer}, Key: ${songsData[piece].key}`)
    }
}
