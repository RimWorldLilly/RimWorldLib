using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001715 RID: 5909
	public class RitualRole_Mother : RitualRoleForced
	{
		// Token: 0x0600939D RID: 37789 RVA: 0x0035E03C File Offset: 0x0035C23C
		public override bool AppliesToPawn(Pawn p, out string reason, TargetInfo selectedTarget, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null, bool skipReason = false)
		{
			reason = null;
			if (base.AppliesToPawn(p, out reason, selectedTarget, ritual, assignments, precept, skipReason))
			{
				return true;
			}
			if (p.health.hediffSet.HasHediff(HediffDefOf.PregnancyLabor, false))
			{
				if (selectedTarget.IsValid && PregnancyUtility.IsUsableBedFor(p, p, (Building_Bed)selectedTarget.Thing))
				{
					return true;
				}
				reason = "CantUseThisBed".Translate();
			}
			else
			{
				reason = "NotInLabor".Translate();
			}
			return false;
		}
	}
}
