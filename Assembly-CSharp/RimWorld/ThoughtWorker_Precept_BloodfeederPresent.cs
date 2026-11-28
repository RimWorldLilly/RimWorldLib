using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DE0 RID: 3552
	public class ThoughtWorker_Precept_BloodfeederPresent : ThoughtWorker_Precept
	{
		// Token: 0x060060A0 RID: 24736 RVA: 0x0022FE9C File Offset: 0x0022E09C
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.BiotechActive || !ModsConfig.IdeologyActive || p.IsBloodfeeder())
			{
				return ThoughtState.Inactive;
			}
			foreach (Pawn pawn in p.MapHeld.mapPawns.AllPawnsSpawned)
			{
				if (pawn.IsBloodfeeder() && (pawn.IsPrisonerOfColony || pawn.IsSlaveOfColony || pawn.IsColonist))
				{
					return ThoughtState.ActiveDefault;
				}
			}
			return ThoughtState.Inactive;
		}
	}
}
