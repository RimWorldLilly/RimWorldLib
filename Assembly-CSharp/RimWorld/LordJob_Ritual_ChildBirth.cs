using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CAC RID: 3244
	public class LordJob_Ritual_ChildBirth : LordJob_Ritual
	{
		// Token: 0x06005C12 RID: 23570 RVA: 0x0021E0C0 File Offset: 0x0021C2C0
		public LordJob_Ritual_ChildBirth()
		{
		}

		// Token: 0x06005C13 RID: 23571 RVA: 0x0021E0C8 File Offset: 0x0021C2C8
		public LordJob_Ritual_ChildBirth(TargetInfo selectedTarget, Precept_Ritual ritual, RitualObligation obligation, List<RitualStage> allStages, RitualRoleAssignments assignments, Pawn organizer = null, IntVec3? spotOverride = null) : base(selectedTarget, ritual, obligation, allStages, assignments, organizer, spotOverride)
		{
		}

		// Token: 0x06005C14 RID: 23572 RVA: 0x0021E0DB File Offset: 0x0021C2DB
		protected override bool RitualFinished(float progress, bool cancelled)
		{
			return progress == 1f;
		}
	}
}
