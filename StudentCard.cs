using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityApp
{
    public class StudentCard
    {
        // первичный ключ студ билета 
        public int Id { get; set; }

        public string CardNumber { get; set; }

        public DateTime IssueDate { get; set; }


        // внешний ключ 
        // хранит ID того Студента из таблицы Students которому принадлежит билет
        public int StudentId { get; set; }

        // у одной студ карты может быть только один владелец 
        public Student Student { get; set; }
    }
}
