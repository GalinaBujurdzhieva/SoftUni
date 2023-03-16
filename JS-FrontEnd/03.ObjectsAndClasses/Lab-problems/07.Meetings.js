function meetings(inputData){
    let scheduledMeetings = {};

  for (const input of inputData) {
    let [weekday, person] = input.split(' ');
    let result = (weekday in scheduledMeetings);
    if (result) {
        console.log(`Conflict on ${weekday}!`);
    }
    else{
    scheduledMeetings[weekday] = person;
    console.log(`Scheduled for ${weekday}`);
    }
  }

  for (const [key, value] of Object.entries(scheduledMeetings)) {
    console.log(`${key} -> ${value}`);
  }
}
meetings(['Monday Peter',
'Wednesday Bill',
'Monday Tim',
'Friday Tim']
);
meetings(['Friday Bob',
'Saturday Ted',
'Monday Bill',
'Monday John',
'Wednesday George']
)