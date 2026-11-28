using System;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse
{
	// Token: 0x0200040F RID: 1039
	public class HediffComp_CauseMentalState : HediffComp
	{
		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x06001EAF RID: 7855 RVA: 0x000BC35C File Offset: 0x000BA55C
		public HediffCompProperties_CauseMentalState Props
		{
			get
			{
				return (HediffCompProperties_CauseMentalState)this.props;
			}
		}

		// Token: 0x06001EB0 RID: 7856 RVA: 0x000BC36C File Offset: 0x000BA56C
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			if (this.parent.Severity < this.Props.minSeverity)
			{
				return;
			}
			if (!base.Pawn.IsHashIntervalTick(60, delta))
			{
				return;
			}
			if (base.Pawn.RaceProps.Humanlike)
			{
				if (base.Pawn.mindState.mentalStateHandler.CurStateDef != this.Props.humanMentalState && Rand.MTBEventOccurs(this.Props.mtbDaysToCauseMentalState, 60000f, 60f) && base.Pawn.Awake() && base.Pawn.Spawned && base.Pawn.mindState.mentalStateHandler.TryStartMentalState(this.Props.humanMentalState, this.parent.def.LabelCap, this.Props.forced, false, false, null, true, false, false))
				{
					this.TriggeredMentalBreak(this.Props.humanMentalState);
					return;
				}
			}
			else if (base.Pawn.RaceProps.Animal && base.Pawn.mindState.mentalStateHandler.CurStateDef != this.Props.animalMentalState && (this.Props.animalMentalStateAlias == null || base.Pawn.mindState.mentalStateHandler.CurStateDef != this.Props.animalMentalStateAlias) && Rand.MTBEventOccurs(this.Props.mtbDaysToCauseMentalState, 60000f, 60f) && base.Pawn.Awake() && base.Pawn.Spawned && base.Pawn.mindState.mentalStateHandler.TryStartMentalState(this.Props.animalMentalState, this.parent.def.LabelCap, false, false, false, null, true, false, false))
			{
				this.TriggeredMentalBreak(this.Props.animalMentalState);
			}
		}

		// Token: 0x06001EB1 RID: 7857 RVA: 0x000BC56C File Offset: 0x000BA76C
		public override void CompPostPostRemoved()
		{
			if (this.Props.endMentalStateOnCure && ((base.Pawn.RaceProps.Humanlike && base.Pawn.mindState.mentalStateHandler.CurStateDef == this.Props.humanMentalState) || (base.Pawn.RaceProps.Animal && (base.Pawn.mindState.mentalStateHandler.CurStateDef == this.Props.animalMentalState || base.Pawn.mindState.mentalStateHandler.CurStateDef == this.Props.animalMentalStateAlias))) && !base.Pawn.mindState.mentalStateHandler.CurState.causedByMood)
			{
				base.Pawn.mindState.mentalStateHandler.CurState.RecoverFromState();
			}
		}

		// Token: 0x06001EB2 RID: 7858 RVA: 0x000BC649 File Offset: 0x000BA849
		private void TriggeredMentalBreak(MentalStateDef mentalStateDef)
		{
			this.SendLetter(mentalStateDef);
			if (this.Props.removeOnTriggered)
			{
				base.Pawn.health.RemoveHediff(this.parent);
			}
		}

		// Token: 0x06001EB3 RID: 7859 RVA: 0x000BC678 File Offset: 0x000BA878
		private void SendLetter(MentalStateDef mentalStateDef)
		{
			string str;
			if (!string.IsNullOrEmpty(this.Props.overrideLetterLabel))
			{
				str = this.Props.overrideLetterLabel.Formatted(base.Pawn.Named("PAWN")).Resolve();
			}
			else
			{
				str = (mentalStateDef.beginLetterLabel ?? mentalStateDef.LabelCap).CapitalizeFirst() + ": " + base.Pawn.LabelShortCap;
			}
			string str2;
			if (!string.IsNullOrEmpty(this.Props.overrideLetterDesc))
			{
				str2 = this.Props.overrideLetterDesc.Formatted(base.Pawn.Named("PAWN")).Resolve();
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(base.Pawn.mindState.mentalStateHandler.CurState.GetBeginLetterText());
				defaultInterpolatedStringHandler.AppendLiteral("\n\n");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("CausedByHediff".Translate(this.parent.LabelCap));
				str2 = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			Find.LetterStack.ReceiveLetter(str, str2, this.Props.letterDef ?? mentalStateDef.beginLetterDef, base.Pawn, null, null, null, null, 0, true);
		}
	}
}
