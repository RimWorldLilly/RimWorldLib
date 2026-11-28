using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C3A RID: 3130
	public static class RepairUtility
	{
		// Token: 0x06005913 RID: 22803 RVA: 0x0020DDC0 File Offset: 0x0020BFC0
		public static bool PawnCanRepairEver(Pawn pawn, Thing t)
		{
			Building building = t as Building;
			return building != null && t.def.useHitPoints && building.def.building.repairable && t.Faction == pawn.Faction;
		}

		// Token: 0x06005914 RID: 22804 RVA: 0x0020DE0D File Offset: 0x0020C00D
		public static bool PawnCanRepairNow(Pawn pawn, Thing t)
		{
			return RepairUtility.PawnCanRepairEver(pawn, t) && pawn.Map.listerBuildingsRepairable.Contains(pawn.Faction, (Building)t) && t.HitPoints != t.MaxHitPoints;
		}
	}
}
