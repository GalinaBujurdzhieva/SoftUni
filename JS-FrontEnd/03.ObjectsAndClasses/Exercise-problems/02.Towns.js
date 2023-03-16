//02. Towns
function towns(inputData){
    for (const input of inputData) {
        let [townName, latitude, longitude] = input.split(' | ')
        let currentTown = {
            town : townName,
            latitude: Number(latitude).toFixed(2),
            longitude: Number(longitude).toFixed(2)
        }
        console.log(currentTown);
    }
}

towns(['Sofia | 42.696552 | 23.32601',
'Beijing | 39.913818 | 116.363625']);
towns(['Plovdiv | 136.45 | 812.575']);