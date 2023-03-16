function phoneBook(inputNumbers){
    let myPhoneBook = {};
    for (const inputNumber of inputNumbers) {
        let[name, phone]  = inputNumber.split(' ');
        myPhoneBook[name] = phone;
    }
    for (const [key, value] of Object.entries(myPhoneBook)) {
        console.log(`${key} -> ${value}`);
    }
}

phoneBook(['Tim 0834212554',
'Peter 0877547887',
'Bill 0896543112',
'Tim 0876566344']
);
phoneBook(['George 0552554',
'Peter 087587',
'George 0453112',
'Bill 0845344']
)