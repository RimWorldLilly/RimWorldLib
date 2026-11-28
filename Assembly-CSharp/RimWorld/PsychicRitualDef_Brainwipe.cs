using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C96 RID: 3222
	public class PsychicRitualDef_Brainwipe : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x06005B4F RID: 23375 RVA: 0x00219269 File Offset: 0x00217469
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, parent);
			list.Add(new PsychicRitualToil_Brainwipe(this.InvokerRole, this.TargetRole));
			list.Add(new PsychicRitualToil_TargetCleanup(this.InvokerRole, this.TargetRole));
			return list;
		}

		// Token: 0x06005B50 RID: 23376 RVA: 0x002192A1 File Offset: 0x002174A1
		public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
		{
			return this.outcomeDescription.Formatted(Mathf.FloorToInt(this.comaDurationDaysFromQualityCurve.Evaluate(qualityRange.min) * 60000f).ToStringTicksToDays("F1"));
		}

		// Token: 0x06005B51 RID: 23377 RVA: 0x002192D9 File Offset: 0x002174D9
		public override IEnumerable<string> GetPawnTooltipExtras(Pawn pawn)
		{
			PsychicRitualDef_Brainwipe.<GetPawnTooltipExtras>d__3 <GetPawnTooltipExtras>d__ = new PsychicRitualDef_Brainwipe.<GetPawnTooltipExtras>d__3(-2);
			<GetPawnTooltipExtras>d__.<>3__pawn = pawn;
			return <GetPawnTooltipExtras>d__;
		}

		// Token: 0x0400344D RID: 13389
		public SimpleCurve comaDurationDaysFromQualityCurve;
	}
}
