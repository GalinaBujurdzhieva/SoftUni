function cats(inputData){ 
    class Cat {
    constructor(name, age){
        this.name = name;
        this.age = age;
    }
    meow(){
        console.log(`${this.name}, age ${this.age} says Meow`);
    }
    }
    for (let input of inputData) {
        let[currentName, currentAge] = input.split(' ');
        let currentCar = new Cat(currentName, currentAge);
        currentCar.meow();
    }
}

cats(['Mellow 2', 'Tom 5']);
cats(['Candy 1', 'Poppy 3', 'Nyx 2']);