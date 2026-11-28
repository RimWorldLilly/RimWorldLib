using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CA9 RID: 3241
	public class LordJob_PartyDanceDrums : LordJob_Ritual
	{
		// Token: 0x06005BBF RID: 23487 RVA: 0x0021B758 File Offset: 0x00219958
		public LordJob_PartyDanceDrums()
		{
		}

		// Token: 0x06005BC0 RID: 23488 RVA: 0x0021B760 File Offset: 0x00219960
		public LordJob_PartyDanceDrums(TargetInfo selectedTarget, Precept_Ritual ritual, RitualObligation obligation, List<RitualStage> allStages, RitualRoleAssignments assignments, Pawn organizer = null) : base(selectedTarget, ritual, obligation, allStages, assignments, organizer, null)
		{
		}

		// Token: 0x06005BC1 RID: 23489 RVA: 0x0021B785 File Offset: 0x00219985
		protected override LordToil_Ritual MakeToil(RitualStage stage)
		{
			if (!ModLister.CheckIdeology("Drum party toil"))
			{
				return null;
			}
			return new LordToil_PartyDanceDrums(this.spot, this, stage, this.organizer);
		}
	}
}
