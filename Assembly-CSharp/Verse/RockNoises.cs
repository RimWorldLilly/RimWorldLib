using System;
using System.Collections.Generic;
using Verse.Noise;

namespace Verse
{
	// Token: 0x020002DF RID: 735
	public static class RockNoises
	{
		// Token: 0x06001659 RID: 5721 RVA: 0x00086578 File Offset: 0x00084778
		public static void Init(Map map)
		{
			RockNoises.rockNoises = new List<RockNoises.RockNoise>();
			foreach (ThingDef rockDef in Find.World.NaturalRockTypesIn(map.Tile))
			{
				RockNoises.RockNoise rockNoise = new RockNoises.RockNoise();
				rockNoise.rockDef = rockDef;
				rockNoise.noise = new Perlin(0.004999999888241291, 2.0, 0.5, 6, Rand.Range(0, int.MaxValue), QualityMode.Medium);
				RockNoises.rockNoises.Add(rockNoise);
				ModuleBase noise = rockNoise.noise;
				ThingDef rockDef2 = rockNoise.rockDef;
				NoiseDebugUI.StoreNoiseRender(noise, ((rockDef2 != null) ? rockDef2.ToString() : null) + " score", map.Size.ToIntVec2);
			}
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x00086658 File Offset: 0x00084858
		public static void Reset()
		{
			RockNoises.rockNoises = null;
		}

		// Token: 0x04001352 RID: 4946
		public static List<RockNoises.RockNoise> rockNoises;

		// Token: 0x04001353 RID: 4947
		private const float RockNoiseFreq = 0.005f;

		// Token: 0x02002617 RID: 9751
		public class RockNoise
		{
			// Token: 0x04009C2C RID: 39980
			public ThingDef rockDef;

			// Token: 0x04009C2D RID: 39981
			public ModuleBase noise;
		}
	}
}
