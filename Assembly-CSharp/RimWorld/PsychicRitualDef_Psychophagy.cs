using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C9D RID: 3229
	public class PsychicRitualDef_Psychophagy : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x06005B81 RID: 23425 RVA: 0x0021A2FF File Offset: 0x002184FF
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, parent);
			list.Add(new PsychicRitualToil_Psychophagy(this.InvokerRole, this.TargetRole, this.brainDamageRange));
			list.Add(new PsychicRitualToil_TargetCleanup(this.InvokerRole, this.TargetRole));
			return list;
		}

		// Token: 0x06005B82 RID: 23426 RVA: 0x0021A340 File Offset: 0x00218540
		public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
		{
			string value = Mathf.FloorToInt(this.effectDurationDaysFromQualityCurve.Evaluate(qualityRange.min) * 60000f).ToStringTicksToDays("F1");
			IntRange disappearsAfterTicks = HediffDefOf.DarkPsychicShock.CompProps<HediffCompProperties_Disappears>().disappearsAfterTicks;
			FloatRange floatRange = new FloatRange((float)Mathf.FloorToInt(disappearsAfterTicks.min.TicksToDays()), (float)Mathf.FloorToInt(disappearsAfterTicks.max.TicksToDays()));
			return this.outcomeDescription.Formatted(value, floatRange.ToString(), this.brainDamageRange.ToString());
		}

		// Token: 0x04003465 RID: 13413
		public FloatRange brainDamageRange;

		// Token: 0x04003466 RID: 13414
		public SimpleCurve effectDurationDaysFromQualityCurve;
	}
}
