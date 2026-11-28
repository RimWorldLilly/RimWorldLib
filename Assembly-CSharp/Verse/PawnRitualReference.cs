using System;

namespace Verse
{
	// Token: 0x020000B6 RID: 182
	public class PawnRitualReference : IExposable
	{
		// Token: 0x06000759 RID: 1881 RVA: 0x0002A199 File Offset: 0x00028399
		public PawnRitualReference()
		{
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x0002A1A1 File Offset: 0x000283A1
		public PawnRitualReference(Pawn p)
		{
			this.pawn = p;
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x0002A1B0 File Offset: 0x000283B0
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
		}

		// Token: 0x040002CA RID: 714
		public Pawn pawn;
	}
}
