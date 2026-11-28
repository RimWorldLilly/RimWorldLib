using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001539 RID: 5433
	public class PawnRelationWorker_HalfSibling : PawnRelationWorker
	{
		// Token: 0x060084D9 RID: 34009 RVA: 0x002FDFD4 File Offset: 0x002FC1D4
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && !PawnRelationDefOf.Sibling.Worker.InRelation(me, other) && (me.HasSameMother(other) || me.HasSameFather(other));
		}
	}
}
