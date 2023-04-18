function flightSchedule(input){
   let flightsInfo = input[0];
   let flightsWithChangedStatuses = input[1];
   let status = input[2];
   let flights = {};
   let changedFlights = {};
   for (const flightInfo of flightsInfo) {
      let [id, ...dest] = flightInfo.split(' ');
      let destination = dest.join(' ');
      flights[id] = {destination}
   }
   for (const flight of flightsWithChangedStatuses) {
      let [id, status] = flight.split(' ');
      if (flights.hasOwnProperty(id)) {
         flights[id].status = status;
         changedFlights[id] = {destination: flights[id].destination, status};
      }
   }
   if (status[0] === 'Ready to fly'){
      for (const [key, value] of Object.entries(flights)) {
         if (Object.keys(value).length === 1) {
            flights[key].status = 'Ready to fly';
            console.log(`{ Destination: '${flights[key].destination}', Status: '${flights[key].status}' }`)
         }
      }
   }
   else{
      for (const [key, value] of Object.entries(flights)) {
         if (Object.keys(value).length === 2) {
            console.log(`{ Destination: '${flights[key].destination}', Status: '${flights[key].status}' }`)
         }
      }
   }
}
flightSchedule([['WN269 Delaware', 
'FL2269 Oregon', 
 'WN498 Las Vegas', 
 'WN3145 Ohio', 
 'WN612 Alabama', 
 'WN4010 New York', 
 'WN1173 California', 
 'DL2120 Texas', 
 'KL5744 Illinois', 
 'WN678 Pennsylvania'], 
 ['DL2120 Cancelled', 
'WN612 Cancelled', 
'WN1173 Cancelled', 
'SK430 Cancelled'], 
['Cancelled'] 
])

flightSchedule([['WN269 Delaware', 
'FL2269 Oregon', 
 'WN498 Las Vegas', 
 'WN3145 Ohio', 
 'WN612 Alabama', 
 'WN4010 New York', 
 'WN1173 California', 
 'DL2120 Texas', 
 'KL5744 Illinois', 
 'WN678 Pennsylvania'], 
 ['DL2120 Cancelled', 
'WN612 Cancelled', 
'WN1173 Cancelled', 
'SK330 Cancelled'], 
['Ready to fly'] 
] )
