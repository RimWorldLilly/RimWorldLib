using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E3D RID: 3645
	public class ThoughtWorker_Ascetic : ThoughtWorker
	{
		// Token: 0x06006170 RID: 24944 RVA: 0x00232D3C File Offset: 0x00230F3C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.IsColonist)
			{
				return false;
			}
			Room ownedRoom = p.ownership.OwnedRoom;
			if (ownedRoom == null)
			{
				return false;
			}
			int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(ownedRoom.GetStat(RoomStatDefOf.Impressiveness));
			if (this.def.stages[scoreStageIndex] != null)
			{
				return ThoughtState.ActiveAtStage(scoreStageIndex);
			}
			return ThoughtState.Inactive;
		}
	}
}
