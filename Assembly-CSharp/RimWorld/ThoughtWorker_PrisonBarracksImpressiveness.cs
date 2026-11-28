using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DF6 RID: 3574
	public class ThoughtWorker_PrisonBarracksImpressiveness : ThoughtWorker_RoomImpressiveness
	{
		// Token: 0x060060D2 RID: 24786 RVA: 0x00230A5C File Offset: 0x0022EC5C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.IsPrisoner)
			{
				return ThoughtState.Inactive;
			}
			ThoughtState result = base.CurrentStateInternal(p);
			if (result.Active && p.GetRoom(RegionType.Set_All).Role == RoomRoleDefOf.PrisonBarracks)
			{
				return result;
			}
			return ThoughtState.Inactive;
		}
	}
}
