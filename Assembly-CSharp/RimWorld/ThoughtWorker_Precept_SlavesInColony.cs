using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DB5 RID: 3509
	public class ThoughtWorker_Precept_SlavesInColony : ThoughtWorker_Precept
	{
		// Token: 0x06006021 RID: 24609 RVA: 0x0022E672 File Offset: 0x0022C872
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return p.IsColonist && !p.IsSlave && !p.IsPrisoner && FactionUtility.GetSlavesInFactionCount(p.Faction) > 0;
		}
	}
}
