using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020010AD RID: 4269
	public class QuestPart_Bestowing_TargetChangedTitle : QuestPart
	{
		// Token: 0x06006AD2 RID: 27346 RVA: 0x002583B8 File Offset: 0x002565B8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				RoyalTitleDef titleAwardedWhenUpdating = this.pawn.royalty.GetTitleAwardedWhenUpdating(this.bestower.Faction, this.pawn.royalty.GetFavor(this.bestower.Faction));
				string key;
				string key2;
				LetterDef textLetterDef;
				if (titleAwardedWhenUpdating != null && titleAwardedWhenUpdating.seniority > this.currentTitle.seniority)
				{
					key = "LetterLabelBestowingCeremonyTitleUpdated";
					key2 = "LetterTextBestowingCeremonyTitleUpdated";
					textLetterDef = LetterDefOf.NeutralEvent;
					SoundDefOf.Quest_Concluded.PlayOneShotOnCamera(null);
				}
				else
				{
					key = "LetterQuestFailedLabel";
					key2 = "LetterQuestCompletedFail";
					textLetterDef = LetterDefOf.NegativeEvent;
					SoundDefOf.Quest_Failed.PlayOneShotOnCamera(null);
				}
				Find.LetterStack.ReceiveLetter(key.Translate(this.pawn.Named("TARGET")), key2.Translate(this.quest.name.CapitalizeFirst(), this.pawn.Named("TARGET")), textLetterDef, this.pawn, null, this.quest, null, null, 0, true);
				this.quest.End(QuestEndOutcome.Unknown, false, true);
			}
		}

		// Token: 0x06006AD3 RID: 27347 RVA: 0x002584E4 File Offset: 0x002566E4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_References.Look<Pawn>(ref this.bestower, "bestower", false);
			Scribe_Defs.Look<RoyalTitleDef>(ref this.currentTitle, "currentTitle");
		}

		// Token: 0x040043E7 RID: 17383
		public string inSignal;

		// Token: 0x040043E8 RID: 17384
		public Pawn pawn;

		// Token: 0x040043E9 RID: 17385
		public Pawn bestower;

		// Token: 0x040043EA RID: 17386
		public RoyalTitleDef currentTitle;
	}
}
