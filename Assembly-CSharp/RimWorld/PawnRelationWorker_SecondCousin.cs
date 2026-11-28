using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001540 RID: 5440
	public class PawnRelationWorker_SecondCousin : PawnRelationWorker
	{
		// Token: 0x060084ED RID: 34029 RVA: 0x002FE51C File Offset: 0x002FC71C
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.GranduncleOrGrandaunt.Worker;
			Pawn mother = other.GetMother();
			if (mother != null && ((mother.GetMother() != null && worker.InRelation(me, mother.GetMother())) || (mother.GetFather() != null && worker.InRelation(me, mother.GetFather()))))
			{
				return true;
			}
			Pawn father = other.GetFather();
			return father != null && ((father.GetMother() != null && worker.InRelation(me, father.GetMother())) || (father.GetFather() != null && worker.InRelation(me, father.GetFather())));
		}
	}
}
