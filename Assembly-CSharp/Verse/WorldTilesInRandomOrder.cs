using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000D0 RID: 208
	public class WorldTilesInRandomOrder
	{
		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060007EA RID: 2026 RVA: 0x0002E320 File Offset: 0x0002C520
		public List<int> Tiles
		{
			get
			{
				if (this.randomizedTiles == null)
				{
					this.randomizedTiles = new List<int>();
					for (int i = 0; i < Find.WorldGrid.TilesCount; i++)
					{
						this.randomizedTiles.Add(i);
					}
					Rand.PushState();
					Rand.Seed = Find.World.info.Seed;
					this.randomizedTiles.Shuffle<int>();
					Rand.PopState();
				}
				return this.randomizedTiles;
			}
		}

		// Token: 0x0400039C RID: 924
		private List<int> randomizedTiles;
	}
}
