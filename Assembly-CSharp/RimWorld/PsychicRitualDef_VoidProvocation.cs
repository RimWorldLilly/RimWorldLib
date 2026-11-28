using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CA5 RID: 3237
	public class PsychicRitualDef_VoidProvocation : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x06005B9D RID: 23453 RVA: 0x0021A6CD File Offset: 0x002188CD
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph graph)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, graph);
			list.Add(new PsychicRitualToil_VoidProvocation(this.InvokerRole, this.psychicShockChanceFromQualityCurve));
			return list;
		}

		// Token: 0x06005B9E RID: 23454 RVA: 0x0021A6EE File Offset: 0x002188EE
		public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
		{
			return this.outcomeDescription.Formatted(this.psychicShockChanceFromQualityCurve.Evaluate(qualityRange.min).ToStringPercent());
		}

		// Token: 0x06005B9F RID: 23455 RVA: 0x0021A716 File Offset: 0x00218916
		public override IEnumerable<string> ConfigErrors()
		{
			PsychicRitualDef_VoidProvocation.<ConfigErrors>d__5 <ConfigErrors>d__ = new PsychicRitualDef_VoidProvocation.<ConfigErrors>d__5(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x0400346F RID: 13423
		public SimpleCurve psychicShockChanceFromQualityCurve;

		// Token: 0x04003470 RID: 13424
		public FloatRange darkPsychicShockDurarionHoursRange;

		// Token: 0x04003471 RID: 13425
		public FloatRange incidentDelayHoursRange;
	}
}
