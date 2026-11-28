using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C9A RID: 3226
	public class PsychicRitualDef_NeurosisPulse : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x06005B75 RID: 23413 RVA: 0x0021A117 File Offset: 0x00218317
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph graph)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, graph);
			list.Add(new PsychicRitualToil_NeurosisPulse(this.InvokerRole));
			return list;
		}

		// Token: 0x06005B76 RID: 23414 RVA: 0x0021A132 File Offset: 0x00218332
		public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
		{
			return this.outcomeDescription.Formatted(Mathf.FloorToInt(this.durationDaysFromQualityCurve.Evaluate(qualityRange.min) * 60000f).ToStringTicksToDays("F1"));
		}

		// Token: 0x04003461 RID: 13409
		public SimpleCurve durationDaysFromQualityCurve;
	}
}
