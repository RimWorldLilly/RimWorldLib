using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DB6 RID: 3510
	public class ThoughtWorker_Precept_Slavery_NoSlavesInColony : ThoughtWorker_Precept
	{
		// Token: 0x06006023 RID: 24611 RVA: 0x0022E6AA File Offset: 0x0022C8AA
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return p.IsColonist && !p.IsSlave && !p.IsPrisoner && FactionUtility.GetSlavesInFactionCount(p.Faction) <= 0;
		}
	}
}
