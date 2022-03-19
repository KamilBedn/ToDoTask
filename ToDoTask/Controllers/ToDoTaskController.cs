using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoTask.Entities;
using ToDoTask.Exeptions;
using ToDoTask.Servies;

namespace ToDoTask.Controllers
{
    [ApiController]
    [Route("todotask")]
    public class ToDoTaskController : ControllerBase
    {
        private readonly IToDoServices _services;

        public ToDoTaskController(IToDoServices services)
        {
            _services = services;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ToDo>> GetAll()
        {
            var toDo = _services.GetAll();
            return Ok(toDo);
        }

        [HttpGet("{id}")]
        public ActionResult<ToDo> GetById([FromRoute]int id)
        {
            var toDo = _services.GetById(id);
            if (toDo is null)
                throw new NotFoundException("ToDo not found");
            return Ok(toDo);
        }

        [HttpGet("incomingdays/{Days}")]
        public ActionResult<IEnumerable<ToDo>> GetIncomingDays([FromRoute]int Days)
        {
            var toDo = _services.GetIncomingDays(Days);
            return Ok(toDo);
        }

        [HttpPost]
        public ActionResult CreatToDoTask([FromBody]ToDo toDo)
        {
            var id = _services.CreateToDo(toDo);
            return Created($"todotask/{id}", null);
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody]ToDo toDo, [FromRoute]int id)
        {
            _services.Uptade(toDo, id);
            return Ok();
        }

        [HttpPut("setcomplete/{id}")]
        public ActionResult SetPercentComplete([FromBody] int percentComplete, [FromRoute] int id)
        {
            _services.SetPercentComplete(percentComplete, id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteToDo([FromRoute]int id)
        {
            _services.DeleteToDo(id);
            return NoContent();
        }

        [HttpPut("markdone/{id}")]
        public ActionResult MarkDoneToDo([FromRoute]int id)
        {
            _services.MarkDoneToDo(id);
            return Ok();    
        }
    }
}
