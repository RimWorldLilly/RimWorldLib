using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200153F RID: 5439
	public class PawnRelationWorker_ParentInLaw : PawnRelationWorker
	{
		// Token: 0x060084EB RID: 34027 RVA: 0x002FE48C File Offset: 0x002FC68C
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			if (me.GetSpouseCount(true) == 0)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Parent.Worker;
			if (worker.InRelation(me, other))
			{
				return false;
			}
			foreach (Pawn me2 in me.GetSpouses(true))
			{
				if (worker.InRelation(me2, other))
				{
					return true;
				}
			}
			return false;
		}
	}
}
