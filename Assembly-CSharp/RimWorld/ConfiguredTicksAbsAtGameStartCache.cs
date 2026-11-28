using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020015FC RID: 5628
	public class ConfiguredTicksAbsAtGameStartCache
	{
		// Token: 0x06008BC2 RID: 35778 RVA: 0x0032A5D4 File Offset: 0x003287D4
		public bool TryGetCachedValue(GameInitData initData, out int ticksAbs)
		{
			if (initData.startingTile == this.cachedForStartingTile && initData.startingSeason == this.cachedForStartingSeason)
			{
				ticksAbs = this.cachedTicks;
				return true;
			}
			ticksAbs = -1;
			return false;
		}

		// Token: 0x06008BC3 RID: 35779 RVA: 0x0032A605 File Offset: 0x00328805
		public void Cache(int ticksAbs, GameInitData initData)
		{
			this.cachedTicks = ticksAbs;
			this.cachedForStartingTile = initData.startingTile;
			this.cachedForStartingSeason = initData.startingSeason;
		}

		// Token: 0x040055B8 RID: 21944
		private int cachedTicks = -1;

		// Token: 0x040055B9 RID: 21945
		private PlanetTile cachedForStartingTile = PlanetTile.Invalid;

		// Token: 0x040055BA RID: 21946
		private Season cachedForStartingSeason;
	}
}
