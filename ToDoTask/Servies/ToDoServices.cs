using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoTask.Entities;
using ToDoTask.Exeptions;

namespace ToDoTask.Servies
{
    public class ToDoServices : IToDoServices
    {
        private readonly ToDoTaskDbContext _context;
        private readonly ILogger<ToDoServices> _logger;

        public ToDoServices(ToDoTaskDbContext context, ILogger<ToDoServices> logger)
        {
            _context = context;
            _logger = logger;
        }
        public ToDo GetById(int id)
        {
            var toDo = _context
                .ToDos
                .FirstOrDefault(t => t.Id == id);
            if (toDo is null) 
                throw new NotFoundException("ToDo not found");

            return toDo;
        }

        public IEnumerable<ToDo> GetAll()
        {
            var toDo = _context
                .ToDos
                .ToList();

            return (toDo);
        }

        public IEnumerable<ToDo> GetIncomingDays(int Days)
        {
            var toDo = _context
                .ToDos
                .Where(t => t.DateAndTimeOfExiry < DateTime.Now.AddDays(Days))
                .ToList();
            if (toDo is null)
                throw new NotFoundException("ToDo not found");

            return (toDo);
        }

        public int CreateToDo(ToDo toDo)
        {
            var result = new ToDo()
            {
                DateAndTimeOfExiry = toDo.DateAndTimeOfExiry,
                Title = toDo.Title,
                Description = toDo.Description,
                PercentComplete = toDo.PercentComplete,
                IsDone = false
            };
            _context.ToDos.Add(result);
            _context.SaveChanges();
            return result.Id;
        }

        public void Uptade(ToDo toDo, int id)
        {
            var result = _context
                .ToDos
                .FirstOrDefault(t => t.Id == id);
            if (toDo is null)
                throw new NotFoundException("ToDo not found");
            result.Title = toDo.Title;
            result.Description = toDo.Description;
            _context.SaveChanges();
        }

        public void SetPercentComplete(int percentComplete, int id)
        {
            var toDo = _context
                .ToDos
                .FirstOrDefault(t => t.Id == id);
            if (toDo is null)
                throw new NotFoundException("ToDo not found");
            toDo.PercentComplete += percentComplete;
            _context.SaveChanges();
        }

        public void DeleteToDo(int id)
        {
            _logger.LogError($"ToDo with id: {id} DELETE action invoked");

            var toDo = _context
                .ToDos
                .FirstOrDefault(t => t.Id == id);
            if (toDo is null)
                throw new NotFoundException("ToDo not found");
            _context.ToDos.Remove(toDo);
            _context.SaveChanges();
        }
        
        public void MarkDoneToDo(int id)
        {
            var toDo = _context
                .ToDos
                .FirstOrDefault(t => t.Id == id);
            if (toDo is null)
                throw new NotFoundException("ToDo not found");
            if (toDo.PercentComplete != 100)
            {
                throw new ForbiddenException("Canno't mark ToDo as done. Percent is no 100%");
            }
            toDo.IsDone = true;
            _context.SaveChanges();
        }
    }
}
