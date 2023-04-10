function solve(input){
    let number = Number(input.shift());
    let initialData = input.splice(0, number);
    let sprint = {};

    for (const task of initialData) {
        let commandTokens = task.split(':');
        let assignee = commandTokens.shift();
        let [taskId, title, status, points] = commandTokens;

        if(!sprint.hasOwnProperty(assignee)){
            sprint[assignee] = {};
            sprint[assignee].tasks = [];
        }
        sprint[assignee].tasks.push({taskId, title, status, points});
    }

    for (const changingCommand of input) {
        let changingCommandTokens = changingCommand.split(':');
        let command = changingCommandTokens.shift();

        if(command === 'Add New'){
            let [assignee, taskId, title, status, points] = changingCommandTokens;
            if(!sprint.hasOwnProperty(assignee)){
                console.log(`Assignee ${assignee} does not exist on the board!`)
            }
            else{
                sprint[assignee].tasks.push({taskId, title, status, points});
            }
        }
        else if(command === 'Change Status'){
            let [assignee, taskId, newStatus] = changingCommandTokens;
            if(!sprint.hasOwnProperty(assignee)){
                console.log(`Assignee ${assignee} does not exist on the board!`);
            }
            else{
                let taskToBeModified = sprint[assignee].tasks.find((task) => task.taskId === taskId);
                if(!taskToBeModified){
                    console.log(`Task with ID ${taskId} does not exist for ${assignee}!`);
                }
                else{
                    let newTask = {taskId, title, status: newStatus, points};
                    let index = sprint[assignee].tasks.indexOf(taskToBeModified);
                    sprint[assignee].tasks.splice(index, 1, newTask);
                }
            }

        }
        else if(command === 'Remove Task'){
            let [assignee, index] = changingCommandTokens;
            if(!sprint.hasOwnProperty(assignee)){
                console.log(`Assignee ${assignee} does not exist on the board!`);
            }
            else{
                if (index < 0 || index >= sprint[assignee].tasks.length){
                    console.log('Index is out of range!')
                }
                else{
                    sprint[assignee].tasks.splice(index, 1);
                }
            }
        }
       
       }

       let toDoPoints = 0;
       let inProgressPoints = 0;
       let codeReviewPoints = 0;
       let donePoints = 0;

       for (const tasksObj of Object.values(sprint)) {
        for (const tasks of Object.values(tasksObj)) {
           for (const task of tasks) {
             switch (task.status) {
                case 'ToDo':
                    toDoPoints += Number(task.points);
                    break;
                case 'In Progress':
                    inProgressPoints += Number(task.points);
                    break;
                case 'Code Review':
                    codeReviewPoints += Number(task.points);
                    break;
                case 'Done':
                    donePoints += Number(task.points);
                    break;
             }
           }
        }
       }
       console.log(`ToDo: ${toDoPoints}pts`);
       console.log(`In Progress: ${inProgressPoints}pts`);
       console.log(`Code Review: ${codeReviewPoints}pts`);
       console.log(`Done Points: ${donePoints}pts`);

       let isSprintSuccessful = donePoints >= toDoPoints + inProgressPoints + codeReviewPoints;
       isSprintSuccessful ? console.log('Sprint was successful!') : console.log('Sprint was unsuccessful...');

}


solve([
        '5',
        'Kiril:BOP-1209:Fix Minor Bug:ToDo:3',
        'Mariya:BOP-1210:Fix Major Bug:In Progress:3',
        'Peter:BOP-1211:POC:Code Review:5',
        'Georgi:BOP-1212:Investigation Task:Done:2',
        'Mariya:BOP-1213:New Account Page:In Progress:13',
        'Add New:Kiril:BOP-1217:Add Info Page:In Progress:5',
        'Change Status:Peter:BOP-1290:ToDo',
        'Remove Task:Mariya:1',
        'Remove Task:Joro:1',
    ])

solve(  [
    '4',
    'Kiril:BOP-1213:Fix Typo:Done:1',
    'Peter:BOP-1214:New Products Page:In Progress:2',
    'Mariya:BOP-1215:Setup Routing:ToDo:8',
    'Georgi:BOP-1216:Add Business Card:Code Review:3',
    'Add New:Sam:BOP-1237:Testing Home Page:Done:3',
    'Change Status:Georgi:BOP-1216:Done',
    'Change Status:Will:BOP-1212:In Progress',
    'Remove Task:Georgi:3',
    'Change Status:Mariya:BOP-1215:Done',
])
