using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200117E RID: 4478
	public class IncidentWorker_HeatWave : IncidentWorker_MakeGameCondition
	{
		// Token: 0x06007055 RID: 28757 RVA: 0x0027539E File Offset: 0x0027359E
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return base.CanFireNowSub(parms) && IncidentWorker_HeatWave.IsTemperatureAppropriate((Map)parms.target);
		}

		// Token: 0x06007056 RID: 28758 RVA: 0x002753BB File Offset: 0x002735BB
		public static bool IsTemperatureAppropriate(Map map)
		{
			return map.mapTemperature.SeasonalTemp >= 20f;
		}
	}
}
