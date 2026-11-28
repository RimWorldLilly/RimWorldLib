using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C04 RID: 7172
	public class Alert_SlaveRebellionLikely : Alert
	{
		// Token: 0x0600B737 RID: 46903 RVA: 0x00409142 File Offset: 0x00407342
		public Alert_SlaveRebellionLikely()
		{
			this.defaultLabel = "AlertSlaveRebellionLikely".Translate();
			this.requireIdeology = true;
		}

		// Token: 0x0600B738 RID: 46904 RVA: 0x00409174 File Offset: 0x00407374
		public override TaggedString GetExplanation()
		{
			Map currentMap = Find.CurrentMap;
			if (currentMap == null)
			{
				return string.Empty;
			}
			int num = currentMap.mapPawns.SlavesOfColonySpawned.Count((Pawn pawn) => this.MTBMeetsRebelliousThreshold(SlaveRebellionUtility.InitiateSlaveRebellionMtbDays(pawn)));
			Pawn mostRebelliousPawn = this.GetMostRebelliousPawn();
			int numTicks = (int)SlaveRebellionUtility.InitiateSlaveRebellionMtbDays(mostRebelliousPawn) * 60000;
			int numTicks2 = (int)SlaveRebellionUtility.RebellionForAnySlaveInMapMtbDays(currentMap) * 60000;
			this.sb.Length = 0;
			if (num >= 2)
			{
				this.sb.Append("AlertSlaveRebellionLikelyRebelliousCount".Translate(num.Named("REBELLIOUSCOUNT")) + " ");
			}
			this.sb.Append("AlertSlaveRebellionLikelyMostRebellious".Translate(numTicks2.ToStringTicksToPeriodVerbose(true, true).Named("COMBINEDTIME"), mostRebelliousPawn.Named("REBEL"), numTicks.ToStringTicksToPeriodVerbose(true, true).Named("INDIVIDUALTIME")));
			this.sb.Append("\n\n" + SlaveRebellionUtility.GetSlaveRebellionMtbCalculationExplanation(mostRebelliousPawn));
			return this.sb.ToString();
		}

		// Token: 0x0600B739 RID: 46905 RVA: 0x00409294 File Offset: 0x00407494
		public override AlertReport GetReport()
		{
			if (Find.CurrentMap == null)
			{
				return false;
			}
			float mtb = SlaveRebellionUtility.RebellionForAnySlaveInMapMtbDays(Find.CurrentMap);
			if (!this.MTBMeetsRebelliousThreshold(mtb))
			{
				return false;
			}
			return AlertReport.CulpritIs(this.GetMostRebelliousPawn());
		}

		// Token: 0x0600B73A RID: 46906 RVA: 0x004092DC File Offset: 0x004074DC
		private Pawn GetMostRebelliousPawn()
		{
			IEnumerable<Pawn> source = from pawn in Find.CurrentMap.mapPawns.SlavesOfColonySpawned
			where SlaveRebellionUtility.InitiateSlaveRebellionMtbDays(pawn) > 0f
			select pawn;
			if (source.Any<Pawn>())
			{
				return source.MinBy(new Func<Pawn, float>(SlaveRebellionUtility.InitiateSlaveRebellionMtbDays));
			}
			return null;
		}

		// Token: 0x0600B73B RID: 46907 RVA: 0x00409339 File Offset: 0x00407539
		private bool MTBMeetsRebelliousThreshold(float mtb)
		{
			return 15f > mtb && mtb > 0f;
		}

		// Token: 0x04006919 RID: 26905
		private const float ReportAnySlaveMtbThreshold = 15f;

		// Token: 0x0400691A RID: 26906
		private StringBuilder sb = new StringBuilder();
	}
}
