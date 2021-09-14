# StudentRS
StudentRS is an application written in C#, which provides a class record book. Teachers using the application are able to add, remove or update students and their  grades/absences.
The application connects to a SQL server database. All students are stored in a database, each class being a table in the database. 
 
 <!-- ![Main Menu](https://github.com/EmanuelPutura/StudentRS/blob/main/img/main_menu.png) -->
 <p align="center"> <img src="https://github.com/EmanuelPutura/StudentRS/blob/main/img/main_menu.png" height="400"/> </p>
 
 
 ## Motivation and Future Improvements
I created the application during highschool, as a helpful tool for my mother, who is a teacher. Hence, unfortunately, the application's UI is in Romanian. It is also worth mentioning that, being created a few years ago, during highschool, the application lacks several key concepts which I would use today, if I were to start refactoring the code
(e.g., using a layered arhitecture, separating the user interface and the functionalities, better exception handling).
 
 
 ## Setup
 1. Clone the repo:
    ```sh
    $ git clone https://github.com/EmanuelPutura/StudentRS
    ```
 2. From the project's location:
    ```sh
    $ cd ./StudentRS/bin/Release/
    $ StudentRS.exe
    ```
    
## Features
1. Check the class record book
   - see all the students in a given class, together with their grades and absences
   - select a class to check its record book (default available classes are XD and XIC)
   
   <!-- ![Check Class Menu](https://github.com/EmanuelPutura/StudentRS/blob/main/img/check_class.png) -->
   <p align="center"> <img src="https://github.com/EmanuelPutura/StudentRS/blob/main/img/check_class.png" height="400"/> </p>
   
   
2. Add a student to the database
   - select the student's class, then introduce his last name and his first name

   <!-- ![Add Menu](https://github.com/EmanuelPutura/StudentRS/blob/main/img/add_student.png) -->
   <p align="center"> <img src="https://github.com/EmanuelPutura/StudentRS/blob/main/img/add_student.png" height="400"/> </p>
   
4. Delete a student from the database
   - select the student's class, then introduce his last name and his first name

   <!-- ![Delete Menu](https://github.com/EmanuelPutura/StudentRS/blob/main/img/delete_student.png) -->
   <p align="center"> <img src="https://github.com/EmanuelPutura/StudentRS/blob/main/img/delete_student.png" height="400"/> </p>

6. Add grades/absences
   - select the student's class, then introduce his last name and his first name
   - add grades and/or absences for the student (e.g., add grade 9.5 and absence 10/09 - absence on the 10th of September)

   <!-- ![Add Grades/Absences Menu](https://github.com/EmanuelPutura/StudentRS/blob/main/img/add_grades_absences.png) -->
   <p align="center"> <img src="https://github.com/EmanuelPutura/StudentRS/blob/main/img/add_grades_absences.png" height="400"/> </p>
   
