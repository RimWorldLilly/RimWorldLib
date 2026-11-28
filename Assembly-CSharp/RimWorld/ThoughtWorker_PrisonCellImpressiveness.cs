using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DF5 RID: 3573
	public class ThoughtWorker_PrisonCellImpressiveness : ThoughtWorker_RoomImpressiveness
	{
		// Token: 0x060060D0 RID: 24784 RVA: 0x00230A0C File Offset: 0x0022EC0C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.IsPrisoner)
			{
				return ThoughtState.Inactive;
			}
			ThoughtState result = base.CurrentStateInternal(p);
			if (result.Active && p.GetRoom(RegionType.Set_All).Role == RoomRoleDefOf.PrisonCell)
			{
				return result;
			}
			return ThoughtState.Inactive;
		}
	}
}
