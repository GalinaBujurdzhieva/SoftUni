function attachEventsListeners() {
    const inputDistance = document.getElementById('inputDistance');
    const outputDistance = document.getElementById('outputDistance');
    const inputDistanceUnit = document.getElementById('inputUnits');
    const resultDistanceUnit = document.getElementById('outputUnits');

    const convertBtn = document.getElementById('convert');
    convertBtn.addEventListener('click', convertFunc);

    function convertFunc() {
        const convertUnitsToMeter = {
            km: 1000,
            m: 1,
            cm: 0.01,
            mm: 0.001,
            mi: 1609.34,
            yrd: 0.9144,
            ft: 0.3048,
            in: 0.0254,
        };
        let valueInMeters = inputDistance.value * convertUnitsToMeter[inputDistanceUnit.value];
        outputDistance.value = valueInMeters / convertUnitsToMeter[resultDistanceUnit.value];
        outputDistance.disabled = false;
    }
    }
