using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200152F RID: 5423
	public class PawnRelationWorker_CousinOnceRemoved : PawnRelationWorker
	{
		// Token: 0x060084C0 RID: 33984 RVA: 0x002FDC18 File Offset: 0x002FBE18
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Cousin.Worker;
			if ((other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather())))
			{
				return true;
			}
			PawnRelationWorker worker2 = PawnRelationDefOf.GranduncleOrGrandaunt.Worker;
			return (other.GetMother() != null && worker2.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker2.InRelation(me, other.GetFather()));
		}
	}
}
