using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C97 RID: 3223
	public class PsychicRitualDef_Chronophagy : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x06005B53 RID: 23379 RVA: 0x002192F1 File Offset: 0x002174F1
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, parent);
			list.Add(new PsychicRitualToil_Chronophagy(this.InvokerRole, this.TargetRole, this.yearsTransferredFromQualityRange));
			list.Add(new PsychicRitualToil_TargetCleanup(this.InvokerRole, this.TargetRole));
			return list;
		}

		// Token: 0x06005B54 RID: 23380 RVA: 0x0021932F File Offset: 0x0021752F
		public override IEnumerable<string> BlockingIssues(PsychicRitualRoleAssignments assignments, Map map)
		{
			PsychicRitualDef_Chronophagy.<BlockingIssues>d__4 <BlockingIssues>d__ = new PsychicRitualDef_Chronophagy.<BlockingIssues>d__4(-2);
			<BlockingIssues>d__.<>4__this = this;
			<BlockingIssues>d__.<>3__assignments = assignments;
			<BlockingIssues>d__.<>3__map = map;
			return <BlockingIssues>d__;
		}

		// Token: 0x06005B55 RID: 23381 RVA: 0x00219350 File Offset: 0x00217550
		public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
		{
			float num = this.yearsTransferredFromQualityRange.LerpThroughRange(qualityRange.min);
			Pawn pawn = assignments.FirstAssignedPawn(this.invokerRole);
			Pawn pawn2 = assignments.FirstAssignedPawn(this.targetRole);
			if (pawn != null)
			{
				float num2 = Mathf.Max(pawn.ageTracker.AgeBiologicalYearsFloat - num, 13f);
				float num3 = pawn.ageTracker.AgeBiologicalYearsFloat - num2;
				TaggedString taggedString = this.outcomeDescription.Formatted(num3, this.yearsTransferredFromQualityRange.LerpThroughRange(qualityRange.min));
				if (pawn2 != null)
				{
					taggedString += "\n\n" + "CronophagyAgeChange".Translate(pawn.Named("INVOKER"), pawn.ageTracker.AgeBiologicalYearsFloat - num3, pawn2.Named("TARGET"), pawn2.ageTracker.AgeBiologicalYearsFloat + this.yearsTransferredFromQualityRange.LerpThroughRange(qualityRange.min));
				}
				return taggedString;
			}
			return this.outcomeDescription.Formatted(num, num);
		}

		// Token: 0x06005B56 RID: 23382 RVA: 0x00219465 File Offset: 0x00217665
		public override IEnumerable<string> GetPawnTooltipExtras(Pawn pawn)
		{
			PsychicRitualDef_Chronophagy.<GetPawnTooltipExtras>d__6 <GetPawnTooltipExtras>d__ = new PsychicRitualDef_Chronophagy.<GetPawnTooltipExtras>d__6(-2);
			<GetPawnTooltipExtras>d__.<>3__pawn = pawn;
			return <GetPawnTooltipExtras>d__;
		}

		// Token: 0x0400344E RID: 13390
		public FloatRange yearsTransferredFromQualityRange;

		// Token: 0x0400344F RID: 13391
		public SimpleCurve brainDamageFromAgeCurve;

		// Token: 0x04003450 RID: 13392
		public const int MinimumAge = 13;
	}
}
