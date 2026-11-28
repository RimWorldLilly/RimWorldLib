using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B07 RID: 6919
	public class SignalAction_Letter : SignalAction
	{
		// Token: 0x0600B0E5 RID: 45285 RVA: 0x003E5238 File Offset: 0x003E3438
		protected override void DoAction(SignalArgs args)
		{
			if (this.requireLookTargetsNotDestroyed && this.lookTargets.PrimaryTarget.ThingDestroyed)
			{
				return;
			}
			Pawn pawn;
			Pawn pawn2;
			if (args.TryGetArg<Pawn>("SUBJECT", out pawn))
			{
				pawn2 = (this.fixedPawnReference ?? pawn);
				if (!this.lookTargets.IsValid())
				{
					this.lookTargets = pawn;
				}
			}
			else
			{
				pawn2 = this.fixedPawnReference;
			}
			ValueTuple<string, string> labelMessage = this.GetLabelMessage(pawn2);
			Find.LetterStack.ReceiveLetter(labelMessage.Item1, labelMessage.Item2, this.letterDef, this.lookTargets, null, null, null, null, 0, true);
		}

		// Token: 0x0600B0E6 RID: 45286 RVA: 0x003E52DC File Offset: 0x003E34DC
		[return: TupleElementNames(new string[]
		{
			"label",
			"message"
		})]
		private ValueTuple<string, string> GetLabelMessage(Pawn pawn = null)
		{
			string item;
			if (!string.IsNullOrEmpty(this.letterLabel))
			{
				item = this.letterLabel;
			}
			else if (pawn != null)
			{
				item = this.letterLabelKey.Translate(pawn.Named("PAWN"));
			}
			else
			{
				item = this.letterLabelKey.Translate();
			}
			string item2;
			if (!string.IsNullOrEmpty(this.letterMessage))
			{
				item2 = this.letterMessage;
			}
			else if (pawn != null)
			{
				item2 = this.letterMessageKey.Translate(pawn.Named("PAWN"));
			}
			else
			{
				item2 = this.letterMessageKey.Translate();
			}
			return new ValueTuple<string, string>(item, item2);
		}

		// Token: 0x0600B0E7 RID: 45287 RVA: 0x003E5380 File Offset: 0x003E3580
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.letterLabelKey, "letterLabelKey", null, false);
			Scribe_Values.Look<string>(ref this.letterLabel, "letterLabel", null, false);
			Scribe_Values.Look<bool>(ref this.requireLookTargetsNotDestroyed, "requireLookTargetsSpawned", false, false);
			Scribe_Values.Look<string>(ref this.letterMessageKey, "letterMessageKey", null, false);
			Scribe_Values.Look<string>(ref this.letterMessage, "letterMessage", null, false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_Defs.Look<LetterDef>(ref this.letterDef, "letterDef");
			Scribe_References.Look<Pawn>(ref this.fixedPawnReference, "fixedPawnReference", true);
			Scribe_Deep.Look<Letter>(ref this.tmpLetter, "letter", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.tmpLetter != null)
			{
				this.letterLabel = this.tmpLetter.Label;
				this.lookTargets = this.tmpLetter.lookTargets;
				this.letterDef = this.tmpLetter.def;
				ChoiceLetter choiceLetter = this.tmpLetter as ChoiceLetter;
				if (choiceLetter != null)
				{
					this.letterMessage = choiceLetter.Text;
				}
			}
		}

		// Token: 0x0400661C RID: 26140
		public string letterLabelKey;

		// Token: 0x0400661D RID: 26141
		public string letterMessageKey;

		// Token: 0x0400661E RID: 26142
		public LetterDef letterDef;

		// Token: 0x0400661F RID: 26143
		public LookTargets lookTargets;

		// Token: 0x04006620 RID: 26144
		public Pawn fixedPawnReference;

		// Token: 0x04006621 RID: 26145
		private string letterLabel;

		// Token: 0x04006622 RID: 26146
		private string letterMessage;

		// Token: 0x04006623 RID: 26147
		public bool requireLookTargetsNotDestroyed;

		// Token: 0x04006624 RID: 26148
		private Letter tmpLetter;
	}
}
