using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200152E RID: 5422
	public class PawnRelationWorker_Cousin : PawnRelationWorker
	{
		// Token: 0x060084BE RID: 33982 RVA: 0x002FDBC0 File Offset: 0x002FBDC0
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.UncleOrAunt.Worker;
			return (other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather()));
		}
	}
}
