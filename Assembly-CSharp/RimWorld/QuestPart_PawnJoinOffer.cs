using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010D5 RID: 4309
	public class QuestPart_PawnJoinOffer : QuestPartActivable
	{
		// Token: 0x06006BC0 RID: 27584 RVA: 0x0025D4ED File Offset: 0x0025B6ED
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			if (this.sendLetterOnEnable)
			{
				this.SendLetter();
			}
		}

		// Token: 0x06006BC1 RID: 27585 RVA: 0x0025D504 File Offset: 0x0025B704
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.sendLetterOnEnable || this.letterSent || this.pawn == null || this.pawn.needs == null || this.pawn.needs.mood == null || !this.pawn.IsHashIntervalTick(2500))
			{
				return;
			}
			float curLevelPercentage = this.pawn.needs.mood.CurLevelPercentage;
			if (curLevelPercentage < 0.5f)
			{
				return;
			}
			if (Rand.MTBEventOccurs(QuestPart_PawnJoinOffer.JoinMTBbyMoodCurve.Evaluate(curLevelPercentage), 60000f, 2500f))
			{
				this.SendLetter();
			}
		}

		// Token: 0x06006BC2 RID: 27586 RVA: 0x0025D5A4 File Offset: 0x0025B7A4
		private void SendLetter()
		{
			if (this.letterSent)
			{
				return;
			}
			this.letter = (ChoiceLetter_AcceptVisitors)LetterMaker.MakeLetter(this.letterLabel, this.letterText, LetterDefOf.AcceptVisitors, null, this.quest);
			this.letter.title = this.letterTitle;
			this.letter.pawns.Add(this.pawn);
			this.letter.quest = this.quest;
			this.letter.acceptedSignal = this.outSignalPawnAccepted;
			this.letter.rejectedSignal = this.outSignalPawnRejected;
			this.letter.lookTargets = new LookTargets(this.pawn);
			this.letter.charity = this.charity;
			Find.LetterStack.ReceiveLetter(this.letter, null, 0, true);
			this.letterSent = true;
			if (this.pawn.guest != null)
			{
				this.pawn.guest.Recruitable = true;
			}
		}

		// Token: 0x06006BC3 RID: 27587 RVA: 0x0025D6A5 File Offset: 0x0025B8A5
		public override void Cleanup()
		{
			this.RemoveLetterIfActive();
		}

		// Token: 0x06006BC4 RID: 27588 RVA: 0x0025D6AD File Offset: 0x0025B8AD
		protected override void Disable()
		{
			this.RemoveLetterIfActive();
		}

		// Token: 0x06006BC5 RID: 27589 RVA: 0x0025D6B5 File Offset: 0x0025B8B5
		private void RemoveLetterIfActive()
		{
			if (this.letter != null && Find.LetterStack.LettersListForReading.Contains(this.letter))
			{
				Find.LetterStack.RemoveLetter(this.letter);
			}
		}

		// Token: 0x06006BC6 RID: 27590 RVA: 0x0025D6E8 File Offset: 0x0025B8E8
		public override void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
			if (base.State != QuestPartState.Enabled || this.letterSent || this.pawn == null)
			{
				return;
			}
			Rect rect = new Rect(innerRect.x, curY, 500f, 25f);
			Rect rect2 = rect;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Add Join Letter for ");
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(this.pawn.NameShortColored);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			if (Widgets.ButtonText(rect2, defaultInterpolatedStringHandler.ToStringAndClear() + base.GetType().Name, true, true, true, null))
			{
				this.SendLetter();
			}
			curY += rect.height + 4f;
		}

		// Token: 0x06006BC7 RID: 27591 RVA: 0x0025D7A0 File Offset: 0x0025B9A0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_Values.Look<bool>(ref this.letterSent, "letterSent", false, false);
			Scribe_Values.Look<string>(ref this.outSignalPawnAccepted, "outSignalPawnAccepted", null, false);
			Scribe_Values.Look<string>(ref this.outSignalPawnRejected, "outSignalPawnRejected", null, false);
			Scribe_Values.Look<string>(ref this.letterLabel, "letterLabel", null, false);
			Scribe_Values.Look<string>(ref this.letterText, "letterText", null, false);
			Scribe_Values.Look<string>(ref this.letterTitle, "letterTitle", null, false);
			Scribe_Values.Look<bool>(ref this.charity, "charity", false, false);
			Scribe_Values.Look<bool>(ref this.sendLetterOnEnable, "sendLetterOnEnable", false, false);
		}

		// Token: 0x0400447F RID: 17535
		public Pawn pawn;

		// Token: 0x04004480 RID: 17536
		public bool letterSent;

		// Token: 0x04004481 RID: 17537
		public string outSignalPawnAccepted;

		// Token: 0x04004482 RID: 17538
		public string outSignalPawnRejected;

		// Token: 0x04004483 RID: 17539
		public string letterLabel;

		// Token: 0x04004484 RID: 17540
		public string letterText;

		// Token: 0x04004485 RID: 17541
		public string letterTitle;

		// Token: 0x04004486 RID: 17542
		public bool charity;

		// Token: 0x04004487 RID: 17543
		public bool sendLetterOnEnable;

		// Token: 0x04004488 RID: 17544
		private ChoiceLetter_AcceptVisitors letter;

		// Token: 0x04004489 RID: 17545
		private const float MinMoodPercentage = 0.5f;

		// Token: 0x0400448A RID: 17546
		private static readonly SimpleCurve JoinMTBbyMoodCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.5f, 60f),
				true
			},
			{
				new CurvePoint(1f, 15f),
				true
			}
		};

		// Token: 0x0400448B RID: 17547
		private const int CheckInterval = 2500;
	}
}
