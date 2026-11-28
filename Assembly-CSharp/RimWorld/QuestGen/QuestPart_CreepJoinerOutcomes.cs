using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200226E RID: 8814
	public class QuestPart_CreepJoinerOutcomes : QuestPart
	{
		// Token: 0x0600D9B7 RID: 55735 RVA: 0x0050C400 File Offset: 0x0050A600
		public void ShowOfferLetter(Pawn_CreepJoinerTracker creepjoiner)
		{
			if (this.letter != null)
			{
				this.letter.OpenLetter();
				return;
			}
			TaggedString label = "LetterCreeperInviteJoins".Translate(this.pawn.Named("PAWN"));
			TaggedString taggedString = creepjoiner.form.letterPrompt.Formatted(this.pawn.Named("PAWN")).CapitalizeFirst();
			taggedString += "\n\n" + creepjoiner.benefit.letterExtra.Formatted(this.pawn.Named("PAWN")).CapitalizeFirst();
			taggedString += "\n\n" + "LetterCreeperInviteAppend".Translate(this.pawn.Named("PAWN")).CapitalizeFirst();
			this.letter = (ChoiceLetter_AcceptCreepJoiner)LetterMaker.MakeLetter(label, taggedString, LetterDefOf.AcceptCreepJoiner, null, this.quest);
			this.letter.signalAccept = this.signalAccept;
			this.letter.signalCapture = this.signalCapture;
			this.letter.signalReject = this.signalReject;
			this.letter.pawn = this.pawn;
			this.letter.speaker = creepjoiner.speaker;
			Find.LetterStack.ReceiveLetter(this.letter, null, 0, true);
			this.letter.OpenLetter();
		}

		// Token: 0x0600D9B8 RID: 55736 RVA: 0x0050C564 File Offset: 0x0050A764
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			Pawn pawn = this.pawn;
			Pawn_CreepJoinerTracker pawn_CreepJoinerTracker = (pawn != null) ? pawn.creepjoiner : null;
			if (pawn_CreepJoinerTracker == null)
			{
				return;
			}
			if (signal.tag == this.signalShow)
			{
				this.ShowOfferLetter(pawn_CreepJoinerTracker);
				return;
			}
			if (signal.tag == this.signalAttacked)
			{
				Pawn instigatorPawn;
				signal.args.TryGetArg<Pawn>("INSTIGATOR", out instigatorPawn);
				pawn_CreepJoinerTracker.Notify_CreepJoinerAttacked(instigatorPawn);
				return;
			}
			if (signal.tag == this.signalReject)
			{
				pawn_CreepJoinerTracker.Notify_CreepJoinerRejected();
				return;
			}
			if (signal.tag == this.signalTimeout)
			{
				pawn_CreepJoinerTracker.Notify_CreepJoinerRejected();
				return;
			}
			if (signal.tag == this.signalCapture)
			{
				CaptureUtility.OrderArrest(pawn_CreepJoinerTracker.speaker, this.pawn);
			}
		}

		// Token: 0x0600D9B9 RID: 55737 RVA: 0x0050C62A File Offset: 0x0050A82A
		public override void Cleanup()
		{
			this.CloseLetter();
		}

		// Token: 0x0600D9BA RID: 55738 RVA: 0x0050C632 File Offset: 0x0050A832
		private void CloseLetter()
		{
			if (this.letter != null && Find.LetterStack.LettersListForReading.Contains(this.letter))
			{
				Find.LetterStack.RemoveLetter(this.letter);
			}
		}

		// Token: 0x0600D9BB RID: 55739 RVA: 0x0050C664 File Offset: 0x0050A864
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_References.Look<ChoiceLetter_AcceptCreepJoiner>(ref this.letter, "letter", false);
			Scribe_Values.Look<string>(ref this.signalAccept, "signalAccept", null, false);
			Scribe_Values.Look<string>(ref this.signalCapture, "signalCapture", null, false);
			Scribe_Values.Look<string>(ref this.signalReject, "signalReject", null, false);
			Scribe_Values.Look<string>(ref this.signalAttacked, "signalAttacked", null, false);
			Scribe_Values.Look<string>(ref this.signalTimeout, "signalTimeout", null, false);
			Scribe_Values.Look<string>(ref this.signalShow, "signalShow", null, false);
			Scribe_Values.Look<int>(ref this.timeout, "timeout", 0, false);
		}

		// Token: 0x04008B26 RID: 35622
		public Pawn pawn;

		// Token: 0x04008B27 RID: 35623
		public string signalAccept;

		// Token: 0x04008B28 RID: 35624
		public string signalCapture;

		// Token: 0x04008B29 RID: 35625
		public string signalReject;

		// Token: 0x04008B2A RID: 35626
		public string signalAttacked;

		// Token: 0x04008B2B RID: 35627
		public string signalShow;

		// Token: 0x04008B2C RID: 35628
		public string signalTimeout;

		// Token: 0x04008B2D RID: 35629
		public int timeout;

		// Token: 0x04008B2E RID: 35630
		private ChoiceLetter_AcceptCreepJoiner letter;
	}
}
