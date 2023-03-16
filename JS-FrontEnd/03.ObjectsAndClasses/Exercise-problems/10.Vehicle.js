class Vehicle{
    constructor(type, model, parts, fuel){
       this.type = type;
       this.model = model;
       this.parts = {
           engine: parts.engine,
           power: parts.power,
           quality: parts.engine * parts.power
       };
       this.fuel = fuel;
   }
       drive(fuel){
           this.fuel -= fuel
    }
}

let partsOne = { engine: 6, power: 100 };
let vehicleOne = new Vehicle('a', 'b', partsOne, 200);
vehicleOne.drive(100);
console.log(vehicleOne.fuel);
console.log(vehicleOne.parts.quality);

let parts = {engine: 9, power: 500};
let vehicle = new Vehicle('l', 'k', parts, 840);
vehicle.drive(20);
console.log(vehicle.fuel);