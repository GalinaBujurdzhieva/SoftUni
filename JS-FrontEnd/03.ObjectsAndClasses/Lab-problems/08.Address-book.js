function address(inputData){
    let addresses = {};
  for (const input of inputData) {
    let[name, address] = input.split(':');
    addresses[name] = address;
  }
  for (const name of Object.keys(addresses).sort()) {
    console.log(`${name} -> ${addresses[name]}`);
  }
  }
  
  address(['Tim:Doe Crossing',
  'Bill:Nelson Place',
  'Peter:Carlyle Ave',
  'Bill:Ornery Rd']
  )
  address(['Bob:Huxley Rd',
  'John:Milwaukee Crossing',
  'Peter:Fordem Ave',
  'Bob:Redwing Ave',
  'George:Mesta Crossing',
  'Ted:Gateway Way',
  'Bill:Gateway Way',
  'John:Grover Rd',
  'Peter:Huxley Rd',
  'Jeff:Gateway Way',
  'Jeff:Huxley Rd']
  )