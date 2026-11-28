using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200129C RID: 4764
	public class GenStep_BurntPatches : GenStep
	{
		// Token: 0x17001407 RID: 5127
		// (get) Token: 0x06007631 RID: 30257 RVA: 0x00299352 File Offset: 0x00297552
		public override int SeedPart
		{
			get
			{
				return 823478234;
			}
		}

		// Token: 0x06007632 RID: 30258 RVA: 0x0029935C File Offset: 0x0029755C
		public override void Generate(Map map, GenStepParams parms)
		{
			int randomInRange = GenStep_BurntPatches.PatchCountRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				this.GeneratePatch(map);
			}
		}

		// Token: 0x06007633 RID: 30259 RVA: 0x0029938C File Offset: 0x0029758C
		private void GeneratePatch(Map map)
		{
			IntVec3 center;
			if (!RCellFinder.TryFindRandomCellNearWith(map.Center, (IntVec3 c) => c.Standable(map), map, out center, 30, 2147483647))
			{
				return;
			}
			foreach (IntVec3 c2 in GridShapeMaker.IrregularLump(center, map, GenStep_BurntPatches.PatchSizeRange.RandomInRange, null))
			{
				List<Thing> thingList = c2.GetThingList(map);
				for (int i = thingList.Count - 1; i >= 0; i--)
				{
					Thing thing = thingList[i];
					if (thing.def.IsPlant)
					{
						thing.Kill(new DamageInfo?(new DamageInfo(DamageDefOf.Flame, 9999f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false)), null);
					}
				}
				map.snowGrid.SetDepth(c2, 0f);
				FilthMaker.TryMakeFilth(c2, map, ThingDefOf.Filth_Ash, 1, FilthSourceFlags.None, true);
			}
		}

		// Token: 0x04004A0C RID: 18956
		private static readonly IntRange PatchCountRange = new IntRange(0, 2);

		// Token: 0x04004A0D RID: 18957
		private static readonly IntRange PatchSizeRange = new IntRange(50, 200);
	}
}
