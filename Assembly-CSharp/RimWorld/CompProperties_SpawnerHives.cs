using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001984 RID: 6532
	public class CompProperties_SpawnerHives : CompProperties
	{
		// Token: 0x0600A40A RID: 41994 RVA: 0x003A9C44 File Offset: 0x003A7E44
		public CompProperties_SpawnerHives()
		{
			this.compClass = typeof(CompSpawnerHives);
		}

		// Token: 0x040060E6 RID: 24806
		public float HiveSpawnPreferredMinDist = 3.5f;

		// Token: 0x040060E7 RID: 24807
		public float HiveSpawnRadius = 10f;

		// Token: 0x040060E8 RID: 24808
		public FloatRange HiveSpawnIntervalDays = new FloatRange(2f, 3f);

		// Token: 0x040060E9 RID: 24809
		public SimpleCurve ReproduceRateFactorFromNearbyHiveCountCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(7f, 0.35f),
				true
			}
		};
	}
}
