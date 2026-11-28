using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000438 RID: 1080
	public class HediffComp_GiveHediff : HediffComp
	{
		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06001F41 RID: 8001 RVA: 0x000BE4D4 File Offset: 0x000BC6D4
		private HediffCompProperties_GiveHediff Props
		{
			get
			{
				return (HediffCompProperties_GiveHediff)this.props;
			}
		}

		// Token: 0x06001F42 RID: 8002 RVA: 0x000BE4E4 File Offset: 0x000BC6E4
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			if (this.parent.Severity < this.Props.atSeverity)
			{
				return;
			}
			if (this.Props.skipIfAlreadyExists && base.Pawn.health.hediffSet.HasHediff(this.Props.hediffDef, false))
			{
				return;
			}
			base.Pawn.health.AddHediff(this.Props.hediffDef, null, null, null);
			if (PawnUtility.ShouldSendNotificationAbout(base.Pawn))
			{
				this.SendLetter();
			}
			if (this.Props.disappearsAfterGiving)
			{
				base.Pawn.health.RemoveHediff(this.parent);
			}
		}

		// Token: 0x06001F43 RID: 8003 RVA: 0x000BE598 File Offset: 0x000BC798
		private void SendLetter()
		{
			if (this.Props.letterLabel == null)
			{
				return;
			}
			Find.LetterStack.ReceiveLetter(this.Props.letterLabel.Formatted(base.Pawn.Named("PAWN")), this.Props.letterText.Formatted(base.Pawn.Named("PAWN")), this.Props.letterDef ?? LetterDefOf.NegativeEvent, base.Pawn, null, null, null, null, 0, true);
		}
	}
}
