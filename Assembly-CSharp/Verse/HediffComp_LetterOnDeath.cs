using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000454 RID: 1108
	public class HediffComp_LetterOnDeath : HediffComp
	{
		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06001FA1 RID: 8097 RVA: 0x000BFEFC File Offset: 0x000BE0FC
		public HediffCompProperties_LetterOnDeath Props
		{
			get
			{
				return (HediffCompProperties_LetterOnDeath)this.props;
			}
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06001FA2 RID: 8098 RVA: 0x000BFF09 File Offset: 0x000BE109
		private bool ShouldSendLetter
		{
			get
			{
				return (!this.Props.onlyIfNoMechanitorDied || !Find.History.mechanitorEverDied) && (this.parent.pawn == null || !PawnGenerator.IsBeingGenerated(this.parent.pawn));
			}
		}

		// Token: 0x06001FA3 RID: 8099 RVA: 0x000BFF48 File Offset: 0x000BE148
		public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
			base.Notify_PawnDied(dinfo, culprit);
			if (this.ShouldSendLetter)
			{
				Find.LetterStack.ReceiveLetter(this.Props.letterLabel.Formatted(this.parent.Named("HEDIFF")), this.Props.letterText.Formatted(this.parent.pawn.Named("PAWN"), this.parent.Named("HEDIFF")), this.Props.letterDef ?? LetterDefOf.NeutralEvent, this.parent.pawn, null, null, null, null, 0, true);
			}
		}
	}
}
