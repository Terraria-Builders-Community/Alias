using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.Hooks;
using Auxiliary.Configuration;
using System.Text.RegularExpressions;

namespace Alias
{
    [ApiVersion(2, 1)]
    public class Alias : TerrariaPlugin
    {
        public override string Author
            => "TBC Developers";

        public override string Description
            => "Allows the ability to add command aliases on request.";

        public override string Name
            => "Alias";

        public override Version Version
            => new(1, 0);

        public Alias(Main game)
            : base(game)
        {
            Order = 2;
        }

        public override void Initialize()
        {
            Configuration<AliasSettings>.Load("Alias");

            GeneralHooks.ReloadEvent += (x) =>
            {
                Configuration<AliasSettings>.Load("Alias");
                x.Player.SendSuccessMessage("[Alias] Reloaded configuration.");
            };

            PlayerHooks.PlayerCommand += OnCommand;
        }

        private void OnCommand(PlayerCommandEventArgs args)
        {
            IEnumerable<string> Yield(string[] parameters)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    if (parameters[i] == "{+}")
                    {
                        if (args.Parameters.Count > i)
                            yield return args.Parameters[i];
                        else
                            yield return parameters[i];
                    }
                    else
                        yield return parameters[i];
                }
            }

            if (!Configuration<AliasSettings>.Settings.Commands.TryGetValue(args.CommandName, out var command))
                return;

            var parameters = command.Split(' ');
            var commands = Commands.TShockCommands.Concat(Commands.ChatCommands);

            if (!commands.Any(p => p.Name == parameters[0]))
            {
                args.Player.SendErrorMessage("Unknown command: '{0}'", parameters[0]);
                args.Handled = true;
                return;
            }

            var formatted = Yield(parameters);

            Commands.HandleCommand(args.Player, $"{args.CommandPrefix}{string.Join(' ', formatted)}");

            args.Handled = true;
        }
    }
}