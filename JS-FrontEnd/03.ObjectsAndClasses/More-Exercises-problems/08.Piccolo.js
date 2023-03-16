// I начин
function piccolo(input){
    let parkingLot = new Set();
    for (const line of input) {
        let [direction, car] = line.split(', ');
        if (direction === 'IN') {
            parkingLot.add(car);
        }
        else{
            parkingLot.delete(car);
        } 
    }
    parkingLot.size === 0 ? console.log('Parking Lot is Empty') : console.log([...parkingLot].sort().join('\n'));
    }

// II начин
function piccolo(input){
    let parkingLot =[];
    for (const line of input) {
            let [direction, car] = line.split(', ');
            if (direction === 'IN' && !parkingLot.includes(car)) {
                parkingLot.push(car);
            }
    else if (direction === 'OUT' && parkingLot.includes(car)){
                    parkingLot.splice(parkingLot.indexOf(car), 1);
            }
    } 
        parkingLot.length === 0 ? console.log('Parking Lot is Empty') : console.log(parkingLot.sort().join('\n'));
}

piccolo(['IN, CA2844AA',
'IN, CA1234TA',
'OUT, CA2844AA',
'IN, CA9999TT',
'IN, CA2866HI',
'OUT, CA1234TA',
'IN, CA2844AA',
'OUT, CA2866HI',
'IN, CA9876HH',
'IN, CA2822UU'])
piccolo(['IN, CA2844AA',
'IN, CA1234TA',
'OUT, CA2844AA',
'OUT, CA1234TA'])