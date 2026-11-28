using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001709 RID: 5897
	public class RitualRolePrisonerOrSlave : RitualRole
	{
		// Token: 0x06009373 RID: 37747 RVA: 0x0035D3E0 File Offset: 0x0035B5E0
		public override bool AppliesToPawn(Pawn p, out string reason, TargetInfo selectedTarget, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null, bool skipReason = false)
		{
			if (!base.AppliesIfChild(p, out reason, skipReason))
			{
				return false;
			}
			if (this.disallowWildManPrisoner && p.IsWildMan() && p.IsPrisoner)
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustNotBeImprisonedWildMan".Translate(base.LabelCap);
				}
				return false;
			}
			if (!p.IsPrisonerOfColony && !p.IsSlaveOfColony)
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustBePrisonerOrSlave".Translate(base.LabelCap);
				}
				return false;
			}
			if (this.mustBeCapableToFight && (p.WorkTagIsDisabled(WorkTags.Violent) || !p.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation)))
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustBeCapableOfFighting".Translate(p);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06009374 RID: 37748 RVA: 0x0035D4AD File Offset: 0x0035B6AD
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null, bool skipReason = false)
		{
			reason = null;
			return false;
		}

		// Token: 0x06009375 RID: 37749 RVA: 0x0035D4B3 File Offset: 0x0035B6B3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.mustBeCapableToFight, "mustBeCapableToFight", false, false);
			Scribe_Values.Look<bool>(ref this.disallowWildManPrisoner, "disallowWildManPrisoner", false, false);
		}

		// Token: 0x0400593A RID: 22842
		public bool mustBeCapableToFight;

		// Token: 0x0400593B RID: 22843
		public bool disallowWildManPrisoner = true;
	}
}
