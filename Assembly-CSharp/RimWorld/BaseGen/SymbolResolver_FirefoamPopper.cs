using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02002104 RID: 8452
	public class SymbolResolver_FirefoamPopper : SymbolResolver
	{
		// Token: 0x0600D44D RID: 54349 RVA: 0x004E7960 File Offset: 0x004E5B60
		public override bool CanResolve(ResolveParams rp)
		{
			IntVec3 intVec;
			return base.CanResolve(rp) && this.TryFindSpawnCell(rp.rect, out intVec);
		}

		// Token: 0x0600D44E RID: 54350 RVA: 0x004E798C File Offset: 0x004E5B8C
		public override void Resolve(ResolveParams rp)
		{
			IntVec3 loc;
			if (!this.TryFindSpawnCell(rp.rect, out loc))
			{
				return;
			}
			Thing thing = ThingMaker.MakeThing(ThingDefOf.FirefoamPopper, null);
			thing.SetFaction(rp.faction, null);
			GenSpawn.Spawn(thing, loc, BaseGen.globalSettings.map, WipeMode.Vanish);
		}

		// Token: 0x0600D44F RID: 54351 RVA: 0x004E79D4 File Offset: 0x004E5BD4
		private bool TryFindSpawnCell(CellRect rect, out IntVec3 result)
		{
			Map map = BaseGen.globalSettings.map;
			return CellFinder.TryFindRandomCellInsideWith(rect, delegate(IntVec3 c)
			{
				if (c.Standable(map) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(c, map) && c.GetFirstItem(map) == null)
				{
					return !GenSpawn.WouldWipeAnythingWith(c, Rot4.North, ThingDefOf.FirefoamPopper, map, (Thing x) => x.def.category == ThingCategory.Building);
				}
				return false;
			}, out result);
		}
	}
}
