using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D4E RID: 7502
	public class ChoiceLetter_AcceptCreepJoiner : ChoiceLetter
	{
		// Token: 0x170023CD RID: 9165
		// (get) Token: 0x0600C225 RID: 49701 RVA: 0x0046C4A1 File Offset: 0x0046A6A1
		public override bool CanDismissWithRightClick
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170023CE RID: 9166
		// (get) Token: 0x0600C226 RID: 49702 RVA: 0x0046C4A4 File Offset: 0x0046A6A4
		public override bool CanShowInLetterStack
		{
			get
			{
				return base.CanShowInLetterStack && this.quest != null && (this.quest.State == QuestState.Ongoing || this.quest.State == QuestState.NotYetAccepted);
			}
		}

		// Token: 0x170023CF RID: 9167
		// (get) Token: 0x0600C227 RID: 49703 RVA: 0x0046C4D6 File Offset: 0x0046A6D6
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				ChoiceLetter_AcceptCreepJoiner.<get_Choices>d__10 <get_Choices>d__ = new ChoiceLetter_AcceptCreepJoiner.<get_Choices>d__10(-2);
				<get_Choices>d__.<>4__this = this;
				return <get_Choices>d__;
			}
		}

		// Token: 0x0600C228 RID: 49704 RVA: 0x0046C4E8 File Offset: 0x0046A6E8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.signalAccept, "signalAccept", null, false);
			Scribe_Values.Look<string>(ref this.signalCapture, "signalCapture", null, false);
			Scribe_Values.Look<string>(ref this.signalReject, "signalReject", null, false);
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_References.Look<Pawn>(ref this.speaker, "speaker", false);
		}

		// Token: 0x04006F05 RID: 28421
		public string signalAccept;

		// Token: 0x04006F06 RID: 28422
		public string signalCapture;

		// Token: 0x04006F07 RID: 28423
		public string signalReject;

		// Token: 0x04006F08 RID: 28424
		public Pawn pawn;

		// Token: 0x04006F09 RID: 28425
		public Pawn speaker;
	}
}
