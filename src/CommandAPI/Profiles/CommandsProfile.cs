namespace CommandAPI.Profiles
{
    using AutoMapper;
    using CommandAPI.Dtos;
    using CommandAPI.Models;

    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            // Source ➤ Target
            this.CreateMap<Command, CommandReadDto>();

            this.CreateMap<CommandCreateDto, Command>();

            this.CreateMap<CommandUpdateDto, Command>();

            this.CreateMap<Command, CommandUpdateDto>();
        }
    }
}