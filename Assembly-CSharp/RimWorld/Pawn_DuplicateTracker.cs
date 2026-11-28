using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001596 RID: 5526
	public class Pawn_DuplicateTracker : IExposable
	{
		// Token: 0x060087CF RID: 34767 RVA: 0x003118D5 File Offset: 0x0030FAD5
		public Pawn_DuplicateTracker()
		{
		}

		// Token: 0x060087D0 RID: 34768 RVA: 0x003118E8 File Offset: 0x0030FAE8
		public Pawn_DuplicateTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060087D1 RID: 34769 RVA: 0x00311902 File Offset: 0x0030FB02
		public void Notify_PawnKilled()
		{
			Find.PawnDuplicator.RemoveDuplicate(this.duplicateOf, this.pawn);
		}

		// Token: 0x060087D2 RID: 34770 RVA: 0x0031191A File Offset: 0x0030FB1A
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.duplicateOf, "duplicateOf", int.MinValue, false);
		}

		// Token: 0x040053D7 RID: 21463
		private Pawn pawn;

		// Token: 0x040053D8 RID: 21464
		public int duplicateOf = int.MinValue;
	}
}
