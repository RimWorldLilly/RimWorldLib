using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200170E RID: 5902
	public class RitualRoleWarden : RitualRole
	{
		// Token: 0x06009386 RID: 37766 RVA: 0x0035D7E0 File Offset: 0x0035B9E0
		public override bool AppliesToPawn(Pawn p, out string reason, TargetInfo selectedTarget, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null, bool skipReason = false)
		{
			if (!base.AppliesIfChild(p, out reason, skipReason))
			{
				return false;
			}
			if (!p.Faction.IsPlayerSafe())
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustBeColonist".Translate(base.Label);
				}
				return false;
			}
			if (!p.RaceProps.Humanlike)
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustBeHumanlike".Translate(base.LabelCap);
				}
				return false;
			}
			if (!p.workSettings.WorkIsActive(WorkTypeDefOf.Warden))
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustBeCapableOfWardening".Translate(p);
				}
				return false;
			}
			if (p.WorkTagIsDisabled(WorkTags.Violent))
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustBeCapableOfViolence".Translate(p);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06009387 RID: 37767 RVA: 0x0035D8AF File Offset: 0x0035BAAF
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null, bool skipReason = false)
		{
			reason = null;
			return false;
		}
	}
}
