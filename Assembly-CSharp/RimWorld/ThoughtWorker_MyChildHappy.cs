using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E79 RID: 3705
	public class ThoughtWorker_MyChildHappy : ThoughtWorker_RelatedChildMoodBase
	{
		// Token: 0x06006222 RID: 25122 RVA: 0x002350B2 File Offset: 0x002332B2
		protected override FloatRange MoodRange()
		{
			return ThoughtWorker_MyChildHappy.HappyMoodRange;
		}

		// Token: 0x06006223 RID: 25123 RVA: 0x002350B9 File Offset: 0x002332B9
		protected override bool Active(Pawn parent)
		{
			return base.ChildrenWithMoodCount(parent) == 1;
		}

		// Token: 0x06006224 RID: 25124 RVA: 0x002350C5 File Offset: 0x002332C5
		protected override string SingleChildLabel()
		{
			return "MyChildIsHappy";
		}

		// Token: 0x0400361D RID: 13853
		public static readonly FloatRange HappyMoodRange = new FloatRange(0.6f, float.PositiveInfinity);
	}
}
