using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C95 RID: 3221
	public class PsychicRitualDef_BloodRain : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x06005B4C RID: 23372 RVA: 0x00219208 File Offset: 0x00217408
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph graph)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, graph);
			list.Add(new PsychicRitualToil_BloodRain(this.InvokerRole, this.durationHoursFromQualityRange));
			return list;
		}

		// Token: 0x06005B4D RID: 23373 RVA: 0x00219229 File Offset: 0x00217429
		public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
		{
			return this.outcomeDescription.Formatted(Mathf.FloorToInt(this.durationHoursFromQualityRange.LerpThroughRange(qualityRange.min) * 2500f).ToStringTicksToPeriod(true, false, true, true, false));
		}

		// Token: 0x0400344C RID: 13388
		private FloatRange durationHoursFromQualityRange;
	}
}
