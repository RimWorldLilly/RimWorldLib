using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001545 RID: 5445
	public class PawnRelationWorker_UncleOrAunt : PawnRelationWorker
	{
		// Token: 0x060084FD RID: 34045 RVA: 0x002FED08 File Offset: 0x002FCF08
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			if (PawnRelationDefOf.Parent.Worker.InRelation(me, other))
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Grandparent.Worker;
			return (me.GetMother() != null && (other.HasSameMother(me.GetMother()) || other.HasSameFather(me.GetMother()))) || (me.GetFather() != null && (other.HasSameMother(me.GetFather()) || other.HasSameFather(me.GetFather())));
		}
	}
}
