using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001081 RID: 4225
	public class QuestPart_GiveHediff : QuestPart
	{
		// Token: 0x060069DD RID: 27101 RVA: 0x002549BC File Offset: 0x00252BBC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal && !this.aboutPawn.DestroyedOrNull())
			{
				this.aboutPawn.health.AddHediff(this.hediff, null, null, null);
			}
		}

		// Token: 0x060069DE RID: 27102 RVA: 0x00254A0B File Offset: 0x00252C0B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.aboutPawn, "aboutPawn", false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Defs.Look<HediffDef>(ref this.hediff, "hediff");
		}

		// Token: 0x0400434C RID: 17228
		public Pawn aboutPawn;

		// Token: 0x0400434D RID: 17229
		public string inSignal;

		// Token: 0x0400434E RID: 17230
		public HediffDef hediff;
	}
}
