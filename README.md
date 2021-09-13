# StudentRS
 StudentRS is an application written in C#, which provides a class record book. Teachers using the application are able to add, remove or update students and their grades/absences.
 All students are stored in a database, each class being a table in the database.
 
 ![Main Menu](https://github.com/EmanuelPutura/StudentRS/blob/main/img/main_menu.png)
 
 ## Motivation and Future Improvements
 I created the application during highschool, as a helpful tool for my mother, who is a teacher. Hence, unfortunately, the application's UI is in Romanian. It is also worth mentioning that, being created a few years ago, during highschool, the application lacks several key concepts which I would use today, if I were to start refactoring the code (e.g., using a layered arhitecture, separating the user interface and the functionalities, better exception handling).
 
 
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

   ![Check Class Menu](https://github.com/EmanuelPutura/StudentRS/blob/main/img/check_class.png)

   - see all the students in a given class, together with their grades and absences
   - select a class to check its record book (default available classes are XD and XIC)
2. Add a student to the database
   
   ![Add Menu](https://github.com/EmanuelPutura/StudentRS/blob/main/img/add_student.png)
   
   - select the student's class, then introduce his last name and his first name
4. Delete a student from the database
   
   ![Delete Menu](https://github.com/EmanuelPutura/StudentRS/blob/main/img/delete_student.png)
   
   - select the student's class, then introduce his last name and his first name
6. Add grades/absences
   
   ![Add Grades/Absences Menu](https://github.com/EmanuelPutura/StudentRS/blob/main/img/add_grades_absences.png)
   
   - select the student's class, then introduce his last name and his first name
   - add grades and/or absences for the student (e.g., add grade 9.5 and absence 10/09 - absence on the 10th of September)

