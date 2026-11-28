using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200153C RID: 5436
	public class PawnRelationWorker_NephewOrNiece : PawnRelationWorker
	{
		// Token: 0x060084E0 RID: 34016 RVA: 0x002FE0AC File Offset: 0x002FC2AC
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			if (PawnRelationDefOf.Child.Worker.InRelation(me, other))
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Sibling.Worker;
			PawnRelationWorker worker2 = PawnRelationDefOf.HalfSibling.Worker;
			return (other.GetMother() != null && (worker.InRelation(me, other.GetMother()) || worker2.InRelation(me, other.GetMother()))) || (other.GetFather() != null && (worker.InRelation(me, other.GetFather()) || worker2.InRelation(me, other.GetFather())));
		}
	}
}
