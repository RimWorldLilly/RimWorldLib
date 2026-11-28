using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FCC RID: 8140
	public class StatWorker_PsyfocusCost : StatWorker
	{
		// Token: 0x0600CEA7 RID: 52903 RVA: 0x004C150C File Offset: 0x004BF70C
		public override string GetStatDrawEntryLabel(StatDef stat, float value, ToStringNumberSense numberSense, StatRequest optionalReq, bool finalized = true)
		{
			if (optionalReq.ForAbility)
			{
				AbilityDef abilityDef = optionalReq.AbilityDef;
				if (abilityDef.AnyCompOverridesPsyfocusCost)
				{
					if (abilityDef.PsyfocusCostRange.Span > 1E-45f)
					{
						return (abilityDef.PsyfocusCostRange.min * 100f).ToString("0.##") + "-" + stat.ValueToString(abilityDef.PsyfocusCostRange.max, numberSense, finalized);
					}
					return stat.ValueToString(abilityDef.PsyfocusCostRange.max, numberSense, finalized);
				}
			}
			return base.GetStatDrawEntryLabel(stat, value, numberSense, optionalReq, finalized);
		}

		// Token: 0x0600CEA8 RID: 52904 RVA: 0x004C15A8 File Offset: 0x004BF7A8
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			if (req.ForAbility)
			{
				foreach (AbilityCompProperties abilityCompProperties in req.AbilityDef.comps)
				{
					if (abilityCompProperties.OverridesPsyfocusCost)
					{
						return abilityCompProperties.PsyfocusCostExplanation;
					}
				}
			}
			return base.GetExplanationUnfinalized(req, numberSense);
		}
	}
}
