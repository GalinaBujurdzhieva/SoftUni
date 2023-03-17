function bookShelvesFunc(inputData){
    let bookShelves = {};
    let genres = {};

for (const input of inputData) {
    if (input.includes('->')) {
        let[id, shelfGenre] = input.split(' -> ');
        if (!bookShelves.hasOwnProperty(id)) {
            bookShelves[id] = {};
            bookShelves[id].shelf = shelfGenre;
            bookShelves[id].books = [];
            bookShelves[id].bookCount = 0;
            genres[shelfGenre] = id;
        }
    }
    else if(input.includes(':')){
            let [bookTitle, restInfo] = input.split(': ');
            let [bookAuthor, genre] = restInfo.split(', ');
           if (genres.hasOwnProperty(genre)) {
            let genreId = genres[genre];
            bookShelves[genreId].books.push({title: bookTitle, author: bookAuthor});
            bookShelves[genreId].bookCount++;
           }
    } 
}
for (const [genreId, genre] of Object.entries(bookShelves).sort(([,shelfA], [,shelfB]) => shelfB.bookCount - shelfA.bookCount)) {
    console.log(`${genreId} ${bookShelves[genreId].shelf}: ${bookShelves[genreId].bookCount}`);
    for (const {title, author} of bookShelves[genreId].books.sort((bookA, bookB) => bookA.title.localeCompare(bookB.title))) {
        console.log(`--> ${title}: ${author}`);
    }
}   
}
bookShelvesFunc(['1 -> history', '1 -> action', 'Death in Time: Criss Bell, mystery', '2 -> mystery', '3 -> sci-fi', 'Child of Silver: Bruce Rich, mystery', 'Hurting Secrets: Dustin Bolt, action', 'Future of Dawn: Aiden Rose, sci-fi', 'Lions and Rats: Gabe Roads, history', '2 -> romance', 'Effect of the Void: Shay B, romance', 'Losing Dreams: Gail Starr, sci-fi', 'Name of Earth: Jo Bell, sci-fi', 'Pilots of Stone: Brook Jay, history']);
bookShelvesFunc(['1 -> mystery', '2 -> sci-fi', 'Child of Silver: Bruce Rich, mystery', 'Lions and Rats: Gabe Roads, history', 'Effect of the Void: Shay B, romance', 'Losing Dreams: Gail Starr, sci-fi', 'Name of Earth: Jo Bell, sci-fi']
)