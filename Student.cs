using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityApp
{
    public class Student
    {
        // первичный ключ студента 
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Email {  get; set; }

        // у одного студента одна студенченская карта 
        public StudentCard StudentCard { get; set; }

        // один студент может учиться на множестве курсов 
        public List<Course> Courses { get; set; } = new List<Course>();

        // телефонный номер для миграции 
        public string? PhoneNumber { get; set; }
    }
}