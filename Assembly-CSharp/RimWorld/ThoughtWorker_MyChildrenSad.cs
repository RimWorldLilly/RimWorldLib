using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E7C RID: 3708
	public class ThoughtWorker_MyChildrenSad : ThoughtWorker_MyChildSad
	{
		// Token: 0x0600622E RID: 25134 RVA: 0x00235136 File Offset: 0x00233336
		protected override bool Active(Pawn parent)
		{
			return base.ChildrenWithMoodCount(parent) > 1;
		}
	}
}
