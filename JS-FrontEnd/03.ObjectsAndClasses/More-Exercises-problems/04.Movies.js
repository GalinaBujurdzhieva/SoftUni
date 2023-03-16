function movies(inputData){
    let moviesList = {};
for (const input of inputData) {
    if (input.startsWith('addMovie ')) {
        let name = input.substring(9, input.length);
        moviesList[name] = {}
        moviesList[name].name = name;
    }
    else if (input.includes(' directedBy ')) {
        let[movieName, movieDirector] = input.split(' directedBy ');
        if (moviesList.hasOwnProperty(movieName)) {
            moviesList[movieName].director = movieDirector;
        }
    } 
    else if (input.includes(' onDate ')) {
        let[movieName, movieDate] = input.split(' onDate ');
        if (moviesList.hasOwnProperty(movieName)) {
            moviesList[movieName].date = movieDate;
        }
    }
}
for (const movie in moviesList) {
    let movieKeys = Object.keys(moviesList[movie]);
    if (movieKeys.length === 3) {
        console.log(JSON.stringify(moviesList[movie]));
    }
}
}

movies([
    'addMovie Fast and Furious',
    'addMovie Godfather',
    'Inception directedBy Christopher Nolan',
    'Godfather directedBy Francis Ford Coppola',
    'Godfather onDate 29.07.2018',
    'Fast and Furious onDate 30.07.2018',
    'Batman onDate 01.08.2018',
    'Fast and Furious directedBy Rob Cohen'
    ]);
movies([
    'addMovie The Avengers',
    'addMovie Superman',
    'The Avengers directedBy Anthony Russo',
    'The Avengers onDate 30.07.2010',
    'Captain America onDate 30.07.2010',
    'Captain America directedBy Joe Russo'
    ]);