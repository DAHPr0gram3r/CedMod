﻿using System;
using CedMod.QuerySystem.WS;
using CommandSystem;
using Exiled.API.Features;

namespace CedMod.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class CmSyncCommandCommand : ICommand
    {
        public string Command { get; } = "cmsync";

        public string[] Aliases { get; } = new string[]
        {
        };

        public string Description { get; } = "syncs roles for cedmod panel";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender,
            out string response)
        {
            if (sender is CmSender)
            {
                if (!sender.CheckPermission(PlayerPermissions.SetGroup))
                {
                    response = "No permission";
                    return false;
                }
                if (ServerStatic.PermissionsHandler._members.ContainsKey(Player.Get(int.Parse(arguments.At(0))).UserId))
                {
                    response = "Already synced";
                    return false;
                }
                Player.Get(int.Parse(arguments.At(0))).ReferenceHub.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler()._groups[arguments.At(1)], false);
                ServerStatic.GetPermissionsHandler()._members[Player.Get(int.Parse(arguments.At(0))).UserId] = arguments.At(1);
                CedMod.QuerySystem.CommandHandler.synced.Add(Player.Get(int.Parse(arguments.At(0))).UserId);
            }

            response = "";
            return true;
        }
    }
}