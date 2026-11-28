using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001538 RID: 5432
	public class PawnRelationWorker_GreatGrandparent : PawnRelationWorker
	{
		// Token: 0x060084D7 RID: 34007 RVA: 0x002FDFB3 File Offset: 0x002FC1B3
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && PawnRelationDefOf.GreatGrandchild.Worker.InRelation(other, me);
		}
	}
}
