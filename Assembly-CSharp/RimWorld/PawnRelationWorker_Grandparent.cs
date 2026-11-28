using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001535 RID: 5429
	public class PawnRelationWorker_Grandparent : PawnRelationWorker
	{
		// Token: 0x060084D1 RID: 34001 RVA: 0x002FDECF File Offset: 0x002FC0CF
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && PawnRelationDefOf.Grandchild.Worker.InRelation(other, me);
		}
	}
}
