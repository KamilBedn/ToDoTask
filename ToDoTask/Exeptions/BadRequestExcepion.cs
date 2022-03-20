using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoTask.Exeptions
{
    public class BadRequestExcepion : Exception
    {
        public BadRequestExcepion(string massage) : base(massage)
        {

        }
    }
}
