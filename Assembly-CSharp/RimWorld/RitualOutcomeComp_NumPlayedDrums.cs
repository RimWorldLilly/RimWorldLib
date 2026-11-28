using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020016CE RID: 5838
	public class RitualOutcomeComp_NumPlayedDrums : RitualOutcomeComp_Quality
	{
		// Token: 0x0600921B RID: 37403 RVA: 0x00355E60 File Offset: 0x00354060
		public override RitualOutcomeComp_Data MakeData()
		{
			return new RitualOutcomeComp_DataThingPresence();
		}

		// Token: 0x0600921C RID: 37404 RVA: 0x00355E68 File Offset: 0x00354068
		public override void Tick(LordJob_Ritual ritual, RitualOutcomeComp_Data data, float progressAmount)
		{
			base.Tick(ritual, data, progressAmount);
			TargetInfo selectedTarget = ritual.selectedTarget;
			if (selectedTarget.ThingDestroyed || !selectedTarget.HasThing)
			{
				return;
			}
			RitualOutcomeComp_DataThingPresence ritualOutcomeComp_DataThingPresence = (RitualOutcomeComp_DataThingPresence)data;
			foreach (Thing thing in selectedTarget.Map.listerBuldingOfDefInProximity.GetForCell(selectedTarget.Cell, (float)this.maxDistance, ThingDefOf.Drum, null))
			{
				Building_MusicalInstrument building_MusicalInstrument = thing as Building_MusicalInstrument;
				if (building_MusicalInstrument != null && thing.GetRoom(RegionType.Set_All) == selectedTarget.Cell.GetRoom(selectedTarget.Map) && building_MusicalInstrument.IsBeingPlayed)
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

		// Token: 0x0600921D RID: 37405 RVA: 0x00355F74 File Offset: 0x00354174
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

		// Token: 0x0600921E RID: 37406 RVA: 0x0035600C File Offset: 0x0035420C
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			int count = ritualTarget.Map.listerBuldingOfDefInProximity.GetForCell(ritualTarget.Cell, (float)this.maxDistance, ThingDefOf.Drum, null).Count;
			float quality = this.curve.Evaluate((float)count);
			return new QualityFactor
			{
				label = "RitualPredictedOutcomeDescNumDrums".Translate(),
				count = Mathf.Min((float)count, base.MaxValue).ToString() + " / " + base.MaxValue.ToString(),
				qualityChange = this.ExpectedOffsetDesc(true, quality),
				quality = quality,
				positive = (count > 0),
				priority = 1f
			};
		}

		// Token: 0x0400588D RID: 22669
		public int maxDistance;
	}
}
