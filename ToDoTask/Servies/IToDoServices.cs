using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoTask.Entities;

namespace ToDoTask.Servies
{
    public interface IToDoServices
    {
        ToDo GetById(int id);
        IEnumerable<ToDo> GetAll();
        IEnumerable<ToDo> GetIncomingDays(string query);
        int CreateToDo(ToDo toDo);
        void Uptade(ToDo toDo, int id);
        void SetPercentComplete(int percentComplete, int id);
        void DeleteToDo(int id);
        void MarkDoneToDo(int id);
    }
}
