using System;

namespace Verse
{
	// Token: 0x0200019E RID: 414
	public class StartingHediff
	{
		// Token: 0x06000B7E RID: 2942 RVA: 0x0003DC6B File Offset: 0x0003BE6B
		public bool HasHediff(Pawn p)
		{
			return p.health.hediffSet.HasHediff(this.def, false);
		}

		// Token: 0x04000A03 RID: 2563
		public HediffDef def;

		// Token: 0x04000A04 RID: 2564
		public float? severity;

		// Token: 0x04000A05 RID: 2565
		public float? chance;

		// Token: 0x04000A06 RID: 2566
		public IntRange? durationTicksRange;
	}
}
