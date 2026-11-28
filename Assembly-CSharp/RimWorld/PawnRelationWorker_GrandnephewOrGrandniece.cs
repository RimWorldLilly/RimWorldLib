using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001534 RID: 5428
	public class PawnRelationWorker_GrandnephewOrGrandniece : PawnRelationWorker
	{
		// Token: 0x060084CF RID: 33999 RVA: 0x002FDE78 File Offset: 0x002FC078
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.NephewOrNiece.Worker;
			return (other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather()));
		}
	}
}
