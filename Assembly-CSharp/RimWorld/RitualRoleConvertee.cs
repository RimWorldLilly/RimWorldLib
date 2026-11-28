using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001712 RID: 5906
	public class RitualRoleConvertee : RitualRole
	{
		// Token: 0x06009394 RID: 37780 RVA: 0x0035DD74 File Offset: 0x0035BF74
		public override bool AppliesToPawn(Pawn p, out string reason, TargetInfo selectedTarget, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null, bool skipReason = false)
		{
			if (!base.AppliesIfChild(p, out reason, skipReason))
			{
				return false;
			}
			if (!p.RaceProps.Humanlike)
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustBeHumanlike".Translate(base.Label);
				}
				return false;
			}
			if ((assignments == null || !assignments.Forced(p)) && p.Ideo == ((assignments != null) ? assignments.Ritual.ideo : null))
			{
				if (!skipReason)
				{
					reason = "MessageRitualPawnIsAlreadyBelievingIdeo".Translate(p, Find.ActiveLanguageWorker.WithIndefiniteArticle(p.Ideo.memberName, p.gender, false, false));
				}
				return false;
			}
			if (p.DevelopmentalStage.Baby())
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleCannotBeABaby".Translate(base.Label);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06009395 RID: 37781 RVA: 0x0035DE56 File Offset: 0x0035C056
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null, bool skipReason = false)
		{
			reason = null;
			return false;
		}
	}
}
