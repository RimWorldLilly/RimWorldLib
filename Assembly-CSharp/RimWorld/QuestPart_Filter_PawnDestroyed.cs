using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001076 RID: 4214
	public class QuestPart_Filter_PawnDestroyed : QuestPart_Filter
	{
		// Token: 0x060069A6 RID: 27046 RVA: 0x00253C27 File Offset: 0x00251E27
		protected override bool Pass(SignalArgs args)
		{
			return this.pawn.DestroyedOrNull();
		}

		// Token: 0x060069A7 RID: 27047 RVA: 0x00253C34 File Offset: 0x00251E34
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
		}

		// Token: 0x0400432A RID: 17194
		public Pawn pawn;
	}
}
