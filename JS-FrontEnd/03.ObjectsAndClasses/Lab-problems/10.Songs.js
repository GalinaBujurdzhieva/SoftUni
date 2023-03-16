function songs(input){
    let desiredType = input[input.length - 1];
    class Song {
        constructor(type, name, length){
            this.type = type;
            this.name = name;
            this.length = length
        }
    }
   for (let i = 1; i < input.length - 1; i++) {
    let currentSongInput = input[i].split('_');
    let currentSong = new Song(currentSongInput[0], currentSongInput[1], currentSongInput[2]);
    if (currentSong.type === desiredType) {
        console.log(currentSong.name); 
    }
    else if(desiredType === 'all'){
        console.log(currentSong.name)
    }
   }
}

songs([3,
    'favourite_DownTown_3:14',
    'favourite_Kiss_4:16',
    'favourite_Smooth Criminal_4:01',
    'favourite']
    );
songs([4,
        'favourite_DownTown_3:14',
        'listenLater_Andalouse_3:24',
        'favourite_In To The Night_3:58',
        'favourite_Live It Up_3:48',
        'listenLater']
        );
songs([2,
    'like_Replay_3:15',
    'ban_Photoshop_3:48',
    'all'])
