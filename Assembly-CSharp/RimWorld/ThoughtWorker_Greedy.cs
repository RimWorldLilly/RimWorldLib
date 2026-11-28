using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E3B RID: 3643
	public class ThoughtWorker_Greedy : ThoughtWorker
	{
		// Token: 0x0600616C RID: 24940 RVA: 0x00232BB4 File Offset: 0x00230DB4
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.IsColonist)
			{
				return false;
			}
			Room ownedRoom = p.ownership.OwnedRoom;
			if (ownedRoom == null)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			int num = RoomStatDefOf.Impressiveness.GetScoreStageIndex(ownedRoom.GetStat(RoomStatDefOf.Impressiveness)) + 1;
			if (this.def.stages[num] != null)
			{
				return ThoughtState.ActiveAtStage(num);
			}
			return ThoughtState.Inactive;
		}
	}
}
