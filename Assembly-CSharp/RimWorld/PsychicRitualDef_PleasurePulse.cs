using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C9C RID: 3228
	public class PsychicRitualDef_PleasurePulse : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x06005B7E RID: 23422 RVA: 0x0021A2A4 File Offset: 0x002184A4
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph graph)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, graph);
			list.Add(new PsychicRitualToil_PleasurePulse(this.InvokerRole));
			return list;
		}

		// Token: 0x06005B7F RID: 23423 RVA: 0x0021A2BF File Offset: 0x002184BF
		public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
		{
			return this.outcomeDescription.Formatted(Mathf.FloorToInt(this.durationDaysFromQualityCurve.Evaluate(qualityRange.min) * 60000f).ToStringTicksToDays("F1"));
		}

		// Token: 0x04003464 RID: 13412
		public SimpleCurve durationDaysFromQualityCurve;
	}
}
