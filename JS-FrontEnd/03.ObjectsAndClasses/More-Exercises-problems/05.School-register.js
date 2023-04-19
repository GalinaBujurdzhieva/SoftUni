function schoolRegister(input){
    let register = {};
for (const student of input) {
    let studentInfo = student.split(', ');
    let name = studentInfo[0].split('Student name: ')[1];
    let grade = Number(studentInfo[1].split('Grade: ')[1]);
    let averageStudentScore = Number(studentInfo[2].split('Graduated with an average score: ')[1]);
    if (averageStudentScore >= 3) {
        if (!register.hasOwnProperty(grade + 1)) {
            register[grade + 1] = {};
            register[grade + 1].students = [];
            register[grade + 1].totalScore = 0;
        }
        register[grade + 1].students.push(name);
        register[grade + 1].totalScore += averageStudentScore;
    }
}
for (const [key, value] of Object.entries(register).sort(([keyA,], [keyB,]) => keyA - keyB)) {
    console.log(`${key} Grade`);
    console.log(`List of students: ${register[key].students.join(', ')}`);
    let averageScore = register[key].totalScore / register[key].students.length
    console.log(`Average annual score from last year: ${averageScore.toFixed(2)}\n`);
}
}
schoolRegister([
"Student name: Mark, Grade: 8, Graduated with an average score: 4.75",
    "Student name: Ethan, Grade: 9, Graduated with an average score: 5.66",
    "Student name: George, Grade: 8, Graduated with an average score: 2.83",
    "Student name: Steven, Grade: 10, Graduated with an average score: 4.20",
    "Student name: Joey, Grade: 9, Graduated with an average score: 4.90",
    "Student name: Angus, Grade: 11, Graduated with an average score: 2.90",
    "Student name: Bob, Grade: 11, Graduated with an average score: 5.15",
    "Student name: Daryl, Grade: 8, Graduated with an average score: 5.95",
    "Student name: Bill, Grade: 9, Graduated with an average score: 6.00",
    "Student name: Philip, Grade: 10, Graduated with an average score: 5.05",
    "Student name: Peter, Grade: 11, Graduated with an average score: 4.88",
    "Student name: Gavin, Grade: 10, Graduated with an average score: 4.00"
])

schoolRegister([
    'Student name: George, Grade: 5, Graduated with an average score: 2.75',
    'Student name: Alex, Grade: 9, Graduated with an average score: 3.66',
    'Student name: Peter, Grade: 8, Graduated with an average score: 2.83',
    'Student name: Boby, Grade: 5, Graduated with an average score: 4.20',
    'Student name: John, Grade: 9, Graduated with an average score: 2.90',
    'Student name: Steven, Grade: 2, Graduated with an average score: 4.90',
    'Student name: Darsy, Grade: 1, Graduated with an average score: 5.15'
    ])
