using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02002108 RID: 8456
	public class SymbolResolver_OutdoorsCampfire : SymbolResolver
	{
		// Token: 0x0600D458 RID: 54360 RVA: 0x004E7CB8 File Offset: 0x004E5EB8
		public override bool CanResolve(ResolveParams rp)
		{
			IntVec3 intVec;
			return base.CanResolve(rp) && this.TryFindSpawnCell(rp.rect, out intVec);
		}

		// Token: 0x0600D459 RID: 54361 RVA: 0x004E7CE4 File Offset: 0x004E5EE4
		public override void Resolve(ResolveParams rp)
		{
			IntVec3 loc;
			if (!this.TryFindSpawnCell(rp.rect, out loc))
			{
				return;
			}
			Thing thing = ThingMaker.MakeThing(ThingDefOf.Campfire, null);
			thing.SetFaction(rp.faction, null);
			GenSpawn.Spawn(thing, loc, BaseGen.globalSettings.map, WipeMode.Vanish);
		}

		// Token: 0x0600D45A RID: 54362 RVA: 0x004E7D2C File Offset: 0x004E5F2C
		private bool TryFindSpawnCell(CellRect rect, out IntVec3 result)
		{
			Map map = BaseGen.globalSettings.map;
			return CellFinder.TryFindRandomCellInsideWith(rect, delegate(IntVec3 c)
			{
				if (c.Standable(map) && !c.Roofed(map) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(c, map) && c.GetFirstItem(map) == null)
				{
					return !GenSpawn.WouldWipeAnythingWith(c, Rot4.North, ThingDefOf.Campfire, map, (Thing x) => x.def.category == ThingCategory.Building);
				}
				return false;
			}, out result);
		}
	}
}
