namespace CommandAPI.Controllers
{
    using System.Collections.Generic;
    using AutoMapper;
    using CommandAPI.Data;
    using CommandAPI.Dtos;
    using CommandAPI.Models;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        // Randoom change 
        private readonly ICommandAPIRepo repository;
        private readonly IMapper mapper;

        public CommandsController(ICommandAPIRepo repository, IMapper mapper)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
        {
            var commandItems = this.repository.GetAllCommands();
            return this.Ok(this.mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }

        [HttpGet("{id}", Name = "GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            var commandItem = this.repository.GetCommandById(id);

            if (commandItem == null)
            {
                return this.NotFound();
            }

            return this.Ok(this.mapper.Map<CommandReadDto>(commandItem));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto)
        {
            var commandModel = this.mapper.Map<Command>(commandCreateDto);

            this.repository.CreateCommand(commandModel);
            this.repository.SaveChanges();

            var commandReadDto = this.mapper.Map<CommandReadDto>(commandModel);

            return this.CreatedAtRoute(
                nameof(this.GetCommandById),
                new { Id = commandReadDto.Id },
                commandReadDto);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, CommandUpdateDto commandUpdateDto)
        {
            var commandModelFromRepo = this.repository.GetCommandById(id);

            if (commandModelFromRepo == null)
            {
                return this.NotFound();
            }

            this.mapper.Map(commandUpdateDto, commandModelFromRepo);
            this.repository.UpdateCommand(commandModelFromRepo);
            this.repository.SaveChanges();

            return this.NoContent();
        }

        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc)
        {
            var commandModelFromRepo = this.repository.GetCommandById(id);

            if (commandModelFromRepo == null)
            {
                return this.NotFound();
            }

            var commandToPatch = this.mapper.Map<CommandUpdateDto>(commandModelFromRepo);
            patchDoc.ApplyTo(commandToPatch, this.ModelState);

            if (!this.TryValidateModel(commandToPatch))
            {
                return this.ValidationProblem(this.ModelState);
            }

            this.mapper.Map(commandToPatch, commandModelFromRepo);
            this.repository.UpdateCommand(commandModelFromRepo);
            this.repository.SaveChanges();

            return this.NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id)
        {
            var commandModelFromRepo = this.repository.GetCommandById(id);

            if (commandModelFromRepo == null)
            {
                return this.NotFound();
            }

            this.repository.DeleteCommand(commandModelFromRepo);
            this.repository.SaveChanges();

            return this.NoContent();
        }
    }
}