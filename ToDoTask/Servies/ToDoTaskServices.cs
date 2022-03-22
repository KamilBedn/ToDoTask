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
    public class ToDoTaskServices : IToDoTaskServices
    {
        #region(propety)
        private readonly ToDoTaskDbContext _context;
        private readonly ILogger<ToDoTaskServices> _logger;
        private string[] incomingToDo = new string[] {"today", "next day","current week"};
        #endregion
        #region(constructor)
        public ToDoTaskServices(ToDoTaskDbContext context, ILogger<ToDoTaskServices> logger)
        {
            _context = context;
            _logger = logger;
        }
        #endregion
        #region(get todo from database)
        private ToDo GetToDoTask(int id)
        {
            ToDo result = _context
                .ToDos
                .FirstOrDefault(t => t.Id == id);
            return result;
        }
        #endregion
        #region(action services)
        public ToDo GetById(int id)
        {
            ToDo toDo = GetToDoTask(id);
            if (toDo is null) 
                throw new NotFoundException("ToDo not found");

            return toDo;
        }

        public IEnumerable<ToDo> GetAll()
        {
            IEnumerable<ToDo> toDo = _context
                .ToDos
                .ToList();

            return (toDo);
        }

        public IEnumerable<ToDo> GetIncomingDays(string query)
        {
            if(query.ToLower() == incomingToDo[0])
            {
                IEnumerable<ToDo> result = _context.ToDos
                    .Where(d => d.DateAndTimeOfExiry < DateTime.Today.AddDays(1))
                    .ToList();
                return result;
            }
            else if(query.ToLower() == incomingToDo[1])
            {
                IEnumerable<ToDo> result = _context.ToDos
                    .Where(d => d.DateAndTimeOfExiry < DateTime.Today.AddDays(2))
                    .ToList();
                return result;
            }
            else if(query.ToLower() == incomingToDo[2])
            {
                IEnumerable<ToDo> result = _context.ToDos
                    .Where(d => d.DateAndTimeOfExiry < DateTime.Today.AddDays(7))
                    .ToList();
                return result;
            }
            else
            {
                throw new BadRequestExcepion($"You can show incoming ToDo for {string.Join(",", incomingToDo)}");
            }

        }

        public int CreateToDo(ToDo toDo)
        {
            ToDo result = new ToDo()
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
            ToDo result = GetToDoTask(id);
            if (toDo is null)
                throw new NotFoundException("ToDo not found");
            result.Title = toDo.Title;
            result.Description = toDo.Description;
            _context.SaveChanges();
        }

        public void SetPercentComplete(int percentComplete, int id)
        {
            ToDo toDo = GetToDoTask(id);
            if (toDo is null)
                throw new NotFoundException("ToDo not found");
            toDo.PercentComplete += percentComplete;
            _context.SaveChanges();
        }

        public void DeleteToDo(int id)
        {
            _logger.LogError($"ToDo with id: {id} DELETE action invoked");

            ToDo toDo = GetToDoTask(id);
            if (toDo is null)
                throw new NotFoundException("ToDo not found");
            _context.ToDos.Remove(toDo);
            _context.SaveChanges();
        }
        
        public void MarkDoneToDo(int id)
        {
            ToDo toDo = GetToDoTask(id);
            if (toDo is null)
                throw new NotFoundException("ToDo not found");
            if (toDo.PercentComplete != 100)
            {
                throw new ForbiddenException("Canno't mark ToDo as done. Percent is no 100%");
            }
            toDo.IsDone = true;
            _context.SaveChanges();
        }
        #endregion
    }
}
