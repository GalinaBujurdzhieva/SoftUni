function toggle() {
    const spanWithButton = document.getElementsByClassName('button')[0];
    const paragraph = document.getElementById('extra');

    if (spanWithButton.textContent === 'More') {
        spanWithButton.textContent = 'Less';
        paragraph.style.display = 'block';
    }
    else if(spanWithButton.textContent === 'Less'){
        spanWithButton.textContent = 'More';
        paragraph.style.display = 'none';
    }
}