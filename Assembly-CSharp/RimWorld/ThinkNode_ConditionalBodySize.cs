using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D3E RID: 3390
	public class ThinkNode_ConditionalBodySize : ThinkNode_Conditional
	{
		// Token: 0x06005EF9 RID: 24313 RVA: 0x0022B730 File Offset: 0x00229930
		protected override bool Satisfied(Pawn pawn)
		{
			float bodySize = pawn.BodySize;
			return bodySize >= this.min && bodySize <= this.max;
		}

		// Token: 0x040035B9 RID: 13753
		public float min;

		// Token: 0x040035BA RID: 13754
		public float max = 99999f;
	}
}
