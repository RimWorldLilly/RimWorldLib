using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200170A RID: 5898
	public class RitualRoleOrganizer : RitualRole
	{
		// Token: 0x06009377 RID: 37751 RVA: 0x0035D4F0 File Offset: 0x0035B6F0
		public override bool AppliesToPawn(Pawn p, out string reason, TargetInfo selectedTarget, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null, bool skipReason = false)
		{
			if (!base.AppliesIfChild(p, out reason, skipReason))
			{
				return false;
			}
			if (ritual == null)
			{
				if (assignments == null || !assignments.Required(p))
				{
					if (!skipReason)
					{
						reason = "MessageRitualRoleCannotReplaceRequiredPawn".Translate();
					}
					return false;
				}
				return true;
			}
			else
			{
				if (p != ritual.Organizer)
				{
					if (!skipReason)
					{
						reason = "MessageRitualRoleCannotReplaceRequiredPawn".Translate();
					}
					return false;
				}
				return true;
			}
		}

		// Token: 0x06009378 RID: 37752 RVA: 0x0035D558 File Offset: 0x0035B758
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null, bool skipReason = false)
		{
			reason = null;
			return false;
		}
	}
}
