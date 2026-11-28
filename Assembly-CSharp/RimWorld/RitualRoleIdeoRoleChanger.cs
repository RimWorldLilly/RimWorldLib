using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001714 RID: 5908
	public class RitualRoleIdeoRoleChanger : RitualRole
	{
		// Token: 0x0600939A RID: 37786 RVA: 0x0035DF34 File Offset: 0x0035C134
		public override bool AppliesToPawn(Pawn p, out string reason, TargetInfo selectedTarget, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null, bool skipReason = false)
		{
			if (!base.AppliesIfChild(p, out reason, skipReason))
			{
				return false;
			}
			if (p.Ideo == null)
			{
				return false;
			}
			if (!p.IsFreeNonSlaveColonist)
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustBeFreeColonist".Translate(base.Label);
				}
				return false;
			}
			if (p.Ideo.GetRole(p) == null && !RitualUtility.AllRolesForPawn(p).Any((Precept_Role r) => r.RequirementsMet(p)))
			{
				reason = "MessageRitualNoRolesAvailable".Translate(p);
				return false;
			}
			if (!Faction.OfPlayer.ideos.Has(p.Ideo))
			{
				reason = "MessageRitualNotOfPlayerIdeo".Translate(p);
				return false;
			}
			return true;
		}

		// Token: 0x0600939B RID: 37787 RVA: 0x0035E02E File Offset: 0x0035C22E
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null, bool skipReason = false)
		{
			reason = null;
			return false;
		}
	}
}
