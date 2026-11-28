using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012BC RID: 4796
	public class GenStep_ScaleElevation : GenStep
	{
		// Token: 0x1700143B RID: 5179
		// (get) Token: 0x06007705 RID: 30469 RVA: 0x0029F880 File Offset: 0x0029DA80
		public override int SeedPart
		{
			get
			{
				return 253986;
			}
		}

		// Token: 0x06007706 RID: 30470 RVA: 0x0029F888 File Offset: 0x0029DA88
		public override void Generate(Map map, GenStepParams parms)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				MapGenFloatGrid mapGenFloatGrid = elevation;
				IntVec3 c = intVec;
				mapGenFloatGrid[c] *= this.factor;
			}
		}

		// Token: 0x04004AB3 RID: 19123
		public float factor = 1f;
	}
}
