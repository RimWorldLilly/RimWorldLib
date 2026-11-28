using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200152D RID: 5421
	public class PawnRelationWorker_ChildInLaw : PawnRelationWorker
	{
		// Token: 0x060084BC RID: 33980 RVA: 0x002FDB30 File Offset: 0x002FBD30
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			if (other.GetSpouseCount(true) == 0)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Child.Worker;
			if (worker.InRelation(me, other))
			{
				return false;
			}
			foreach (Pawn other2 in other.GetSpouses(true))
			{
				if (worker.InRelation(me, other2))
				{
					return true;
				}
			}
			return false;
		}
	}
}
