using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001710 RID: 5904
	public class RitualRoleColonist : RitualRole
	{
		// Token: 0x0600938D RID: 37773 RVA: 0x0035DA58 File Offset: 0x0035BC58
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
			if (this.requiredWorkType != null && p.WorkTypeIsDisabled(this.requiredWorkType))
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustBeCapableOfGeneric".Translate(base.LabelCap, this.requiredWorkType.gerundLabel);
				}
				return false;
			}
			if (this.usedSkill != null && p.skills.GetSkill(this.usedSkill).TotallyDisabled)
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustBeCapableOfGeneric".Translate(base.LabelCap, this.usedSkill.label);
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
			if (p.GuestStatus.GetValueOrDefault() == GuestStatus.Prisoner)
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustBeFree".Translate(base.Label);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600938E RID: 37774 RVA: 0x0035DB9F File Offset: 0x0035BD9F
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null, bool skipReason = false)
		{
			reason = null;
			return false;
		}

		// Token: 0x0600938F RID: 37775 RVA: 0x0035DBA8 File Offset: 0x0035BDA8
		protected override int PawnDesirability(Pawn pawn)
		{
			int num = base.PawnDesirability(pawn);
			if (this.usedStat != null)
			{
				if (!this.usedStat.Worker.IsDisabledFor(pawn))
				{
					num += Mathf.RoundToInt(100f * pawn.GetStatValue(this.usedStat, true, -1));
				}
			}
			else if (this.usedSkill != null)
			{
				num += 2 * pawn.skills.GetSkill(this.usedSkill).Level;
			}
			return num;
		}

		// Token: 0x06009390 RID: 37776 RVA: 0x0035DC1A File Offset: 0x0035BE1A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<WorkTypeDef>(ref this.requiredWorkType, "requiredWorkType");
		}

		// Token: 0x0400593D RID: 22845
		public WorkTypeDef requiredWorkType;

		// Token: 0x0400593E RID: 22846
		public SkillDef usedSkill;

		// Token: 0x0400593F RID: 22847
		public StatDef usedStat;
	}
}
