using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001080 RID: 4224
	public class QuestPart_GiveDiedOrDownedThoughts : QuestPart
	{
		// Token: 0x060069DA RID: 27098 RVA: 0x0025493C File Offset: 0x00252B3C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal)
			{
				PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(this.aboutPawn, null, this.thoughtsKind);
			}
		}

		// Token: 0x060069DB RID: 27099 RVA: 0x00254976 File Offset: 0x00252B76
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.aboutPawn, "aboutPawn", false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<PawnDiedOrDownedThoughtsKind>(ref this.thoughtsKind, "thoughtsKind", PawnDiedOrDownedThoughtsKind.Died, false);
		}

		// Token: 0x04004349 RID: 17225
		public Pawn aboutPawn;

		// Token: 0x0400434A RID: 17226
		public string inSignal;

		// Token: 0x0400434B RID: 17227
		public PawnDiedOrDownedThoughtsKind thoughtsKind;
	}
}
