using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FCE RID: 8142
	public class StatWorker_ShootingAccuracy : StatWorker
	{
		// Token: 0x0600CEAC RID: 52908 RVA: 0x004C1674 File Offset: 0x004BF874
		public override string GetExplanationFinalizePart(StatRequest req, ToStringNumberSense numberSense, float finalVal)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.GetExplanationFinalizePart(req, numberSense, finalVal));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("StatsReport_ShootingExampleDistances".Translate());
			float f = ShotReport.HitFactorFromShooter(req.Thing, 1f, new float?(finalVal));
			stringBuilder.AppendLine("distance".Translate().CapitalizeFirst() + " 1: " + f.ToStringPercent("F1"));
			for (int i = 5; i <= 55; i += 5)
			{
				f = ShotReport.HitFactorFromShooter(req.Thing, (float)i, new float?(finalVal));
				stringBuilder.AppendLine("distance".Translate().CapitalizeFirst() + " " + i.ToString() + ": " + f.ToStringPercent("F1"));
			}
			return stringBuilder.ToString();
		}
	}
}
