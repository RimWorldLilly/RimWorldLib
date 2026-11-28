using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200170B RID: 5899
	public class RitualRoleForced : RitualRole
	{
		// Token: 0x0600937A RID: 37754 RVA: 0x0035D566 File Offset: 0x0035B766
		public override bool AppliesToPawn(Pawn p, out string reason, TargetInfo selectedTarget, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null, bool skipReason = false)
		{
			reason = null;
			return ritual != null && base.AppliesIfChild(p, out reason, skipReason) && ritual.assignments.ForcedRole(p).id == this.id;
		}

		// Token: 0x0600937B RID: 37755 RVA: 0x0035D59C File Offset: 0x0035B79C
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null, bool skipReason = false)
		{
			reason = null;
			return false;
		}
	}
}
