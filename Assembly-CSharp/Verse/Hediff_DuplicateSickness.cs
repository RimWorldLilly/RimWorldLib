using System;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020004AE RID: 1198
	public class Hediff_DuplicateSickness : HediffWithComps
	{
		// Token: 0x06002135 RID: 8501 RVA: 0x000C5E5F File Offset: 0x000C405F
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (!ModLister.CheckAnomaly("Duplicate sickness"))
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			base.PostAdd(dinfo);
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x000C5E88 File Offset: 0x000C4088
		public override void PostTickInterval(int delta)
		{
			base.PostTickInterval(delta);
			if (this.Severity >= 0.2f && !Find.History.duplicateSicknessDiscovered)
			{
				Find.LetterStack.ReceiveLetter("DuplicateSicknessLabel".Translate(), "DuplicateSicknessText".Translate(this.pawn), LetterDefOf.NegativeEvent, this.pawn, null, null, null, null, 0, true);
				Find.History.Notify_DuplicateSicknessDiscovered();
			}
			if (this.pawn.IsWorldPawn() && this.Severity >= 0.7f && !this.pawn.Dead)
			{
				this.pawn.Kill(null, null);
			}
		}

		// Token: 0x0400195E RID: 6494
		private const float LetterSeverityThreshold = 0.2f;

		// Token: 0x0400195F RID: 6495
		private const float WorldPawnKillSeverityThreshold = 0.7f;
	}
}
