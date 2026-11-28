using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CB5 RID: 3253
	public class LordJob_Joinable_Speech : LordJob_Ritual
	{
		// Token: 0x17001016 RID: 4118
		// (get) Token: 0x06005C69 RID: 23657 RVA: 0x0021F8B6 File Offset: 0x0021DAB6
		protected override int MinTicksToFinish
		{
			get
			{
				return base.DurationTicks / 2;
			}
		}

		// Token: 0x17001017 RID: 4119
		// (get) Token: 0x06005C6A RID: 23658 RVA: 0x0021F8C0 File Offset: 0x0021DAC0
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001018 RID: 4120
		// (get) Token: 0x06005C6B RID: 23659 RVA: 0x0021F8C3 File Offset: 0x0021DAC3
		public override bool OrganizerIsStartingPawn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005C6C RID: 23660 RVA: 0x0021F8C6 File Offset: 0x0021DAC6
		public LordJob_Joinable_Speech()
		{
		}

		// Token: 0x06005C6D RID: 23661 RVA: 0x0021F8D0 File Offset: 0x0021DAD0
		public LordJob_Joinable_Speech(TargetInfo spot, Pawn organizer, Precept_Ritual ritual, List<RitualStage> stages, RitualRoleAssignments assignments, bool titleSpeech) : base(spot, ritual, null, stages, assignments, organizer, null)
		{
			Building_Throne firstThing = spot.Cell.GetFirstThing(organizer.Map);
			if (firstThing != null)
			{
				this.selectedTarget = firstThing;
			}
			this.titleSpeech = titleSpeech;
		}

		// Token: 0x06005C6E RID: 23662 RVA: 0x0021F91E File Offset: 0x0021DB1E
		protected override LordToil_Ritual MakeToil(RitualStage stage)
		{
			if (stage == null)
			{
				return new LordToil_Speech(this.spot, this.ritual, this, this.organizer);
			}
			return new LordToil_Ritual(this.spot, this, stage, this.organizer);
		}

		// Token: 0x06005C6F RID: 23663 RVA: 0x0021F950 File Offset: 0x0021DB50
		public override string GetReport(Pawn pawn)
		{
			return ((pawn != this.organizer) ? "LordReportListeningSpeech".Translate(this.organizer.Named("ORGANIZER")) : "LordReportGivingSpeech".Translate()) + base.TimeLeftPostfix;
		}

		// Token: 0x06005C70 RID: 23664 RVA: 0x0021F99C File Offset: 0x0021DB9C
		public override void ApplyOutcome(float progress, bool showFinishedMessage = true, bool showFailedMessage = true, bool cancelled = false)
		{
			if (this.ticksPassed >= this.MinTicksToFinish && !cancelled)
			{
				base.ApplyOutcome(progress, false, true, false);
				return;
			}
			Find.LetterStack.ReceiveLetter("LetterLabelSpeechCancelled".Translate(), "LetterSpeechCancelled".Translate(this.titleSpeech ? GrammarResolverSimple.PawnResolveBestRoyalTitle(this.organizer) : this.organizer.LabelShort).CapitalizeFirst(), LetterDefOf.NegativeEvent, this.organizer, null, null, null, null, 0, true);
			RitualOutcomeEffectWorker outcomeEffect = this.ritual.outcomeEffect;
			if (outcomeEffect == null)
			{
				return;
			}
			outcomeEffect.ResetCompDatas();
		}

		// Token: 0x040034C4 RID: 13508
		private bool titleSpeech;
	}
}
