namespace CommandAPI.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CommandAPI.Models;

    public class SqlCommandAPIRepo : ICommandAPIRepo
    {
        private readonly CommandContext context;

        public SqlCommandAPIRepo(CommandContext context)
        {
            this.context = context;
        }

        public void CreateCommand(Command cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }

            this.context.CommandItems.Add(cmd);
        }

        public void DeleteCommand(Command cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }

            this.context.CommandItems.Remove(cmd);
        }

        public IEnumerable<Command> GetAllCommands()
        {
            return this.context.CommandItems.ToList();
        }

        public Command GetCommandById(int id)
        {
            return this.context.CommandItems.FirstOrDefault(p => p.Id == id);
        }

        public bool SaveChanges()
        {
            return this.context.SaveChanges() >= 0;
        }

        public void UpdateCommand(Command cmd)
        {
            // We don't need to do anything here
        }
    }
}