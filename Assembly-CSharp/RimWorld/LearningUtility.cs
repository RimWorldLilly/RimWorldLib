using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B96 RID: 2966
	public static class LearningUtility
	{
		// Token: 0x060055EF RID: 21999 RVA: 0x001FF917 File Offset: 0x001FDB17
		public static float SchoolDeskLearningRate(Thing schoolDesk)
		{
			return 1f + (float)LearningUtility.ConnectedBlackboards(schoolDesk) * 0.2f;
		}

		// Token: 0x060055F0 RID: 22000 RVA: 0x001FF92C File Offset: 0x001FDB2C
		public static int ConnectedBlackboards(Thing desk)
		{
			int num = 0;
			CompAffectedByFacilities compAffectedByFacilities = desk.TryGetComp<CompAffectedByFacilities>();
			if (compAffectedByFacilities != null)
			{
				using (List<Thing>.Enumerator enumerator = compAffectedByFacilities.LinkedFacilitiesListForReading.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def == ThingDefOf.Blackboard)
						{
							num++;
							if (num >= 3)
							{
								break;
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x060055F1 RID: 22001 RVA: 0x001FF99C File Offset: 0x001FDB9C
		public static float LearningRateFactor(Pawn pawn)
		{
			Job curJob = pawn.CurJob;
			float num = pawn.GetStatValue(StatDefOf.LearningRateFactor, true, -1);
			if (curJob.targetA.HasThing && curJob.targetA.Thing.def == ThingDefOf.SchoolDesk)
			{
				num *= LearningUtility.SchoolDeskLearningRate(curJob.targetA.Thing);
			}
			return num;
		}

		// Token: 0x060055F2 RID: 22002 RVA: 0x001FF9F6 File Offset: 0x001FDBF6
		public static bool LearningTickCheckEnd(Pawn pawn, int delta, bool forced = false)
		{
			pawn.needs.learning.Learn(1.2E-05f * LearningUtility.LearningRateFactor(pawn) * (float)delta);
			if (LearningUtility.LearningSatisfied(pawn) && !forced)
			{
				pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
				return true;
			}
			return false;
		}

		// Token: 0x060055F3 RID: 22003 RVA: 0x001FFA33 File Offset: 0x001FDC33
		public static bool LearningSatisfied(Pawn pawn)
		{
			return pawn.needs.learning.CurLevel >= 0.999f;
		}

		// Token: 0x040032FA RID: 13050
		public const float NeedSatisfiedPerTick = 1.2E-05f;

		// Token: 0x040032FB RID: 13051
		public const float StartJobMaxLearning = 0.9f;

		// Token: 0x040032FC RID: 13052
		private const float EndJobMaxLearning = 0.999f;

		// Token: 0x040032FD RID: 13053
		public const float LearningRateBonusOffset_Blackboard = 0.2f;

		// Token: 0x040032FE RID: 13054
		public const int MaxConnectedBlackboards = 3;
	}
}
