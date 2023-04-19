function browserHistory(browser, actions){
    for (const action of actions) {
        if (action.includes('Open ')) {
            let openedPage = action.split('Open ')[1];
            browser['Open Tabs'].push(openedPage);
            browser['Browser Logs'].push(action);
        }
        else if (action.includes('Close ')) {
            let closedPage = action.split('Close ')[1];
            let validOpenedPage = browser['Open Tabs'].find((p) => p === closedPage);
            if (validOpenedPage) {
                let index = browser['Open Tabs'].indexOf(validOpenedPage);
                browser['Open Tabs'].splice(index, 1);
                browser['Recently Closed'].push(closedPage);
                browser['Browser Logs'].push(action);
            }
        }
        else if (action === 'Clear History and Cache') {
            browser['Open Tabs'] = [];
            browser['Recently Closed'] = [];
            browser['Browser Logs'] = [];
        }
    }
    console.log(browser['Browser Name']);
    console.log(`Open Tabs: ${browser['Open Tabs'].join(', ')}`);
    console.log(`Recently Closed: ${browser['Recently Closed'].join(', ')}`);
    console.log(`Browser Logs: ${browser['Browser Logs'].join(', ')}`);
}

browserHistory({"Browser Name":"Google Chrome","Open Tabs":["Facebook","YouTube","Google Translate"],
"Recently Closed":["Yahoo","Gmail"],
"Browser Logs":["Open YouTube","Open Yahoo","Open Google Translate","Close Yahoo","Open Gmail","Close Gmail","Open Facebook"]},
["Close Facebook", "Open StackOverFlow", "Open Google"]
)
browserHistory({"Browser Name":"Mozilla Firefox",
"Open Tabs":["YouTube"],
"Recently Closed":["Gmail", "Dropbox"],
"Browser Logs":["Open Gmail", "Close Gmail", "Open Dropbox", "Open YouTube", "Close Dropbox"]},
["Open Wikipedia", "Clear History and Cache", "Open Twitter"]
)
