using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000427 RID: 1063
	public class HediffComp_Discoverable : HediffComp
	{
		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06001F09 RID: 7945 RVA: 0x000BD562 File Offset: 0x000BB762
		public HediffCompProperties_Discoverable Props
		{
			get
			{
				return (HediffCompProperties_Discoverable)this.props;
			}
		}

		// Token: 0x06001F0A RID: 7946 RVA: 0x000BD56F File Offset: 0x000BB76F
		public override void CompExposeData()
		{
			Scribe_Values.Look<bool>(ref this.discovered, "discovered", false, false);
		}

		// Token: 0x06001F0B RID: 7947 RVA: 0x000BD583 File Offset: 0x000BB783
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			if (this.parent.pawn.IsHashIntervalTick(103, delta))
			{
				this.CheckDiscovered();
			}
		}

		// Token: 0x06001F0C RID: 7948 RVA: 0x000BD5A0 File Offset: 0x000BB7A0
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			this.CheckDiscovered();
		}

		// Token: 0x06001F0D RID: 7949 RVA: 0x000BD5A8 File Offset: 0x000BB7A8
		private void CheckDiscovered()
		{
			if (this.discovered)
			{
				return;
			}
			if (!this.parent.Visible)
			{
				return;
			}
			this.discovered = true;
			if (this.Props.sendLetterWhenDiscovered && PawnUtility.ShouldSendNotificationAbout(base.Pawn))
			{
				if (base.Pawn.RaceProps.Humanlike)
				{
					string str;
					if (!this.Props.discoverLetterLabel.NullOrEmpty())
					{
						str = string.Format(this.Props.discoverLetterLabel, base.Pawn.LabelShortCap).CapitalizeFirst();
					}
					else
					{
						str = "LetterLabelNewDisease".Translate() + ": " + base.Def.LabelCap;
					}
					string str2;
					if (!this.Props.discoverLetterText.NullOrEmpty())
					{
						str2 = this.Props.discoverLetterText.Formatted(base.Pawn.LabelIndefinite(), base.Pawn.Named("PAWN")).AdjustedFor(base.Pawn, "PAWN", true).CapitalizeFirst();
					}
					else if (this.parent.Part == null)
					{
						str2 = "NewDisease".Translate(base.Pawn.Named("PAWN"), base.Def.label, base.Pawn.LabelDefinite()).AdjustedFor(base.Pawn, "PAWN", true).CapitalizeFirst();
					}
					else
					{
						str2 = "NewPartDisease".Translate(base.Pawn.Named("PAWN"), this.parent.Part.Label, base.Pawn.LabelDefinite(), base.Def.label).AdjustedFor(base.Pawn, "PAWN", true).CapitalizeFirst();
					}
					Find.LetterStack.ReceiveLetter(str, str2, (this.Props.letterType != null) ? this.Props.letterType : LetterDefOf.NegativeEvent, base.Pawn, null, null, null, null, 0, true);
					return;
				}
				string text;
				if (!this.Props.discoverLetterText.NullOrEmpty())
				{
					string value = base.Pawn.KindLabelIndefinite();
					if (base.Pawn.Name.IsValid && !base.Pawn.Name.Numerical)
					{
						Name name = base.Pawn.Name;
						value = ((name != null) ? name.ToString() : null) + " (" + base.Pawn.KindLabel + ")";
					}
					text = this.Props.discoverLetterText.Formatted(value, base.Pawn.Named("PAWN")).AdjustedFor(base.Pawn, "PAWN", true).CapitalizeFirst();
				}
				else if (this.parent.Part == null)
				{
					text = "NewDiseaseAnimal".Translate(base.Pawn.LabelShort, base.Def.LabelCap, base.Pawn.LabelDefinite(), base.Pawn.Named("PAWN")).AdjustedFor(base.Pawn, "PAWN", true).CapitalizeFirst();
				}
				else
				{
					text = "NewPartDiseaseAnimal".Translate(base.Pawn.LabelShort, this.parent.Part.Label, base.Pawn.LabelDefinite(), base.Def.LabelCap, base.Pawn.Named("PAWN")).AdjustedFor(base.Pawn, "PAWN", true).CapitalizeFirst();
				}
				Messages.Message(text, base.Pawn, (this.Props.messageType != null) ? this.Props.messageType : MessageTypeDefOf.NegativeHealthEvent, true);
			}
		}

		// Token: 0x06001F0E RID: 7950 RVA: 0x000BD9EA File Offset: 0x000BBBEA
		public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
			this.CheckDiscovered();
		}

		// Token: 0x06001F0F RID: 7951 RVA: 0x000BD9F2 File Offset: 0x000BBBF2
		public override string CompDebugString()
		{
			return "discovered: " + this.discovered.ToString();
		}

		// Token: 0x04001849 RID: 6217
		private bool discovered;

		// Token: 0x0400184A RID: 6218
		private const int CheckInterval = 103;
	}
}
