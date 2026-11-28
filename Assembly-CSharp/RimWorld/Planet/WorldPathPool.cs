using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022DA RID: 8922
	public class WorldPathPool
	{
		// Token: 0x17002649 RID: 9801
		// (get) Token: 0x0600DC15 RID: 56341 RVA: 0x0051FCB5 File Offset: 0x0051DEB5
		public static WorldPath NotFoundPath { get; } = WorldPath.NewNotFound();

		// Token: 0x0600DC17 RID: 56343 RVA: 0x0051FCC8 File Offset: 0x0051DEC8
		public WorldPath GetEmptyWorldPath()
		{
			for (int i = 0; i < this.paths.Count; i++)
			{
				if (!this.paths[i].inUse)
				{
					this.paths[i].inUse = true;
					return this.paths[i];
				}
			}
			if (this.paths.Count > Find.WorldObjects.CaravansCount + 2 + (Find.WorldObjects.RoutePlannerWaypointsCount - 1))
			{
				Log.ErrorOnce("WorldPathPool leak: more paths than caravans. Force-recovering.", 664788);
				this.paths.Clear();
			}
			WorldPath worldPath = new WorldPath();
			this.paths.Add(worldPath);
			worldPath.inUse = true;
			return worldPath;
		}

		// Token: 0x04008D2F RID: 36143
		private readonly List<WorldPath> paths = new List<WorldPath>(64);
	}
}
