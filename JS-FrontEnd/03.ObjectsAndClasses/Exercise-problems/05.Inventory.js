function heroRegister(inputData){
    let heroList = [];
for (const line of inputData) {
    let[heroName, heroLevel, items] = line.split(' / ');
    const currentHero = {
        name: heroName,
        level: Number(heroLevel),
        items,
        print: function(){
            console.log(`Hero: ${this.name}\nlevel => ${this.level}\nitems => ${this.items}`)
        }
    };
    heroList.push(currentHero);
}
heroList.sort((heroA, heroB) => heroA.level - heroB.level).forEach((hero) => hero.print());
}

heroRegister([
    'Isacc / 25 / Apple, GravityGun',
    'Derek / 12 / BarrelVest, DestructionSword',
    'Hes / 1 / Desolator, Sentinel, Antara'
    ]);
heroRegister([
    'Batman / 2 / Banana, Gun',
    'Superman / 18 / Sword',
    'Poppy / 28 / Sentinel, Antara'
    ]);
