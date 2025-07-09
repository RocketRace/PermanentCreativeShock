using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace PermanentCreativeShock
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class PermanentCreativeShock : Mod
	{

	}

	// Responsible for the Creative Shock functionality
	public class CreativeShockPlayer : ModPlayer
	{
		bool hasBeenChecked = false;
		// This hook is called every tick.
		public override void PreUpdateBuffs()
		{
			// The buff itself is only cosmetic.
			Player.AddBuff(BuffID.NoBuilding, 60);
			// The actual effect is a flag on the player.
			Player.noBuilding = true;
		}
	}

	// Computes the temple entrance check
	public class TempleCheckCommand : ModCommand
	{
		// Can be used by players in chat
		public override CommandType Type => CommandType.Chat;
		
		public override string Command => "templecheck";
		public override string Description => "Checks for the ability to enter the Jungle Temple via Shimmer";

		// Run this check once after world gen
		public override void Action(CommandCaller caller, string input, string[] args)
		{
			// Find the highest Lihzard Brick tile in the world,
			// and the highest Shimmer liquid in the world.
			int maxLihzahrdBrickY = Int32.MinValue;
			int maxShimmerY = Int32.MinValue;
			for (int x = 0; x < Main.maxTilesX; x++)
			{
				for (int y = 0; y < Main.maxTilesY; y++)
				{
					Tile tile = Main.tile[x, y];
					if (tile.HasTile && tile.TileType == TileID.LihzahrdBrick && y > maxLihzahrdBrickY)
					{
						maxLihzahrdBrickY = y;
					}
					if (tile.LiquidAmount > 0 && tile.LiquidType == LiquidID.Shimmer && y > maxShimmerY)
					{
						maxShimmerY = y;
					}
				}
			}
			// Most likely impossible if the lowest Shimmer is below the highest temple
			// There is some nuance here, since the Shimmer does not need to be at the very highest point,
			// but in practice only the ceiling of the Jungle Temple can be entered via Shimmer
			if (maxShimmerY < maxLihzahrdBrickY)
			{
				ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(
					"The Jungle Temple is above the Aether biome. " + maxLihzahrdBrickY.ToString() + " vs " + maxShimmerY.ToString()
				), Color.Red);
			}
			else
			{
				ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(
					"The Jungle Temple is below the Aether biome. " + maxLihzahrdBrickY.ToString() + " vs " + maxShimmerY.ToString()
				), Color.Green);
			}
		}
	}
}
