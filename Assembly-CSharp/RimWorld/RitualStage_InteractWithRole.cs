using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200167C RID: 5756
	public class RitualStage_InteractWithRole : RitualStage
	{
		// Token: 0x06009052 RID: 36946 RVA: 0x0034FBDB File Offset: 0x0034DDDB
		public override TargetInfo GetSecondFocus(LordJob_Ritual ritual)
		{
			if (this.targetId != null)
			{
				return ritual.PawnWithRole(this.targetId);
			}
			return base.GetSecondFocus(ritual);
		}

		// Token: 0x06009053 RID: 36947 RVA: 0x0034FBFE File Offset: 0x0034DDFE
		public override IEnumerable<RitualStagePawnSecondFocus> GetPawnSecondFoci(LordJob_Ritual ritual)
		{
			RitualStage_InteractWithRole.<GetPawnSecondFoci>d__4 <GetPawnSecondFoci>d__ = new RitualStage_InteractWithRole.<GetPawnSecondFoci>d__4(-2);
			<GetPawnSecondFoci>d__.<>4__this = this;
			<GetPawnSecondFoci>d__.<>3__ritual = ritual;
			return <GetPawnSecondFoci>d__;
		}

		// Token: 0x06009054 RID: 36948 RVA: 0x0034FC15 File Offset: 0x0034DE15
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.targetId, "targetId", null, false);
			Scribe_Collections.Look<RitualStage_InteractWithRole.PawnTarget>(ref this.targets, "targets", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x0400581B RID: 22555
		[NoTranslate]
		public string targetId;

		// Token: 0x0400581C RID: 22556
		public List<RitualStage_InteractWithRole.PawnTarget> targets;

		// Token: 0x020034C1 RID: 13505
		public class PawnTarget : IExposable
		{
			// Token: 0x0601357D RID: 79229 RVA: 0x0065B24C File Offset: 0x0065944C
			public void ExposeData()
			{
				Scribe_Values.Look<string>(ref this.pawnId, "pawnId", null, false);
				Scribe_Values.Look<string>(ref this.targetId, "targetId", null, false);
			}

			// Token: 0x0400D312 RID: 54034
			[NoTranslate]
			public string pawnId;

			// Token: 0x0400D313 RID: 54035
			[NoTranslate]
			public string targetId;
		}
	}
}
