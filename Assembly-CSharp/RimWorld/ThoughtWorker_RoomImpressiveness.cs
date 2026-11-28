using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DF4 RID: 3572
	public abstract class ThoughtWorker_RoomImpressiveness : ThoughtWorker
	{
		// Token: 0x060060CE RID: 24782 RVA: 0x00230990 File Offset: 0x0022EB90
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.story.traits.HasTrait(TraitDefOf.Ascetic))
			{
				return ThoughtState.Inactive;
			}
			Room room = p.GetRoom(RegionType.Set_All);
			if (room == null)
			{
				return ThoughtState.Inactive;
			}
			int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(room.GetStat(RoomStatDefOf.Impressiveness));
			if (this.def.stages[scoreStageIndex] == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(scoreStageIndex);
		}
	}
}
