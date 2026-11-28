using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200153A RID: 5434
	public class PawnRelationWorker_Kin : PawnRelationWorker
	{
		// Token: 0x060084DB RID: 34011 RVA: 0x002FE010 File Offset: 0x002FC210
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other || me.RaceProps.Animal != other.RaceProps.Animal)
			{
				return false;
			}
			IEnumerable<Pawn> familyByBlood = me.relations.FamilyByBlood;
			HashSet<Pawn> hashSet = familyByBlood as HashSet<Pawn>;
			if (hashSet == null)
			{
				return familyByBlood.Contains(other);
			}
			return hashSet.Contains(other);
		}
	}
}
