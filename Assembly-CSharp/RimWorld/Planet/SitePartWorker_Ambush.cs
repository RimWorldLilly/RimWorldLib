using System;

namespace RimWorld.Planet
{
	// Token: 0x020023C4 RID: 9156
	public class SitePartWorker_Ambush : SitePartWorker
	{
		// Token: 0x0600E45E RID: 58462 RVA: 0x0054DB01 File Offset: 0x0054BD01
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, PlanetTile tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			sitePartParams.threatPoints *= 0.8f;
			return sitePartParams;
		}

		// Token: 0x04009158 RID: 37208
		private const float ThreatPointsFactor = 0.8f;
	}
}
