using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CommandAPI.Data;
using CommandAPI.Models;
using AutoMapper;
using CommandAPI.Dtos;

namespace CommandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
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
            return Ok(this.mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }

        [HttpGet("{id}", Name="GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            var commandItem = this.repository.GetCommandById(id);

            if (commandItem == null)
            {
                return NotFound();
            }

            return Ok(this.mapper.Map<CommandReadDto>(commandItem));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto)
        {
            var commandModel = this.mapper.Map<Command>(commandCreateDto);

            this.repository.CreateCommand(commandModel);
            this.repository.SaveChanges();

            var commandReadDto = this.mapper.Map<CommandReadDto>(commandModel);

            return CreatedAtRoute(nameof(GetCommandById),
                new { Id = commandReadDto.Id }, commandReadDto);
        }
    }
}