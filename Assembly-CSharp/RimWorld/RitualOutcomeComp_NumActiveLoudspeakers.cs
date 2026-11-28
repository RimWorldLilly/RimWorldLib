using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020016CD RID: 5837
	public class RitualOutcomeComp_NumActiveLoudspeakers : RitualOutcomeComp_Quality
	{
		// Token: 0x06009215 RID: 37397 RVA: 0x00355B80 File Offset: 0x00353D80
		public override RitualOutcomeComp_Data MakeData()
		{
			return new RitualOutcomeComp_DataThingPresence();
		}

		// Token: 0x06009216 RID: 37398 RVA: 0x00355B87 File Offset: 0x00353D87
		private bool LoudspeakerActive(Thing s, TargetInfo target)
		{
			return s.GetRoom(RegionType.Set_All) == target.Cell.GetRoom(target.Map) && s.TryGetComp<CompPowerTrader>().PowerOn;
		}

		// Token: 0x06009217 RID: 37399 RVA: 0x00355BB4 File Offset: 0x00353DB4
		public override void Tick(LordJob_Ritual ritual, RitualOutcomeComp_Data data, float progressAmount)
		{
			base.Tick(ritual, data, progressAmount);
			TargetInfo selectedTarget = ritual.selectedTarget;
			if (selectedTarget.ThingDestroyed || !selectedTarget.HasThing)
			{
				return;
			}
			RitualOutcomeComp_DataThingPresence ritualOutcomeComp_DataThingPresence = (RitualOutcomeComp_DataThingPresence)data;
			foreach (Thing thing in selectedTarget.Map.listerBuldingOfDefInProximity.GetForCell(selectedTarget.Cell, (float)this.maxDistance, ThingDefOf.Loudspeaker, null))
			{
				if (this.LoudspeakerActive(thing, selectedTarget))
				{
					if (!ritualOutcomeComp_DataThingPresence.presentForTicks.ContainsKey(thing))
					{
						ritualOutcomeComp_DataThingPresence.presentForTicks.Add(thing, 0f);
					}
					Dictionary<Thing, float> presentForTicks = ritualOutcomeComp_DataThingPresence.presentForTicks;
					Thing key = thing;
					float num = presentForTicks[key];
					presentForTicks[key] = num + 1f;
				}
			}
		}

		// Token: 0x06009218 RID: 37400 RVA: 0x00355C94 File Offset: 0x00353E94
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			int num = 0;
			foreach (KeyValuePair<Thing, float> keyValuePair in ((RitualOutcomeComp_DataThingPresence)data).presentForTicks)
			{
				if (keyValuePair.Value >= (float)(ritual.DurationTicks / 2))
				{
					num++;
				}
			}
			return (float)((int)Math.Min((float)num, this.curve.Points[this.curve.PointsCount - 1].x));
		}

		// Token: 0x06009219 RID: 37401 RVA: 0x00355D2C File Offset: 0x00353F2C
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			List<Thing> forCell = ritualTarget.Map.listerBuldingOfDefInProximity.GetForCell(ritualTarget.Cell, (float)this.maxDistance, ThingDefOf.Loudspeaker, null);
			int num = 0;
			foreach (Thing thing in forCell)
			{
				CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
				CompFlickable compFlickable = thing.TryGetComp<CompFlickable>();
				if (compPowerTrader != null && compPowerTrader.PowerNet != null && compPowerTrader.PowerNet.HasActivePowerSource && compFlickable != null && compFlickable.SwitchIsOn)
				{
					num++;
				}
			}
			float quality = this.curve.Evaluate((float)num);
			return new QualityFactor
			{
				label = "RitualPredictedOutcomeDescNumActiveLoudspeakers".Translate(),
				count = Mathf.Min((float)num, base.MaxValue).ToString() + " / " + base.MaxValue.ToString(),
				qualityChange = this.ExpectedOffsetDesc(true, quality),
				quality = quality,
				positive = (num > 0),
				priority = 1f
			};
		}

		// Token: 0x0400588C RID: 22668
		public int maxDistance;
	}
}
