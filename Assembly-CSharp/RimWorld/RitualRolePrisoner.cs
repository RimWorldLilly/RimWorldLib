using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001708 RID: 5896
	public class RitualRolePrisoner : RitualRole
	{
		// Token: 0x06009370 RID: 37744 RVA: 0x0035D394 File Offset: 0x0035B594
		public override bool AppliesToPawn(Pawn p, out string reason, TargetInfo selectedTarget, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null, bool skipReason = false)
		{
			if (!base.AppliesIfChild(p, out reason, skipReason))
			{
				return false;
			}
			if (p.IsPrisonerOfColony)
			{
				return true;
			}
			if (!skipReason)
			{
				reason = "MessageRitualRoleMustBePrisoner".Translate(base.LabelCap);
			}
			return false;
		}

		// Token: 0x06009371 RID: 37745 RVA: 0x0035D3CF File Offset: 0x0035B5CF
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null, bool skipReason = false)
		{
			reason = null;
			return false;
		}
	}
}
