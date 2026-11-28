using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CAF RID: 3247
	public class LordJob_Ritual_Mutilation : LordJob_Ritual
	{
		// Token: 0x06005C29 RID: 23593 RVA: 0x0021E528 File Offset: 0x0021C728
		public LordJob_Ritual_Mutilation()
		{
		}

		// Token: 0x06005C2A RID: 23594 RVA: 0x0021E53C File Offset: 0x0021C73C
		public LordJob_Ritual_Mutilation(TargetInfo selectedTarget, Precept_Ritual ritual, RitualObligation obligation, List<RitualStage> allStages, RitualRoleAssignments assignments, Pawn organizer = null) : base(selectedTarget, ritual, obligation, allStages, assignments, organizer, null)
		{
		}

		// Token: 0x06005C2B RID: 23595 RVA: 0x0021E56C File Offset: 0x0021C76C
		protected override bool ShouldCallOffBecausePawnNoLongerOwned(Pawn p)
		{
			return base.ShouldCallOffBecausePawnNoLongerOwned(p) && !this.mutilatedPawns.Contains(p);
		}

		// Token: 0x06005C2C RID: 23596 RVA: 0x0021E588 File Offset: 0x0021C788
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.mutilatedPawns, "mutilatedPawns", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x040034B8 RID: 13496
		public List<Pawn> mutilatedPawns = new List<Pawn>();
	}
}
