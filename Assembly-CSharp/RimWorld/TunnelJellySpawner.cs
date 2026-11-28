using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B0F RID: 6927
	public class TunnelJellySpawner : TunnelHiveSpawner
	{
		// Token: 0x0600B114 RID: 45332 RVA: 0x003E6658 File Offset: 0x003E4858
		protected override void Spawn(Map map, IntVec3 loc)
		{
			if (this.jellyCount > 0)
			{
				int num = Rand.Range(2, 3);
				int num2 = this.jellyCount;
				for (int i = 0; i < num; i++)
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.InsectJelly, null);
					if (i < num - 1)
					{
						thing.stackCount = this.jellyCount / num;
						num2 -= thing.stackCount;
					}
					else
					{
						thing.stackCount = num2;
					}
					GenSpawn.Spawn(thing, CellFinder.RandomClosewalkCellNear(loc, map, 2, null), map, WipeMode.Vanish);
				}
				this.jellyCount = 0;
			}
		}

		// Token: 0x0600B115 RID: 45333 RVA: 0x003E66D4 File Offset: 0x003E48D4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.jellyCount, "jellyCount", 0, false);
		}

		// Token: 0x04006657 RID: 26199
		public int jellyCount;
	}
}
