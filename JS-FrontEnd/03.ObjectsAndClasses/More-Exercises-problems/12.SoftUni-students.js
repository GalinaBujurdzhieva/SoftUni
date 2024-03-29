function softUniStudentsFunc(inputData){
    let courses = {};

    for (const input of inputData) {
        if (input.includes(':')) {
            let [courseName, capacity] = input.split(': ');
            if(!courses.hasOwnProperty(courseName)){
                courses[courseName] = {};
                courses[courseName].capacity = parseInt(capacity);
                courses[courseName].students = [];
                courses[courseName].studentsCount = 0;
            }
            else{
                courses[courseName].capacity += parseInt(capacity);
            }
        }
        else{
            let studentsInfo = input.split(' ');
            let [studentUsername, studentsCreditsCount] = studentsInfo[0].split('[');
            studentsCreditsCount = parseInt(studentsCreditsCount.substring(0, studentsCreditsCount.length - 1));
            let studentsEmail = studentsInfo[3];
            let nameOfCourse = studentsInfo[5];

            if (courses.hasOwnProperty(nameOfCourse) && courses[nameOfCourse].capacity > courses[nameOfCourse].studentsCount) {
                courses[nameOfCourse].studentsCount += 1;
                courses[nameOfCourse].students.push({username: studentUsername, email: studentsEmail, credits: studentsCreditsCount})
            }
        }
    }
    for (const [courseName, student] of Object.entries(courses).sort(([,corseA], [,corseB]) => corseB.studentsCount - corseA.studentsCount)) {
        let placesLeft = courses[courseName].capacity - courses[courseName].studentsCount;
        console.log(`${courseName}: ${placesLeft} places left`);
        for (const {credits, username, email} of courses[courseName].students.sort((studentA, studentB) => studentB.credits - studentA.credits)) {
            console.log(`--- ${credits}: ${username}, ${email}`);
        }
    }
}
softUniStudentsFunc(['JavaBasics: 2', 'user1[25] with email user1@user.com joins C#Basics', 'C#Advanced: 3', 'JSCore: 4', 'user2[30] with email user2@user.com joins C#Basics', 'user13[50] with email user13@user.com joins JSCore', 'user1[25] with email user1@user.com joins JSCore', 'user8[18] with email user8@user.com joins C#Advanced', 'user6[85] with email user6@user.com joins JSCore', 'JSCore: 2', 'user11[3] with email user11@user.com joins JavaBasics', 'user45[105] with email user45@user.com joins JSCore', 'user007[20] with email user007@user.com joins JSCore', 'user700[29] with email user700@user.com joins JSCore', 'user900[88] with email user900@user.com joins JSCore']
);
softUniStudentsFunc(['JavaBasics: 15', 'user1[26] with email user1@user.com joins JavaBasics', 'user2[36] with email user11@user.com joins JavaBasics', 'JavaBasics: 5', 'C#Advanced: 5', 'user1[26] with email user1@user.com joins C#Advanced', 'user2[36] with email user11@user.com joins C#Advanced', 'user3[6] with email user3@user.com joins C#Advanced', 'C#Advanced: 1', 'JSCore: 8', 'user23[62] with email user23@user.com joins JSCore']);