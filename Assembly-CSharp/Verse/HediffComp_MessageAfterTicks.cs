using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200045A RID: 1114
	public class HediffComp_MessageAfterTicks : HediffComp
	{
		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x06001FB5 RID: 8117 RVA: 0x000C0384 File Offset: 0x000BE584
		protected HediffCompProperties_MessageAfterTicks Props
		{
			get
			{
				return (HediffCompProperties_MessageAfterTicks)this.props;
			}
		}

		// Token: 0x06001FB6 RID: 8118 RVA: 0x000C0391 File Offset: 0x000BE591
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.ticksUntilMessage = this.Props.ticks;
		}

		// Token: 0x06001FB7 RID: 8119 RVA: 0x000C03AC File Offset: 0x000BE5AC
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (this.ticksUntilMessage == 0)
			{
				if (PawnUtility.ShouldSendNotificationAbout(base.Pawn))
				{
					if (this.Props.messageType != null)
					{
						Messages.Message(this.Props.message.Formatted(base.Pawn), base.Pawn, this.Props.messageType, true);
					}
					if (this.Props.letterType != null)
					{
						Find.LetterStack.ReceiveLetter(this.Props.letterLabel.Formatted(base.Pawn), this.GetLetterText(), this.Props.letterType, base.Pawn, null, null, null, null, 0, true);
					}
				}
				this.ticksUntilMessage--;
				return;
			}
			if (this.ticksUntilMessage > 0)
			{
				this.ticksUntilMessage--;
			}
		}

		// Token: 0x06001FB8 RID: 8120 RVA: 0x000C0497 File Offset: 0x000BE697
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<int>(ref this.ticksUntilMessage, "ticksUntilMessage", 0, false);
		}

		// Token: 0x06001FB9 RID: 8121 RVA: 0x000C04B4 File Offset: 0x000BE6B4
		private TaggedString GetLetterText()
		{
			Hediff_Pregnant hediff_Pregnant = this.parent as Hediff_Pregnant;
			if (hediff_Pregnant != null && hediff_Pregnant.Mother != null && hediff_Pregnant.Mother != hediff_Pregnant.pawn)
			{
				TaggedString taggedString = "IvfPregnancyLetterText".Translate(hediff_Pregnant.pawn.NameFullColored);
				if (hediff_Pregnant.Mother != null && hediff_Pregnant.Father != null)
				{
					taggedString += "\n\n" + "IvfPregnancyLetterParents".Translate(hediff_Pregnant.Mother.NameFullColored, hediff_Pregnant.Father.NameFullColored);
				}
				return taggedString;
			}
			return this.Props.letterText.Formatted(base.Pawn);
		}

		// Token: 0x040018B3 RID: 6323
		private int ticksUntilMessage;
	}
}
