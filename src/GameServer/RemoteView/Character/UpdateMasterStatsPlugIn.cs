﻿// <copyright file="UpdateMasterStatsPlugIn.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameServer.RemoteView.Character
{
    using System.Runtime.InteropServices;
    using MUnique.OpenMU.GameLogic.Attributes;
    using MUnique.OpenMU.GameLogic.Views.Character;
    using MUnique.OpenMU.Network.Packets.ServerToClient;
    using MUnique.OpenMU.PlugIns;

    /// <summary>
    /// The default implementation of the <see cref="IUpdateMasterStatsPlugIn"/> which is forwarding everything to the game client with specific data packets.
    /// </summary>
    [PlugIn("UpdateMasterStatsPlugIn", "The default implementation of the IUpdateMasterStatsPlugIn which is forwarding everything to the game client with specific data packets.")]
    [Guid("41b27ec2-5bc6-4acf-b395-ddf9e81a3611")]
    public class UpdateMasterStatsPlugIn : IUpdateMasterStatsPlugIn
    {
        private readonly RemotePlayer player;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateMasterStatsPlugIn"/> class.
        /// </summary>
        /// <param name="player">The player.</param>
        public UpdateMasterStatsPlugIn(RemotePlayer player) => this.player = player;

        /// <inheritdoc />
        public void SendMasterStats()
        {
            var character = this.player.SelectedCharacter;
            if (character is null || this.player.Attributes is null)
            {
                return;
            }

            this.player.Connection?.SendMasterStatsUpdate(
                (ushort)this.player.Attributes[Stats.MasterLevel],
                (ulong)character.MasterExperience,
                (ulong)this.player.GameServerContext.Configuration.MasterExperienceTable![(int)this.player.Attributes[Stats.MasterLevel] + 1],
                (ushort)character.MasterLevelUpPoints,
                (ushort)this.player.Attributes[Stats.MaximumHealth],
                (ushort)this.player.Attributes[Stats.MaximumMana],
                (ushort)this.player.Attributes[Stats.MaximumShield],
                (ushort)this.player.Attributes[Stats.MaximumAbility]);

            this.player.ViewPlugIns.GetPlugIn<IUpdateMasterSkillsPlugIn>()?.UpdateMasterSkills();
        }
    }
}