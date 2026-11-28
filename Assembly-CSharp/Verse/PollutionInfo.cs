using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200029F RID: 671
	public class PollutionInfo : MapComponent
	{
		// Token: 0x06001490 RID: 5264 RVA: 0x0007599C File Offset: 0x00073B9C
		public PollutionInfo(Map map) : base(map)
		{
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x000759A5 File Offset: 0x00073BA5
		public override void MapGenerated()
		{
			if (ModsConfig.BiotechActive)
			{
				this.pollutionAtGen = (float)this.map.pollutionGrid.TotalPollution;
			}
		}

		// Token: 0x06001492 RID: 5266 RVA: 0x000759C8 File Offset: 0x00073BC8
		public override void MapRemoved()
		{
			if (ModsConfig.BiotechActive)
			{
				float num = (float)this.map.pollutionGrid.TotalPollution - this.pollutionAtGen;
				CompProperties_DissolutionEffectPollution compProperties = ThingDefOf.Wastepack.GetCompProperties<CompProperties_DissolutionEffectPollution>();
				int amount = (int)(num / (float)compProperties.cellsToPollutePerDissolution);
				if (num > 0f)
				{
					CompDissolutionEffect_Goodwill.AddWorldDissolutionEvent(amount, this.map.Tile);
				}
			}
		}

		// Token: 0x06001493 RID: 5267 RVA: 0x00075A22 File Offset: 0x00073C22
		public override void ExposeData()
		{
			if (ModsConfig.BiotechActive)
			{
				Scribe_Values.Look<float>(ref this.pollutionAtGen, "pollutionAtGen", 0f, false);
			}
		}

		// Token: 0x04001220 RID: 4640
		private float pollutionAtGen;
	}
}
