using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200170D RID: 5901
	public class RitualRoleScarificationTarget : RitualRole
	{
		// Token: 0x06009382 RID: 37762 RVA: 0x0035D68C File Offset: 0x0035B88C
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
					reason = "MessageRitualRoleMustBeHumanlike".Translate(base.LabelCap);
				}
				return false;
			}
			if (p.Ideo == null || p.Ideo.RequiredScars <= p.health.hediffSet.GetHediffCount(HediffDefOf.Scarification))
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustRequireScarification".Translate(p);
				}
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
			return true;
		}

		// Token: 0x06009383 RID: 37763 RVA: 0x0035D74D File Offset: 0x0035B94D
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null, bool skipReason = false)
		{
			reason = null;
			return true;
		}

		// Token: 0x06009384 RID: 37764 RVA: 0x0035D754 File Offset: 0x0035B954
		public override string ExtraInfoForDialog(IEnumerable<Pawn> selected)
		{
			Pawn pawn = selected.FirstOrDefault<Pawn>();
			if (pawn != null)
			{
				if (pawn.Ideo == null || pawn.Ideo.RequiredScars <= 0)
				{
					return "RitualScarificationWarnNonScarificationIdeo".Translate(pawn.Named("PAWN"));
				}
				if (pawn.story != null && pawn.story.traits.HasTrait(TraitDefOf.Wimp))
				{
					return "RitualScarificationWarnWimp".Translate(pawn.Named("PAWN"));
				}
			}
			return null;
		}
	}
}
