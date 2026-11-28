using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C08 RID: 7176
	public class Alert_AgeReversalDemandNear : Alert
	{
		// Token: 0x0600B74A RID: 46922 RVA: 0x00409B14 File Offset: 0x00407D14
		public Alert_AgeReversalDemandNear()
		{
			this.defaultLabel = "AlertAgeReversalDemandNear".Translate();
			this.requireIdeology = true;
		}

		// Token: 0x0600B74B RID: 46923 RVA: 0x00409B44 File Offset: 0x00407D44
		private void CalcPawnsNearDeadline()
		{
			this.targets.Clear();
			foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
			{
				Ideo ideo = pawn.Ideo;
				if (ideo != null && ideo.HasPrecept(PreceptDefOf.AgeReversal_Demanded) && ThoughtWorker_AgeReversalDemanded.CanHaveThought(pawn))
				{
					float ageReversalDemandedDeadlineTicks = (float)pawn.ageTracker.AgeReversalDemandedDeadlineTicks;
					float num = CompBiosculpterPod.HasBiotunedAutoAgeReversePod(pawn) ? -60000f : 300000f;
					if (ageReversalDemandedDeadlineTicks <= num)
					{
						this.targets.Add(pawn);
					}
				}
			}
		}

		// Token: 0x0600B74C RID: 46924 RVA: 0x00409BEC File Offset: 0x00407DEC
		public override TaggedString GetExplanation()
		{
			TaggedString taggedString = "AlertAgeReversalDemandDesc".Translate();
			foreach (Pawn pawn in this.targets)
			{
				long num = pawn.ageTracker.AgeReversalDemandedDeadlineTicks;
				string key;
				if (num > 0L)
				{
					if (pawn.ageTracker.LastAgeReversalReason == Pawn_AgeTracker.AgeReversalReason.Recruited)
					{
						key = "AlertAgeReversalDemandDesc_Recruit";
					}
					else if (pawn.ageTracker.LastAgeReversalReason == Pawn_AgeTracker.AgeReversalReason.ViaTreatment)
					{
						key = "AlertAgeReversalDemandDesc_Next";
					}
					else
					{
						key = "AlertAgeReversalDemandDesc_Initial";
					}
				}
				else
				{
					num = -num;
					key = "AlertAgeReversalDemandDesc_Overdue";
				}
				taggedString += "\n  - " + key.Translate(pawn.Named("PAWN"), pawn.Faction.Named("FACTION"), ((int)num).ToStringTicksToPeriodVerbose(true, true).Named("DURATION"));
			}
			return taggedString;
		}

		// Token: 0x0600B74D RID: 46925 RVA: 0x00409CE4 File Offset: 0x00407EE4
		public override AlertReport GetReport()
		{
			this.CalcPawnsNearDeadline();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x04006924 RID: 26916
		private const float WarnWhenCloserThanTicks = 300000f;

		// Token: 0x04006925 RID: 26917
		private const float WarnWhenCloserThanTicksAutoAgeReverse = -60000f;

		// Token: 0x04006926 RID: 26918
		private List<Pawn> targets = new List<Pawn>();
	}
}
