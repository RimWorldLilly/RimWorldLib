using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000762 RID: 1890
	public class Perlin : ModuleBase
	{
		// Token: 0x17000B97 RID: 2967
		// (get) Token: 0x06003B9E RID: 15262 RVA: 0x0017B11F File Offset: 0x0017931F
		// (set) Token: 0x06003B9F RID: 15263 RVA: 0x0017B127 File Offset: 0x00179327
		public double Frequency
		{
			get
			{
				return this.frequency;
			}
			set
			{
				this.frequency = value;
			}
		}

		// Token: 0x17000B98 RID: 2968
		// (get) Token: 0x06003BA0 RID: 15264 RVA: 0x0017B130 File Offset: 0x00179330
		// (set) Token: 0x06003BA1 RID: 15265 RVA: 0x0017B138 File Offset: 0x00179338
		public double Lacunarity
		{
			get
			{
				return this.lacunarity;
			}
			set
			{
				this.lacunarity = value;
			}
		}

		// Token: 0x17000B99 RID: 2969
		// (get) Token: 0x06003BA2 RID: 15266 RVA: 0x0017B141 File Offset: 0x00179341
		// (set) Token: 0x06003BA3 RID: 15267 RVA: 0x0017B149 File Offset: 0x00179349
		public QualityMode Quality
		{
			get
			{
				return this.quality;
			}
			set
			{
				this.quality = value;
			}
		}

		// Token: 0x17000B9A RID: 2970
		// (get) Token: 0x06003BA4 RID: 15268 RVA: 0x0017B152 File Offset: 0x00179352
		// (set) Token: 0x06003BA5 RID: 15269 RVA: 0x0017B15A File Offset: 0x0017935A
		public int OctaveCount
		{
			get
			{
				return this.octaveCount;
			}
			set
			{
				this.octaveCount = Mathf.Clamp(value, 1, 30);
			}
		}

		// Token: 0x17000B9B RID: 2971
		// (get) Token: 0x06003BA6 RID: 15270 RVA: 0x0017B16B File Offset: 0x0017936B
		// (set) Token: 0x06003BA7 RID: 15271 RVA: 0x0017B173 File Offset: 0x00179373
		public double Persistence
		{
			get
			{
				return this.persistence;
			}
			set
			{
				this.persistence = value;
			}
		}

		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x06003BA8 RID: 15272 RVA: 0x0017B17C File Offset: 0x0017937C
		// (set) Token: 0x06003BA9 RID: 15273 RVA: 0x0017B184 File Offset: 0x00179384
		public int Seed
		{
			get
			{
				return this.seed;
			}
			set
			{
				this.seed = value;
			}
		}

		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x06003BAA RID: 15274 RVA: 0x0017B18D File Offset: 0x0017938D
		// (set) Token: 0x06003BAB RID: 15275 RVA: 0x0017B195 File Offset: 0x00179395
		public bool Invert
		{
			get
			{
				return this.invert;
			}
			set
			{
				this.invert = value;
			}
		}

		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x06003BAC RID: 15276 RVA: 0x0017B19E File Offset: 0x0017939E
		// (set) Token: 0x06003BAD RID: 15277 RVA: 0x0017B1A6 File Offset: 0x001793A6
		public bool Normalized
		{
			get
			{
				return this.normalized;
			}
			set
			{
				this.normalized = value;
			}
		}

		// Token: 0x06003BAE RID: 15278 RVA: 0x0017B1B0 File Offset: 0x001793B0
		public Perlin() : base(0)
		{
		}

		// Token: 0x06003BAF RID: 15279 RVA: 0x0017B200 File Offset: 0x00179400
		public Perlin(double frequency, double lacunarity, double persistence, int octaves, int seed, QualityMode quality) : base(0)
		{
			this.Frequency = frequency;
			this.Lacunarity = lacunarity;
			this.OctaveCount = octaves;
			this.Persistence = persistence;
			this.Seed = seed;
			this.Quality = quality;
		}

		// Token: 0x06003BB0 RID: 15280 RVA: 0x0017B27C File Offset: 0x0017947C
		public Perlin(double frequency, double lacunarity, double persistence, int octaves, bool normalized, bool invert, int seed, QualityMode quality) : base(0)
		{
			this.Frequency = frequency;
			this.Lacunarity = lacunarity;
			this.OctaveCount = octaves;
			this.Persistence = persistence;
			this.Seed = seed;
			this.Normalized = normalized;
			this.Invert = invert;
			this.Quality = quality;
		}

		// Token: 0x06003BB1 RID: 15281 RVA: 0x0017B308 File Offset: 0x00179508
		public static double GetValue(double x, double y, double z, double frequency, int seed, double lacunarity = 2.0, double persistence = 0.5, int octaveCount = 6, bool normalized = false, bool invert = false, QualityMode quality = QualityMode.Medium)
		{
			double num = 0.0;
			double num2 = 1.0;
			x *= frequency;
			y *= frequency;
			z *= frequency;
			for (int i = 0; i < octaveCount; i++)
			{
				double x2 = Utils.MakeInt32Range(x);
				double y2 = Utils.MakeInt32Range(y);
				double z2 = Utils.MakeInt32Range(z);
				long num3 = (long)(seed + i) & (long)((ulong)-1);
				double num4 = Utils.GradientCoherentNoise3D(x2, y2, z2, num3, quality);
				num += num4 * num2;
				x *= lacunarity;
				y *= lacunarity;
				z *= lacunarity;
				num2 *= persistence;
			}
			if (normalized)
			{
				num = num * 0.5 + 0.5;
				if (num < 0.0)
				{
					num = 0.0;
				}
				if (num > 1.0)
				{
					num = 1.0;
				}
			}
			if (invert)
			{
				num = 1.0 - num;
			}
			return num;
		}

		// Token: 0x06003BB2 RID: 15282 RVA: 0x0017B3F4 File Offset: 0x001795F4
		public override double GetValue(double x, double y, double z)
		{
			return Perlin.GetValue(x, y, z, this.frequency, this.seed, this.lacunarity, this.persistence, this.octaveCount, this.normalized, this.invert, this.quality);
		}

		// Token: 0x040026F9 RID: 9977
		private double frequency = 1.0;

		// Token: 0x040026FA RID: 9978
		private double lacunarity = 2.0;

		// Token: 0x040026FB RID: 9979
		private QualityMode quality = QualityMode.Medium;

		// Token: 0x040026FC RID: 9980
		private int octaveCount = 6;

		// Token: 0x040026FD RID: 9981
		private double persistence = 0.5;

		// Token: 0x040026FE RID: 9982
		private int seed;

		// Token: 0x040026FF RID: 9983
		private bool normalized;

		// Token: 0x04002700 RID: 9984
		private bool invert;
	}
}
