using System.Text;
using Microsoft.EntityFrameworkCore;
using UniversityApp;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

using var appDbContext = new AppDbContext();

//appDbContext.Database.EnsureDeleted(); // Сначала удаляем старую базу со всеми изменениями
//appDbContext.Database.EnsureCreated(); // Создаем чистую свежую базу заново

// создание учителей 
var teacherJohn = new Teacher { Name = "John", Email = "john@univ.com" };
var teacherAli = new Teacher { Name = "Ali", Email = "ali@univ.com" };
var teacherElena = new Teacher { Name = "Elena", Email = "elena@univ.com" };

// создание курсов и связание их с нашими учителями 
var csharpCourse = new Course { Name = "C#", Description = "Базовый курс C#", Teacher = teacherJohn };
var efCoreCourse = new Course { Name = "EF Core", Description = "Работа с БД", Teacher = teacherJohn };

var sqlCourse = new Course { Name = "SQL", Description = "Базы данных SQL", Teacher = teacherAli };
var algoCourse = new Course { Name = "Algorithms", Description = "Алгоритмы", Teacher = teacherAli };

var pythonCourse = new Course { Name = "Python", Description = "Курс Python", Teacher = teacherElena };

//создание студентов с билетами и курсами 
var student1 = new Student
{
    Name = "Ali",
    Age = 20,
    Email = "ali@mail.com",
    StudentCard = new StudentCard { CardNumber = "SC001", IssueDate = DateTime.Now },
    Courses = new List<Course> { csharpCourse, sqlCourse, efCoreCourse } // Записан на 3 курса!
};

var student2 = new Student
{
    Name = "Vali",
    Age = 22,
    Email = "vali@mail.com",
    StudentCard = new StudentCard { CardNumber = "SC002", IssueDate = DateTime.Now },
    Courses = new List<Course> { csharpCourse, algoCourse } // Записан на 2 курса!
};

var student3 = new Student
{
    Name = "Murad",
    Age = 21,
    Email = "murad@mail.com",
    StudentCard = new StudentCard { CardNumber = "SC003", IssueDate = DateTime.Now },
    Courses = new List<Course> { efCoreCourse, pythonCourse } // Записан на EF Core и Python
};

var student4 = new Student
{
    Name = "Zara",
    Age = 19,
    Email = "zara@mail.com",
    StudentCard = new StudentCard { CardNumber = "SC004", IssueDate = DateTime.Now },
    Courses = new List<Course> { sqlCourse, pythonCourse, algoCourse } // Записан на SQL, Python, Algorithms
};

var student5 = new Student
{
    Name = "David",
    Age = 23,
    Email = "david@mail.com",
    StudentCard = new StudentCard { CardNumber = "SC005", IssueDate = DateTime.Now },
    Courses = new List<Course> { csharpCourse, efCoreCourse } // Записан на C# и EF Core
};

// сохранение студентов в базу данных и делаем обязательно SaveChanges();
using var context = new AppDbContext();
context.Database.EnsureCreated(); // Создаем базу, если её нет

if (!context.Students.Any())
{
    context.Students.AddRange(student1, student2, student3, student4, student5); // Добавляем студентов
    context.SaveChanges(); // EF Core сам свяжет преподавателей, курсы, билеты и таблицы!
}

var studentsWithCards = context.Students
    .Include(s => s.StudentCard)
    .ToList();

foreach (var s in studentsWithCards)
{
    // Если StudentCard не null — выведет CardNumber. Если null — выведет "Нет билета"
    string cardNumber = s.StudentCard?.CardNumber ?? "Нет билета";
    Console.WriteLine($"{s.Name} — Card: {cardNumber}");
}

var coursesWithTeachers = context.Courses
    .Include(c => c.Teacher)
    .ToList();

foreach (var c in coursesWithTeachers)
{
    Console.WriteLine($"{c.Name} — Teacher: {c.Teacher.Name}");
}

var studentsWithCourses = context.Students
    .Include(s => s.Courses)
    .ToList();

foreach (var s in studentsWithCourses)
{
    Console.WriteLine($"{s.Name}:");
    foreach (var c in s.Courses)
    {
        Console.WriteLine($"    - {c.Name}");
    }
}

// Создаем объект (он живет только в оперативной памяти C#)
var testStudent = new Student
{
    Name = "Test Student",
    Age = 20,
    Email = "test@test.com"
};

Console.WriteLine($"Состояние до Add: {appDbContext.Entry(testStudent).State}"); 
// состояние объекта до добавления должно быть Detached 

appDbContext.Students.Add(testStudent); // добавление студента в DbContext

Console.WriteLine($"Состояние после Add: {appDbContext.Entry(testStudent).State}");
// состояние после добавления должно быть Added 

appDbContext.SaveChanges(); // обязатель сохраняем в sql

Console.WriteLine($"Состояние после SaveChanges: {appDbContext.Entry(testStudent).State}");
// состояние после сохранение в sql должно быть Unchanged

// берём самого первого суещствующего студента из базы 
var studentToUpdate = appDbContext.Students.First();

// состояние после загрузки из базы является Unchanged
Console.WriteLine($"1. Состояние после загрузки из базы: {appDbContext.Entry(studentToUpdate).State}");

// меняем имя в оперативной памяти C#
studentToUpdate.Name = "Александр";

// состояние после изменения свойства являтся Modified
Console.WriteLine($"2. Состояние после изменения Name: {appDbContext.Entry(studentToUpdate).State}");

// cохраняем в SQL должен выполнится UPDATE
appDbContext.SaveChanges();

// состояние после сохранения является Unchanged
Console.WriteLine($"3. Состояние после SaveChanges: {appDbContext.Entry(studentToUpdate).State}");

// сортируем по Id и берем последнего студента
var studentToDelete = appDbContext.Students.OrderBy(x => x.Id).Last();

// далее удаляем студента 
appDbContext.Students.Remove(studentToDelete);

// состоянием до SaveChanges(); является Deleted
Console.WriteLine($"Состояние после Remove: {appDbContext.Entry(studentToDelete).State}");


// cохраняем выполнится DELETE FROM Students 
appDbContext.SaveChanges();

// состояние после SaveChanges(); является Detached
Console.WriteLine($"Состояние после SaveChanges: {appDbContext.Entry(studentToDelete).State}");

// объект был удален из базы данных 

// берём первого студента без поиска по имени:
var studentToEdit = appDbContext.Students.First();
studentToEdit.Name = "Александр (Updated)";

// учителя тоже берем первого попавшегося:
var teacherToEdit = appDbContext.Teachers.First();
teacherToEdit.Name = "John Doe";

// добавляем новый курс 
var newCourse = new Course
{
    Name = "ASP.NET Core",
    Description = "Web development",
    Teacher = teacherToEdit
};
appDbContext.Courses.Add(newCourse);

// выводим список всех объектов за которыми сейчас следит EF Core
Console.WriteLine("\n=== CHANGE TRACKER ENTRIES ===");
foreach (var entry in appDbContext.ChangeTracker.Entries())
{
    // entry.Entity.GetType().Name — название класса (Student, Teacher, Course)
    // entry.State — его текущее состояние (Modified, Added, Unchanged)
    Console.WriteLine($"{entry.Entity.GetType().Name} - {entry.State}");
}

// сохраняем все изменения 
appDbContext.SaveChanges();

// Делаем глубокий запрос с ThenInclude
var teachersChain = appDbContext.Teachers
    .Include(t => t.Courses)
        .ThenInclude(c => c.Students)
    .ToList();

// Красиво выводим древовидные данные в 3 вложенных циклах foreach:
Console.WriteLine("\n=== ДАННЫЕ ПО ЦЕПОЧКЕ (TEACHER -> COURSES -> STUDENTS) ===");

foreach (var teacher in teachersChain)
{
    Console.WriteLine($"Teacher: {teacher.Name}\n");

    foreach (var course in teacher.Courses)
    {
        Console.WriteLine($"  Course: {course.Name}");
        Console.WriteLine("  Students:");

        if (course.Students.Count == 0)
        {
            Console.WriteLine("    (Нет записанных студентов)");
        }
        else
        {
            foreach (var student in course.Students)
            {
                Console.WriteLine($"    - {student.Name}");
            }
        }
        Console.WriteLine(); // Пустая строчка для красоты
    }
    Console.WriteLine(new string('-', 50));
}