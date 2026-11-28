using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020016C5 RID: 5829
	public abstract class RitualOutcomeComp_BuildingsPresent : RitualOutcomeComp_QualitySingleOffset
	{
		// Token: 0x060091E7 RID: 37351 RVA: 0x00354E3B File Offset: 0x0035303B
		protected virtual int RequiredAmount(RitualRoleAssignments assignments)
		{
			return 1;
		}

		// Token: 0x060091E8 RID: 37352 RVA: 0x00354E3E File Offset: 0x0035303E
		protected virtual string LabelForPredictedOutcomeDesc(Precept_Ritual ritual)
		{
			return this.LabelForDesc;
		}

		// Token: 0x060091E9 RID: 37353
		protected abstract Thing LookForBuilding(IntVec3 cell, Map map, Precept_Ritual ritual);

		// Token: 0x060091EA RID: 37354 RVA: 0x00354E48 File Offset: 0x00353048
		protected virtual int CountAvailable(Precept_Ritual ritual, TargetInfo ritualTarget)
		{
			RitualOutcomeComp_BuildingsPresent.<>c__DisplayClass4_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.ritualTarget = ritualTarget;
			CS$<>8__locals1.ritual = ritual;
			if (CS$<>8__locals1.ritualTarget == null || !CS$<>8__locals1.ritualTarget.IsValid)
			{
				return 0;
			}
			int result;
			try
			{
				bool usesLectern = CS$<>8__locals1.ritual.behavior.def.UsesLectern;
				RitualOutcomeComp_BuildingsPresent.<>c__DisplayClass4_1 CS$<>8__locals2;
				CS$<>8__locals2.lecternPos = CS$<>8__locals1.ritual.behavior.def.FirstLecternPosition;
				CS$<>8__locals2.allLecterns = (usesLectern ? CS$<>8__locals1.ritualTarget.Map.listerThings.ThingsOfDef(ThingDefOf.Lectern) : null);
				int num = 0;
				if (GatheringsUtility.UseWholeRoomAsGatheringArea(CS$<>8__locals1.ritualTarget.Cell, CS$<>8__locals1.ritualTarget.Map))
				{
					using (IEnumerator<IntVec3> enumerator = CS$<>8__locals1.ritualTarget.Cell.GetRoom(CS$<>8__locals1.ritualTarget.Map).Cells.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							IntVec3 cell = enumerator.Current;
							if (this.<CountAvailable>g__Check|4_0(cell, ref CS$<>8__locals1, ref CS$<>8__locals2))
							{
								num++;
							}
						}
						goto IL_168;
					}
				}
				foreach (IntVec3 cell2 in CellRect.CenteredOn(CS$<>8__locals1.ritualTarget.Cell, 18))
				{
					if (this.<CountAvailable>g__Check|4_0(cell2, ref CS$<>8__locals1, ref CS$<>8__locals2))
					{
						num++;
					}
				}
				IL_168:
				result = num;
			}
			finally
			{
				RitualOutcomeComp_BuildingsPresent.tmpThingsAlreadyCounted.Clear();
			}
			return result;
		}

		// Token: 0x060091EB RID: 37355 RVA: 0x0035501C File Offset: 0x0035321C
		private float CalcQualityOffset(int available, int required)
		{
			return this.qualityOffset * Mathf.Clamp01((float)available / (float)required);
		}

		// Token: 0x060091EC RID: 37356 RVA: 0x0035502F File Offset: 0x0035322F
		public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			return this.CalcQualityOffset(this.CountAvailable(ritual.Ritual, ritual.selectedTarget), this.RequiredAmount(ritual.assignments));
		}

		// Token: 0x060091ED RID: 37357 RVA: 0x00355055 File Offset: 0x00353255
		public override bool Applies(LordJob_Ritual ritual)
		{
			return this.CountAvailable(ritual.Ritual, ritual.selectedTarget) > 0;
		}

		// Token: 0x060091EE RID: 37358 RVA: 0x0035506C File Offset: 0x0035326C
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			int num = this.RequiredAmount(assignments);
			if (num <= 0)
			{
				return null;
			}
			int num2 = this.CountAvailable(ritual, ritualTarget);
			string count = null;
			if (num != 1)
			{
				count = num2.ToString() + " / " + num.ToString();
			}
			float quality = this.CalcQualityOffset(num2, num);
			return new QualityFactor
			{
				label = this.LabelForPredictedOutcomeDesc(ritual),
				qualityChange = this.ExpectedOffsetDesc(num2 > 0, quality),
				present = (num2 > 0),
				quality = quality,
				positive = (num2 > 0),
				priority = 1f,
				count = count
			};
		}

		// Token: 0x060091F1 RID: 37361 RVA: 0x00355120 File Offset: 0x00353320
		[CompilerGenerated]
		private bool <CountAvailable>g__Check|4_0(IntVec3 cell, ref RitualOutcomeComp_BuildingsPresent.<>c__DisplayClass4_0 A_2, ref RitualOutcomeComp_BuildingsPresent.<>c__DisplayClass4_1 A_3)
		{
			Thing thing = this.LookForBuilding(cell, A_2.ritualTarget.Map, A_2.ritual);
			if (thing != null && GatheringsUtility.InGatheringArea(cell, A_2.ritualTarget.Cell, A_2.ritualTarget.Map))
			{
				IntVec3? intVec = null;
				if (A_3.allLecterns != null)
				{
					foreach (Thing thing2 in A_3.allLecterns)
					{
						if (GatheringsUtility.InGatheringArea(thing2.Position, A_2.ritualTarget.Cell, A_2.ritualTarget.Map) && A_3.lecternPos.IsUsableThing(thing2, A_2.ritualTarget.Cell, A_2.ritualTarget))
						{
							intVec = new IntVec3?(thing2.Position);
							break;
						}
					}
				}
				if (intVec == null)
				{
					intVec = new IntVec3?(A_2.ritualTarget.Thing.Position);
				}
				if (thing.def.building == null || !thing.def.building.isSittable || SpectatorCellFinder.CorrectlyRotatedChairAt(thing.Position, thing.Map, CellRect.CenteredOn(intVec.Value, 1)))
				{
					return RitualOutcomeComp_BuildingsPresent.tmpThingsAlreadyCounted.Add(thing);
				}
			}
			return false;
		}

		// Token: 0x04005882 RID: 22658
		private static HashSet<Thing> tmpThingsAlreadyCounted = new HashSet<Thing>();
	}
}
