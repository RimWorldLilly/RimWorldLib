using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C9B RID: 3227
	public class PsychicRitualDef_Philophagy : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x06005B78 RID: 23416 RVA: 0x0021A172 File Offset: 0x00218372
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, parent);
			list.Add(new PsychicRitualToil_Philophagy(this.InvokerRole, this.TargetRole, this.brainDamageRange));
			list.Add(new PsychicRitualToil_TargetCleanup(this.InvokerRole, this.TargetRole));
			return list;
		}

		// Token: 0x06005B79 RID: 23417 RVA: 0x0021A1B0 File Offset: 0x002183B0
		public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
		{
			float num = this.xpTransferFromQualityCurve.Evaluate(qualityRange.min);
			string value = num.ToStringPercent();
			Pawn pawn = assignments.FirstAssignedPawn(this.InvokerRole);
			Pawn pawn2 = assignments.FirstAssignedPawn(this.TargetRole);
			TaggedString taggedString = this.outcomeDescription.Formatted(value);
			if (pawn == null || pawn2 == null)
			{
				return taggedString;
			}
			float value2;
			SkillDef philophagySkillAndXpTransfer = PsychicRitualUtility.GetPhilophagySkillAndXpTransfer(pawn, pawn2, num, out value2);
			if (philophagySkillAndXpTransfer != null)
			{
				taggedString += "\n\n" + "PhilophagyTransferReport".Translate(pawn.Named("INVOKER"), value2, philophagySkillAndXpTransfer.Named("SKILL"), pawn2.Named("TARGET"));
			}
			return taggedString;
		}

		// Token: 0x06005B7A RID: 23418 RVA: 0x0021A264 File Offset: 0x00218464
		public override IEnumerable<string> BlockingIssues(PsychicRitualRoleAssignments assignments, Map map)
		{
			PsychicRitualDef_Philophagy.<BlockingIssues>d__4 <BlockingIssues>d__ = new PsychicRitualDef_Philophagy.<BlockingIssues>d__4(-2);
			<BlockingIssues>d__.<>4__this = this;
			<BlockingIssues>d__.<>3__assignments = assignments;
			<BlockingIssues>d__.<>3__map = map;
			return <BlockingIssues>d__;
		}

		// Token: 0x06005B7B RID: 23419 RVA: 0x0021A282 File Offset: 0x00218482
		public override IEnumerable<string> GetPawnTooltipExtras(Pawn pawn)
		{
			PsychicRitualDef_Philophagy.<GetPawnTooltipExtras>d__5 <GetPawnTooltipExtras>d__ = new PsychicRitualDef_Philophagy.<GetPawnTooltipExtras>d__5(-2);
			<GetPawnTooltipExtras>d__.<>3__pawn = pawn;
			return <GetPawnTooltipExtras>d__;
		}

		// Token: 0x04003462 RID: 13410
		public FloatRange brainDamageRange;

		// Token: 0x04003463 RID: 13411
		public SimpleCurve xpTransferFromQualityCurve;
	}
}
