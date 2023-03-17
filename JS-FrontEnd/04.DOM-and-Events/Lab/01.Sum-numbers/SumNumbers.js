function calc() {
    const numOne = document.getElementById('num1');
    const numTwo = document.getElementById('num2');
    let sum = parseInt(numOne.value) + parseInt(numTwo.value) 
    const result = document.getElementById('sum');
    result.value = sum;
}
