using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012B6 RID: 4790
	public class GenStep_Pollution : GenStep
	{
		// Token: 0x17001435 RID: 5173
		// (get) Token: 0x060076D7 RID: 30423 RVA: 0x0029DEC8 File Offset: 0x0029C0C8
		public override int SeedPart
		{
			get
			{
				return 7593785;
			}
		}

		// Token: 0x060076D8 RID: 30424 RVA: 0x0029DECF File Offset: 0x0029C0CF
		public override void Generate(Map map, GenStepParams parms)
		{
			PollutionUtility.PolluteMapToPercent(map, Find.WorldGrid[map.Tile].pollution, 1000);
		}

		// Token: 0x04004A99 RID: 19097
		private static readonly FloatRange LightPollutionCellRange = new FloatRange(0.1f, 0.3f);

		// Token: 0x04004A9A RID: 19098
		private static readonly FloatRange ModeratePollutionCellRange = new FloatRange(0.4f, 0.6f);

		// Token: 0x04004A9B RID: 19099
		private const int PollutionGlobSize = 1000;
	}
}
