using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E7B RID: 3707
	public class ThoughtWorker_MyChildSad : ThoughtWorker_RelatedChildMoodBase
	{
		// Token: 0x06006229 RID: 25129 RVA: 0x002350FE File Offset: 0x002332FE
		protected override FloatRange MoodRange()
		{
			return ThoughtWorker_MyChildSad.SadMoodRange;
		}

		// Token: 0x0600622A RID: 25130 RVA: 0x00235105 File Offset: 0x00233305
		protected override bool Active(Pawn parent)
		{
			return base.ChildrenWithMoodCount(parent) == 1;
		}

		// Token: 0x0600622B RID: 25131 RVA: 0x00235111 File Offset: 0x00233311
		protected override string SingleChildLabel()
		{
			return "MyChildIsSad";
		}

		// Token: 0x0400361E RID: 13854
		public static readonly FloatRange SadMoodRange = new FloatRange(float.NegativeInfinity, 0.35f);
	}
}
