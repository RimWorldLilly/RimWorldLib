using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200117F RID: 4479
	public class IncidentWorker_ColdSnap : IncidentWorker_MakeGameCondition
	{
		// Token: 0x06007058 RID: 28760 RVA: 0x002753DA File Offset: 0x002735DA
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return base.CanFireNowSub(parms) && IncidentWorker_ColdSnap.IsTemperatureAppropriate((Map)parms.target);
		}

		// Token: 0x06007059 RID: 28761 RVA: 0x002753F7 File Offset: 0x002735F7
		public static bool IsTemperatureAppropriate(Map map)
		{
			return map.mapTemperature.SeasonalTemp > 0f && map.mapTemperature.SeasonalTemp < 15f;
		}
	}
}
