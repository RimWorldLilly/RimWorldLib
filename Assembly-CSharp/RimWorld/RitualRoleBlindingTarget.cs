using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200170F RID: 5903
	public class RitualRoleBlindingTarget : RitualRole
	{
		// Token: 0x06009389 RID: 37769 RVA: 0x0035D8C0 File Offset: 0x0035BAC0
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
			if (p.Ideo != null)
			{
				if (p.Ideo.PreceptsListForReading.Any((Precept pc) => pc.def.issue == IssueDefOf.Blindness))
				{
					if (p.health != null)
					{
						if (p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Any((BodyPartRecord part) => part.def == BodyPartDefOf.Eye))
						{
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
					}
					if (!skipReason)
					{
						reason = "MessageRitualRoleMustHaveEyes".Translate(base.LabelCap);
					}
					return false;
				}
			}
			if (!skipReason)
			{
				reason = "MessageRitualRoleMustRequireBlinding".Translate(p);
			}
			return false;
		}

		// Token: 0x0600938A RID: 37770 RVA: 0x0035D9F4 File Offset: 0x0035BBF4
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null, bool skipReason = false)
		{
			reason = null;
			return false;
		}

		// Token: 0x0600938B RID: 37771 RVA: 0x0035D9FC File Offset: 0x0035BBFC
		public override string ExtraInfoForDialog(IEnumerable<Pawn> selected)
		{
			Pawn pawn = selected.FirstOrDefault<Pawn>();
			if (pawn != null && pawn.story != null && pawn.story.traits.HasTrait(TraitDefOf.Wimp))
			{
				return "RitualBlindingWarnWimp".Translate(pawn.Named("PAWN"));
			}
			return null;
		}
	}
}
