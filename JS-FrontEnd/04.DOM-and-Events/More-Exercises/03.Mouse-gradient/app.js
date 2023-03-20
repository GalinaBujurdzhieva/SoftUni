function attachGradientEvents() {
    const gradientDiv = document.getElementById('gradient');
    const resultDiv = document.getElementById('result');
    gradientDiv.addEventListener('mousemove', gradientMove);
    gradientDiv.addEventListener('mouseout', gradientOut)
    
    function gradientMove(event){
        let result = Number(event.offsetX / gradientDiv.clientWidth * 100);
        resultDiv.textContent = `${Math.floor(result)}%`
    }
    function gradientOut(){
        resultDiv.textContent = '';
    }
}
