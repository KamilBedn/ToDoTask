using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoTask.Exeptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string massage) : base(massage)
        {

        }
    }
}
