using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E88 RID: 3720
	public class ThoughtWorker_CribQuality : ThoughtWorker
	{
		// Token: 0x06006256 RID: 25174 RVA: 0x002358B0 File Offset: 0x00233AB0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			Building_Bed building_Bed = p.CurrentBed();
			if (building_Bed == null || !building_Bed.def.building.bed_crib)
			{
				return false;
			}
			QualityCategory stageIndex;
			building_Bed.TryGetQuality(out stageIndex);
			return ThoughtState.ActiveAtStage((int)stageIndex);
		}
	}
}
