using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001544 RID: 5444
	public class PawnRelationWorker_Stepparent : PawnRelationWorker
	{
		// Token: 0x060084FB RID: 34043 RVA: 0x002FEC98 File Offset: 0x002FCE98
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
			PawnRelationWorker worker = PawnRelationDefOf.Spouse.Worker;
			return (me.GetMother() != null && worker.InRelation(me.GetMother(), other)) || (me.GetFather() != null && worker.InRelation(me.GetFather(), other));
		}
	}
}
