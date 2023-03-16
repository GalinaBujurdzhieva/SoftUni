function employees(inputData){
    for (const input of inputData) {
        let newEmployee = {
            name: input,
            'personal number': input.length,
        };
        console.log(`Name: ${newEmployee.name} -- Personal Number: ${newEmployee['personal number']}`)
    }
}

employees([
    'Silas Butler',
    'Adnaan Buckley',
    'Juan Peterson',
    'Brendan Villarreal'
    ]);
employees([
        'Samuel Jackson',
        'Will Smith',
        'Bruce Willis',
        'Tom Holland'
]);