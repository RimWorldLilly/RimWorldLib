using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011C0 RID: 4544
	public class IncidentWorker_Aurora : IncidentWorker_MakeGameCondition
	{
		// Token: 0x06007183 RID: 29059 RVA: 0x0027C728 File Offset: 0x0027A928
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			foreach (Map map in Find.Maps)
			{
				if (map.IsPlayerHome && !map.GameConditionManager.IsAlwaysDarkOutside && !this.AuroraWillEndSoon(map))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007184 RID: 29060 RVA: 0x0027C7A4 File Offset: 0x0027A9A4
		private bool AuroraWillEndSoon(Map map)
		{
			return GenCelestial.CurCelestialSunGlow(map) > 0.5f || GenCelestial.CelestialSunGlow(map, Find.TickManager.TicksAbs + 5000) > 0.5f;
		}

		// Token: 0x040047D1 RID: 18385
		private const int EnsureMinDurationTicks = 5000;
	}
}
