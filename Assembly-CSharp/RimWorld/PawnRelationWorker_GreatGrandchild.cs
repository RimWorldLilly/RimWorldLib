using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001537 RID: 5431
	public class PawnRelationWorker_GreatGrandchild : PawnRelationWorker
	{
		// Token: 0x060084D5 RID: 34005 RVA: 0x002FDF5C File Offset: 0x002FC15C
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Grandchild.Worker;
			return (other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather()));
		}
	}
}
