using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020012D7 RID: 4823
	public class GenStep_PlaceCaveExit : GenStep
	{
		// Token: 0x17001455 RID: 5205
		// (get) Token: 0x060077A7 RID: 30631 RVA: 0x002A44DE File Offset: 0x002A26DE
		public override int SeedPart
		{
			get
			{
				return 12412314;
			}
		}

		// Token: 0x060077A8 RID: 30632 RVA: 0x002A44E8 File Offset: 0x002A26E8
		public override void Generate(Map map, GenStepParams parms)
		{
			IntVec3 intVec;
			CellFinder.TryFindRandomCell(map, (IntVec3 cell) => cell.Standable(map) && (float)cell.DistanceToEdge(map) > 5.5f, out intVec);
			foreach (IntVec3 c in GenRadial.RadialCellsAround(intVec, 4.5f, true))
			{
				foreach (Thing thing in from t in c.GetThingList(map).ToList<Thing>()
				where t.def.destroyable
				select t)
				{
					thing.Destroy(DestroyMode.Vanish);
				}
			}
			GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.CaveExit, null), intVec, map, WipeMode.Vanish);
			MapGenerator.PlayerStartSpot = intVec;
		}

		// Token: 0x04004B1C RID: 19228
		public const float ClearRadius = 4.5f;
	}
}
