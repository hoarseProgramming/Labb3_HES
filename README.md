# Labb3_HES

## Quiz App - "hoarseQuizzerer" overview:

This is a quiz app made as an final assignment in the course "programming with C#".

The application is made with WPF following the MVVM architectural pattern.

## Features:

This is an application that lets you create and play quizzes.

### Configuration mode

Creation and configuration of question packs with possibility to choose: 

- Name
- Difficulty
- Time limit (for playing mode)

And the ability to:
- Add/Remove questions.
- Import questions from Open Trivia Database.

### Playing mode

Play your quiz!

You get a set number of seconds to answer each question (set in pack options).
Every question has a correct answer and three incorrect answers.

Get direct feedback if your answer is right or wrong (and which one was the right one.)

After you've answered all of the questions in the question you get a result.

### Saving and loading

The application creates a directory (User/AppData/Local/Labb3_HES) on it's first startup 
where it saves your packs.

The application automatically saves your pack every 30 seconds or upon closing.

### Enjoy!

/ hoarseProgramming


