using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012B7 RID: 4791
	public class GenStep_PoluxTrees : GenStep_SpecialTrees
	{
		// Token: 0x17001436 RID: 5174
		// (get) Token: 0x060076DB RID: 30427 RVA: 0x0029DF23 File Offset: 0x0029C123
		public override int SeedPart
		{
			get
			{
				return 537455645;
			}
		}

		// Token: 0x060076DC RID: 30428 RVA: 0x0029DF2C File Offset: 0x0029C12C
		public override int DesiredTreeCountForMap(Map map)
		{
			PollutionLevel pollutionLevel = Find.WorldGrid[map.Tile].PollutionLevel();
			switch (pollutionLevel)
			{
			case PollutionLevel.None:
				return this.pollutionNone;
			case PollutionLevel.Light:
				return this.pollutionLight;
			case PollutionLevel.Moderate:
				return this.pollutionModerate;
			case PollutionLevel.Extreme:
				return this.pollutionExtreme;
			default:
				Log.Error("Unrecognized pollution level '" + pollutionLevel.ToString() + "'.");
				return 0;
			}
		}

		// Token: 0x060076DD RID: 30429 RVA: 0x0029DFA5 File Offset: 0x0029C1A5
		protected override float GetGrowth()
		{
			return 1f;
		}

		// Token: 0x04004A9C RID: 19100
		private const float Growth = 1f;
	}
}
