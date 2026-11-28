using System;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FBB RID: 8123
	public class StatWorker_CaravanRidingSpeedFactor : StatWorker
	{
		// Token: 0x0600CE5C RID: 52828 RVA: 0x004BECF0 File Offset: 0x004BCEF0
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (req.HasThing && req.Thing.def.race != null)
			{
				stringBuilder.Append("StatsReport_CaravanRideableAge".Translate()).Append(": ").Append(CaravanRideableUtility.RideableLifeStagesDesc(req.Thing.def.race)).AppendLine();
			}
			stringBuilder.Append(base.GetExplanationUnfinalized(req, numberSense));
			return stringBuilder.ToString();
		}
	}
}
