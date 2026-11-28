using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001536 RID: 5430
	public class PawnRelationWorker_GranduncleOrGrandaunt : PawnRelationWorker
	{
		// Token: 0x060084D3 RID: 34003 RVA: 0x002FDEF0 File Offset: 0x002FC0F0
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			if (PawnRelationDefOf.Grandparent.Worker.InRelation(me, other))
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.GreatGrandparent.Worker;
			return (other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather()));
		}
	}
}
