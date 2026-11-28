using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C98 RID: 3224
	public class PsychicRitualDef_ImbueDeathRefusal : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x06005B59 RID: 23385 RVA: 0x00219487 File Offset: 0x00217687
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, parent);
			list.Add(new PsychicRitualToil_ImbueDeathRefusal(this.TargetRole));
			list.Add(new PsychicRitualToil_TargetCleanup(this.InvokerRole, this.TargetRole));
			return list;
		}

		// Token: 0x06005B5A RID: 23386 RVA: 0x002194B9 File Offset: 0x002176B9
		public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
		{
			return this.outcomeDescription.Formatted(Mathf.Abs(this.skillOffsetPercentFromQualityCurve.Evaluate(qualityRange.min)).ToStringPercent());
		}

		// Token: 0x04003451 RID: 13393
		public SimpleCurve skillOffsetPercentFromQualityCurve;
	}
}
