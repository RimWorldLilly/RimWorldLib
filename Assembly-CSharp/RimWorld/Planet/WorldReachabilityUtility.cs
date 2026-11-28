using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022DC RID: 8924
	public static class WorldReachabilityUtility
	{
		// Token: 0x0600DC23 RID: 56355 RVA: 0x00520151 File Offset: 0x0051E351
		public static bool CanReach(this Caravan c, PlanetTile tile)
		{
			return Find.WorldReachability.CanReach(c, tile);
		}
	}
}
