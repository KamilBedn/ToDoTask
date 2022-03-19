using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoTask.Entities
{
    public class ToDo
    {
        public int Id { get; set; }
        public DateTime DateAndTimeOfExiry { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int PercentComplete { get; set; }
        public bool IsDone { get; set; }
    }
}
