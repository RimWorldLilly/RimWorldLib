using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012B4 RID: 4788
	public class GenStep_PitBurrows : GenStep
	{
		// Token: 0x17001433 RID: 5171
		// (get) Token: 0x060076CF RID: 30415 RVA: 0x0029DD09 File Offset: 0x0029BF09
		public override int SeedPart
		{
			get
			{
				return 512346123;
			}
		}

		// Token: 0x060076D0 RID: 30416 RVA: 0x0029DD10 File Offset: 0x0029BF10
		public override void Generate(Map map, GenStepParams parms)
		{
			int randomInRange = GenStep_PitBurrows.NumBurrowsRange.RandomInRange;
			Predicate<IntVec3> <>9__0;
			for (int i = 0; i < randomInRange; i++)
			{
				IntVec3 center = map.Center;
				Predicate<IntVec3> validator;
				if ((validator = <>9__0) == null)
				{
					validator = (<>9__0 = ((IntVec3 c) => GenSpawn.CanSpawnAt(ThingDefOf.PitBurrow, c, map, null, true)));
				}
				IntVec3 loc;
				if (RCellFinder.TryFindRandomCellNearWith(center, validator, map, out loc, 30, 2147483647))
				{
					GenSpawn.Spawn(ThingDefOf.PitBurrow, loc, map, WipeMode.Vanish).SetFaction(Faction.OfEntities, null);
				}
			}
		}

		// Token: 0x060076D1 RID: 30417 RVA: 0x0029DDA4 File Offset: 0x0029BFA4
		private bool Validator(CellRect rect, Map map)
		{
			foreach (IntVec3 c in rect.Cells)
			{
				if (!c.Standable(map))
				{
					return false;
				}
				if (!c.GetAffordances(map).Contains(ThingDefOf.PitBurrow.terrainAffordanceNeeded))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04004A97 RID: 19095
		private static readonly IntRange NumBurrowsRange = new IntRange(2, 3);
	}
}
