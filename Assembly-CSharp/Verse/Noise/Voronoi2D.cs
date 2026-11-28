using System;

namespace Verse.Noise
{
	// Token: 0x02000764 RID: 1892
	public class Voronoi2D : ModuleBase
	{
		// Token: 0x06003BC1 RID: 15297 RVA: 0x0017B6E0 File Offset: 0x001798E0
		public Voronoi2D() : base(0)
		{
		}

		// Token: 0x06003BC2 RID: 15298 RVA: 0x0017B710 File Offset: 0x00179910
		public Voronoi2D(double frequency, int seed, float xRandomness = 1f, float zRandomness = 1f, bool staggered = false) : base(0)
		{
			this.frequency = frequency;
			this.seed = seed;
			this.xRandomness = xRandomness;
			this.zRandomness = zRandomness;
			this.staggered = staggered;
		}

		// Token: 0x06003BC3 RID: 15299 RVA: 0x0017B770 File Offset: 0x00179970
		public Voronoi2D(double frequency, int seed, float randomness = 1f, bool staggered = false) : base(0)
		{
			this.frequency = frequency;
			this.seed = seed;
			this.xRandomness = randomness;
			this.zRandomness = randomness;
			this.staggered = staggered;
		}

		// Token: 0x06003BC4 RID: 15300 RVA: 0x0017B7D0 File Offset: 0x001799D0
		public override double GetValue(double x, double y, double z)
		{
			x *= this.frequency;
			z *= this.frequency;
			int num = (x > 0.0) ? ((int)x) : ((int)x - 1);
			int num2 = (z > 0.0) ? ((int)z) : ((int)z - 1);
			double num3 = 2147483647.0;
			double num4 = 0.0;
			double num5 = 0.0;
			for (int i = num2 - 2; i <= num2 + 2; i++)
			{
				for (int j = num - 2; j <= num + 2; j++)
				{
					double num6 = (double)j + Utils.ValueNoise3D(j, 0, i, this.seed) * (double)this.xRandomness;
					double num7 = (double)i + Utils.ValueNoise3D(j, 0, i, this.seed + 2) * (double)this.zRandomness;
					if (this.staggered && i % 2 == 0)
					{
						num6 += 0.5;
					}
					double num8 = num6 - x;
					double num9 = num7 - z;
					double num10 = num8 * num8 + num9 * num9;
					if (num10 < num3)
					{
						num3 = num10;
						num4 = num6;
						num5 = num7;
					}
				}
			}
			double num11 = num4 - x;
			double num12 = num5 - z;
			return Math.Sqrt(num11 * num11 + num12 * num12);
		}

		// Token: 0x04002707 RID: 9991
		private double frequency = 1.0;

		// Token: 0x04002708 RID: 9992
		private int seed;

		// Token: 0x04002709 RID: 9993
		private float xRandomness = 1f;

		// Token: 0x0400270A RID: 9994
		private float zRandomness = 1f;

		// Token: 0x0400270B RID: 9995
		private bool staggered;
	}
}
