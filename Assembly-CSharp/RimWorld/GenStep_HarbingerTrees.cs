using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012A7 RID: 4775
	public class GenStep_HarbingerTrees : GenStep_SpecialTrees
	{
		// Token: 0x17001411 RID: 5137
		// (get) Token: 0x0600765D RID: 30301 RVA: 0x0029AC68 File Offset: 0x00298E68
		public override int SeedPart
		{
			get
			{
				return 786491238;
			}
		}

		// Token: 0x0600765E RID: 30302 RVA: 0x0029AC6F File Offset: 0x00298E6F
		protected override float GetGrowth()
		{
			return 1f;
		}

		// Token: 0x0600765F RID: 30303 RVA: 0x0029AC76 File Offset: 0x00298E76
		public override int DesiredTreeCountForMap(Map map)
		{
			return Find.Anomaly.LevelDef.desiredHarbingerTreeCount;
		}

		// Token: 0x04004A34 RID: 18996
		private const float Growth = 1f;
	}
}
