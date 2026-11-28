using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DF7 RID: 3575
	public class ThoughtWorker_HospitalPatientRoomStats : ThoughtWorker
	{
		// Token: 0x060060D4 RID: 24788 RVA: 0x00230AAC File Offset: 0x0022ECAC
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Building_Bed building_Bed = p.CurrentBed();
			if (building_Bed == null || !building_Bed.Medical)
			{
				return ThoughtState.Inactive;
			}
			Room room = p.GetRoom(RegionType.Set_All);
			if (room == null || room.Role != RoomRoleDefOf.Hospital)
			{
				return ThoughtState.Inactive;
			}
			int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(room.GetStat(RoomStatDefOf.Impressiveness));
			if (this.def.stages[scoreStageIndex] != null)
			{
				return ThoughtState.ActiveAtStage(scoreStageIndex);
			}
			return ThoughtState.Inactive;
		}
	}
}
