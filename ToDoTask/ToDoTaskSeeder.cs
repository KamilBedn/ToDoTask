using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoTask.Entities;

namespace ToDoTask
{
    public class ToDoTaskSeeder
    {
        private readonly ToDoTaskDbContext _context;

        public ToDoTaskSeeder(ToDoTaskDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if(_context.Database.CanConnect())
            {
                if(!_context.ToDos.Any())
                {
                    var toDos = GetToDos();
                    _context.ToDos.AddRange(toDos);
                    _context.SaveChanges();
                }
            }
        }

        public IEnumerable<ToDo> GetToDos()
        {
            var toDos = new List<ToDo>()
            {
                new ToDo()
                {
                    DateAndTimeOfExiry = DateTime.Parse("22.03.2022 16:00:00"),
                    Title = "Creat web appliction",
                    Description = "Application about book collection",
                    PercentComplete = 0,
                    IsDone = false,
                },
                new ToDo()
                {
                    DateAndTimeOfExiry = DateTime.Parse("25.03.2022 16:00:00"),
                    Title = "Test application",
                    Description = null,
                    PercentComplete = 0,
                    IsDone = false,
                }
            };
            return toDos;
        }
    }
}
