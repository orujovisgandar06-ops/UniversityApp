using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace UniversityApp
{
    public class Course
    {
        // первичный ключ курса 
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        // внешний ключ 
        // хранит ID того Учителя из таблицы Teachers который ведет этот курс
        public int TeacherId { get; set; }

        // объект учителя со всеми его полями 
        public Teacher Teacher { get; set; }

        // на этот курс может быть записано много студентов 
        public List<Student> Students { get; set; } = new List<Student>();
    }
}