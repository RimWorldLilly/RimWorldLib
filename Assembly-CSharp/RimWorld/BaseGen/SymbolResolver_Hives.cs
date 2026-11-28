using System;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02002129 RID: 8489
	public class SymbolResolver_Hives : SymbolResolver
	{
		// Token: 0x0600D4BE RID: 54462 RVA: 0x004EC4D8 File Offset: 0x004EA6D8
		public override bool CanResolve(ResolveParams rp)
		{
			IntVec3 intVec;
			return base.CanResolve(rp) && Faction.OfInsects != null && this.TryFindFirstHivePos(rp.rect, out intVec);
		}

		// Token: 0x0600D4BF RID: 54463 RVA: 0x004EC50C File Offset: 0x004EA70C
		public override void Resolve(ResolveParams rp)
		{
			IntVec3 spawnCell;
			if (!this.TryFindFirstHivePos(rp.rect, out spawnCell))
			{
				return;
			}
			int count = rp.hivesCount ?? SymbolResolver_Hives.DefaultHivesCountRange.RandomInRange;
			HiveUtility.SpawnHives(spawnCell, BaseGen.globalSettings.map, count, 5f, WipeMode.VanishOrMoveAside, false, true, true, rp.disableHives.GetValueOrDefault(), true, true);
		}

		// Token: 0x0600D4C0 RID: 54464 RVA: 0x004EC57C File Offset: 0x004EA77C
		private bool TryFindFirstHivePos(CellRect rect, out IntVec3 pos)
		{
			Map map = BaseGen.globalSettings.map;
			return (from mc in rect.Cells
			where mc.Standable(map)
			select mc).TryRandomElement(out pos);
		}

		// Token: 0x040086F7 RID: 34551
		private static readonly IntRange DefaultHivesCountRange = new IntRange(1, 3);
	}
}
