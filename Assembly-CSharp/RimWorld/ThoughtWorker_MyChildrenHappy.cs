using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E7A RID: 3706
	public class ThoughtWorker_MyChildrenHappy : ThoughtWorker_MyChildHappy
	{
		// Token: 0x06006227 RID: 25127 RVA: 0x002350EA File Offset: 0x002332EA
		protected override bool Active(Pawn parent)
		{
			return base.ChildrenWithMoodCount(parent) > 1;
		}
	}
}
