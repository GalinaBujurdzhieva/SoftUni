function extract(content) {
    const text = document.getElementById(content).textContent;
    let regExp = /\((.*?)\)/g;
    let matches = Array.from([...text.match(regExp)].map((word) => word.substring(1, word.length - 1)));
    return matches.join('; ');
}
