function attachEvents() {
    const BASE_URL = 'http://localhost:3030/jsonstore/forecaster/locations';
    const TODAYS_FORECAST_URL = 'http://localhost:3030/jsonstore/forecaster/today/';
    const UPCOMING_FORECAST_URL = 'http://localhost:3030/jsonstore/forecaster/upcoming/';
    
    const location = document.getElementById('location');
    const forecastDiv = document.getElementById('forecast');
    const currentForecastDiv = document.getElementById('current');
    const upcomingForecastDiv = document.getElementById('upcoming');

    const getWeatherBtn = document.getElementById('submit');
    getWeatherBtn.addEventListener('click', showWeatherForecast);

    const weatherIcons ={
        'Sunny': '☀',
        'Partly sunny': '⛅',
        'Overcast': '☁',
        'Rain': '☂',
    }

    function showWeatherForecast(){
        fetch(BASE_URL)
        .then((res) => res.json())
        .then((data) =>{
            let neededLocation = data.find((loc) => loc.name === location.value);
            forecastDiv.style.display = 'block';

                fetch(TODAYS_FORECAST_URL + neededLocation.code)
               .then((res) => res.json())
               .then((data) =>{
                   const divWithClassForecasts = createElement('div', '', currentForecastDiv, '', 'forecasts');
                   const spanWithWeatherIcon = createElement('span', weatherIcons[data.forecast.condition], divWithClassForecasts, '', 'condition');
                   spanWithWeatherIcon.classList.add('symbol');
                   const spanWithClassCondition = createElement('span', '', divWithClassForecasts, '', 'condition');
                   createElement('span', data.name, spanWithClassCondition, '', 'forecast-data');
                   let degrees = `${data.forecast.low}°/${data.forecast.high}°`;
                   createElement('span', degrees, spanWithClassCondition, '', 'forecast-data');
                   createElement('span', data.forecast.condition, spanWithClassCondition, '', 'forecast-data');
               })

               fetch(UPCOMING_FORECAST_URL + neededLocation.code)
               .then((res) => res.json())
               .then((data) => {
                    const divWithClassForecastInfo = createElement('div', '', upcomingForecastDiv, '', 'forecast-info');
                    for (const dayForecast of data.forecast) {
                        const spanWithClassUpcoming = createElement('span', '', divWithClassForecastInfo, '',
                        'upcoming');
                        createElement('span', weatherIcons[dayForecast.condition], spanWithClassUpcoming, '', 'symbol');
                        let degrees = `${dayForecast.low}°/${dayForecast.high}°`;
                        createElement('span', degrees, spanWithClassUpcoming, '', 'forecast-data');
                        createElement('span', dayForecast.condition, spanWithClassUpcoming, '', 'forecast-data');
                    }
               })
        })
        .catch((err) =>{
            forecastDiv.style.display = 'block';
            forecastDiv.textContent = 'Error'
        })
    }

    function createElement(type, content, parentNode, id, classes, attributes){
		const htmlElement = document.createElement(type);
	
		if (content && type === 'input') {
		  htmlElement.value = content;
		}
		if (content && type !== 'input') {
		  htmlElement.textContent = content;
		}
		if (id) {
		  htmlElement.id = id;
		}
		if (classes) {
		  htmlElement.classList.add(classes)
		}
		if (attributes) {
		  for (const key in attributes) {
			htmlElement.setAttribute(key, attributes[key])
		  }
		}
		if (parentNode) {
		  parentNode.appendChild(htmlElement);
		}
		return htmlElement;
	  }
}
attachEvents();