function attachEventsListeners() {
    const daysInput = document.getElementById('days');
    const hoursInput = document.getElementById('hours');
    const minutesInput = document.getElementById('minutes');
    const secondsInput = document.getElementById('seconds');

    const daysConvertButton = document.getElementById('daysBtn');
    const hoursConvertButton = document.getElementById('hoursBtn');
    const minutesConvertButton = document.getElementById('minutesBtn');
    const secondsConvertButton = document.getElementById('secondsBtn');

    daysConvertButton.addEventListener('click', daysConverter);
    hoursConvertButton.addEventListener('click', hoursConverter);
    minutesConvertButton.addEventListener('click', minutesConverter);
    secondsConvertButton.addEventListener('click', secondsConverter);

    function daysConverter(){
        hoursInput.value = daysInput.value * 24;
        minutesInput.value = hoursInput.value * 60;
        secondsInput.value = minutesInput.value * 60;
    }
    function hoursConverter(){
        daysInput.value = hoursInput.value / 24;
        minutesInput.value = hoursInput.value * 60;
        secondsInput.value = minutesInput.value * 60;
    }
    function minutesConverter(){
        hoursInput.value = minutesInput.value / 60;
        daysInput.value = hoursInput.value / 24;
        secondsInput.value = minutesInput.value * 60;
    }
    function secondsConverter(){
        minutesInput.value = secondsInput.value / 60;
        hoursInput.value = minutesInput.value /60;
        daysInput.value = hoursInput.value / 24;
    }
}