using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001533 RID: 5427
	public class PawnRelationWorker_Grandchild : PawnRelationWorker
	{
		// Token: 0x060084CD RID: 33997 RVA: 0x002FDE20 File Offset: 0x002FC020
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Child.Worker;
			return (other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather()));
		}
	}
}
