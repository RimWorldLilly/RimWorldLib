using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FD3 RID: 4051
	public class FactionCurve
	{
		// Token: 0x060066C1 RID: 26305 RVA: 0x0024767B File Offset: 0x0024587B
		public float Evaluate(float x)
		{
			return this.selectionWeightPerPointsCurve.Evaluate(x);
		}

		// Token: 0x04003FDA RID: 16346
		public FactionDef faction;

		// Token: 0x04003FDB RID: 16347
		private SimpleCurve selectionWeightPerPointsCurve;
	}
}
