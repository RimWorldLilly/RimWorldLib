using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200233A RID: 9018
	public static class GenWorldClosest
	{
		// Token: 0x0600DED3 RID: 57043 RVA: 0x0052E464 File Offset: 0x0052C664
		public static bool TryFindClosestTile(PlanetTile rootTile, Predicate<PlanetTile> predicate, out PlanetTile foundTile, int maxTilesToScan = 2147483647, bool canSearchThroughImpassable = true)
		{
			PlanetTile foundTileLocal = PlanetTile.Invalid;
			rootTile.Layer.Filler.FloodFill(rootTile, (PlanetTile x) => canSearchThroughImpassable || !Find.World.Impassable(x), delegate(PlanetTile t)
			{
				bool flag = predicate(t);
				if (flag)
				{
					foundTileLocal = t;
				}
				return flag;
			}, maxTilesToScan, null);
			foundTile = foundTileLocal;
			return foundTileLocal.Valid;
		}

		// Token: 0x0600DED4 RID: 57044 RVA: 0x0052E4D4 File Offset: 0x0052C6D4
		public static bool TryFindClosestPassableTile(PlanetTile rootTile, out PlanetTile foundTile)
		{
			return GenWorldClosest.TryFindClosestTile(rootTile, (PlanetTile x) => !Find.World.Impassable(x), out foundTile, int.MaxValue, true);
		}
	}
}
