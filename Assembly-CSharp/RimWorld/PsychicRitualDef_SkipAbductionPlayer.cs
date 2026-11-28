using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C9F RID: 3231
	public class PsychicRitualDef_SkipAbductionPlayer : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x06005B86 RID: 23430 RVA: 0x0021A411 File Offset: 0x00218611
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, parent);
			list.Add(new PsychicRitualToil_SkipAbductionPlayer(this.InvokerRole));
			return list;
		}

		// Token: 0x06005B87 RID: 23431 RVA: 0x0021A42C File Offset: 0x0021862C
		public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
		{
			string value = Mathf.FloorToInt(this.comaDurationDaysFromQualityCurve.Evaluate(qualityRange.min) * 60000f).ToStringTicksToDays("F1");
			return this.outcomeDescription.Formatted(value);
		}

		// Token: 0x04003467 RID: 13415
		public SimpleCurve comaDurationDaysFromQualityCurve;
	}
}
