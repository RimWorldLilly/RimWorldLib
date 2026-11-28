using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001085 RID: 4229
	public class QuestPart_LinkUnnaturalCorpse : QuestPart
	{
		// Token: 0x060069F6 RID: 27126 RVA: 0x002550C5 File Offset: 0x002532C5
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal && !this.aboutPawn.DestroyedOrNull())
			{
				Find.Anomaly.RegisterUnnaturalCorpse(this.aboutPawn, this.corpse);
			}
		}

		// Token: 0x060069F7 RID: 27127 RVA: 0x002550FD File Offset: 0x002532FD
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.aboutPawn, "aboutPawn", false);
			Scribe_References.Look<UnnaturalCorpse>(ref this.corpse, "corpse", false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
		}

		// Token: 0x0400435B RID: 17243
		public Pawn aboutPawn;

		// Token: 0x0400435C RID: 17244
		public UnnaturalCorpse corpse;

		// Token: 0x0400435D RID: 17245
		public string inSignal;
	}
}
