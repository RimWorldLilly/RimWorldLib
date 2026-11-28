using System;
using RimWorld;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x02000920 RID: 2336
	public class PsychicRitualToil_BloodRain : PsychicRitualToil
	{
		// Token: 0x06004775 RID: 18293 RVA: 0x001BFC3E File Offset: 0x001BDE3E
		protected PsychicRitualToil_BloodRain()
		{
		}

		// Token: 0x06004776 RID: 18294 RVA: 0x001BFC46 File Offset: 0x001BDE46
		public PsychicRitualToil_BloodRain(PsychicRitualRoleDef invokerRole, FloatRange durationHoursFromQualityRange)
		{
			this.invokerRole = invokerRole;
			this.durationHoursFromQualityRange = durationHoursFromQualityRange;
		}

		// Token: 0x06004777 RID: 18295 RVA: 0x001BFC5C File Offset: 0x001BDE5C
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			base.Start(psychicRitual, parent);
			Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
			float duration = this.durationHoursFromQualityRange.LerpThroughRange(psychicRitual.PowerPercent);
			psychicRitual.ReleaseAllPawnsAndBuildings();
			if (pawn != null)
			{
				this.ApplyOutcome(psychicRitual, pawn, duration);
			}
		}

		// Token: 0x06004778 RID: 18296 RVA: 0x001BFCA8 File Offset: 0x001BDEA8
		private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker, float duration)
		{
			if (!(invoker.Map.gameConditionManager.GetActiveCondition(GameConditionDefOf.BloodRain) is GameCondition_BloodRain))
			{
				GameCondition_BloodRain gameCondition_BloodRain = (GameCondition_BloodRain)GameConditionMaker.MakeCondition(GameConditionDefOf.BloodRain, Mathf.FloorToInt(duration * 2500f));
				gameCondition_BloodRain.psychicRitualDef = psychicRitual.def;
				invoker.Map.GameConditionManager.RegisterCondition(gameCondition_BloodRain);
			}
			Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label), "BloodRainCompleteText".Translate(invoker, Mathf.FloorToInt(duration * 2500f).ToStringTicksToPeriod(true, false, true, true, false), psychicRitual.def.Named("RITUAL")), LetterDefOf.ThreatBig, null, 0, true);
		}

		// Token: 0x06004779 RID: 18297 RVA: 0x001BFD74 File Offset: 0x001BDF74
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
			Scribe_Values.Look<FloatRange>(ref this.durationHoursFromQualityRange, "durationHoursFromQualityRange", default(FloatRange), false);
		}

		// Token: 0x04002E48 RID: 11848
		private PsychicRitualRoleDef invokerRole;

		// Token: 0x04002E49 RID: 11849
		private FloatRange durationHoursFromQualityRange;
	}
}
