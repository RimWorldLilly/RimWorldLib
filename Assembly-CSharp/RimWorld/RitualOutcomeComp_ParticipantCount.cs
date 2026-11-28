using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020016CA RID: 5834
	public class RitualOutcomeComp_ParticipantCount : RitualOutcomeComp_Quality
	{
		// Token: 0x06009202 RID: 37378 RVA: 0x0035554C File Offset: 0x0035374C
		public override RitualOutcomeComp_Data MakeData()
		{
			return new RitualOutcomeComp_DataThingPresence();
		}

		// Token: 0x06009203 RID: 37379 RVA: 0x00355554 File Offset: 0x00353754
		public override void Tick(LordJob_Ritual ritual, RitualOutcomeComp_Data data, float progressAmount)
		{
			base.Tick(ritual, data, progressAmount);
			RitualOutcomeComp_DataThingPresence ritualOutcomeComp_DataThingPresence = (RitualOutcomeComp_DataThingPresence)data;
			foreach (Pawn pawn in ritual.PawnsToCountTowardsPresence)
			{
				if (ritual.Ritual != null)
				{
					RitualRole ritualRole = ritual.RoleFor(pawn, true);
					if (ritualRole != null && !ritualRole.countsAsParticipant)
					{
						continue;
					}
				}
				if (GatheringsUtility.InGatheringArea(pawn.Position, ritual.Spot, pawn.MapHeld))
				{
					if (!ritualOutcomeComp_DataThingPresence.presentForTicks.ContainsKey(pawn))
					{
						ritualOutcomeComp_DataThingPresence.presentForTicks.Add(pawn, 0f);
					}
					Dictionary<Thing, float> presentForTicks = ritualOutcomeComp_DataThingPresence.presentForTicks;
					Thing key = pawn;
					presentForTicks[key] += progressAmount;
				}
			}
		}

		// Token: 0x06009204 RID: 37380 RVA: 0x00355620 File Offset: 0x00353820
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			int num = 0;
			RitualOutcomeComp_DataThingPresence ritualOutcomeComp_DataThingPresence = (RitualOutcomeComp_DataThingPresence)data;
			float num2 = (ritual.DurationTicks != 0) ? ((float)ritual.DurationTicks) : ritual.TicksPassedWithProgress;
			foreach (KeyValuePair<Thing, float> keyValuePair in ritualOutcomeComp_DataThingPresence.presentForTicks)
			{
				Pawn p = (Pawn)keyValuePair.Key;
				if (this.Counts(ritual.assignments, p) && keyValuePair.Value >= num2 / 2f)
				{
					num++;
				}
			}
			return (float)((this.curve != null) ? ((int)Math.Min((float)num, this.curve.Points[this.curve.PointsCount - 1].x)) : num);
		}

		// Token: 0x06009205 RID: 37381 RVA: 0x003556F8 File Offset: 0x003538F8
		protected bool Counts(RitualRoleAssignments assignments, Pawn p)
		{
			if (assignments != null && assignments.Ritual == null && assignments.Required(p))
			{
				return false;
			}
			RitualRole ritualRole = (assignments != null) ? assignments.RoleForPawn(p, true) : null;
			return (ritualRole == null || ritualRole.countsAsParticipant) && p.RaceProps.Humanlike;
		}

		// Token: 0x06009206 RID: 37382 RVA: 0x00355744 File Offset: 0x00353944
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			int num = assignments.Participants.Count((Pawn p) => this.Counts(assignments, p));
			float quality = this.curve.Evaluate((float)num);
			return new QualityFactor
			{
				label = this.label.CapitalizeFirst(),
				count = num.ToString() + " / " + Mathf.Max(base.MaxValue, (float)num).ToString(),
				qualityChange = this.ExpectedOffsetDesc(true, quality),
				quality = quality,
				positive = true,
				priority = 4f
			};
		}
	}
}
