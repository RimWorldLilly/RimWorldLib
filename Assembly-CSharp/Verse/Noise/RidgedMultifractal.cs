using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000763 RID: 1891
	public class RidgedMultifractal : ModuleBase
	{
		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x06003BB3 RID: 15283 RVA: 0x0017B439 File Offset: 0x00179639
		// (set) Token: 0x06003BB4 RID: 15284 RVA: 0x0017B441 File Offset: 0x00179641
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

		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x06003BB5 RID: 15285 RVA: 0x0017B44A File Offset: 0x0017964A
		// (set) Token: 0x06003BB6 RID: 15286 RVA: 0x0017B452 File Offset: 0x00179652
		public double Lacunarity
		{
			get
			{
				return this.lacunarity;
			}
			set
			{
				this.lacunarity = value;
				this.UpdateWeights();
			}
		}

		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x06003BB7 RID: 15287 RVA: 0x0017B461 File Offset: 0x00179661
		// (set) Token: 0x06003BB8 RID: 15288 RVA: 0x0017B469 File Offset: 0x00179669
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

		// Token: 0x17000BA2 RID: 2978
		// (get) Token: 0x06003BB9 RID: 15289 RVA: 0x0017B472 File Offset: 0x00179672
		// (set) Token: 0x06003BBA RID: 15290 RVA: 0x0017B47A File Offset: 0x0017967A
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

		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x06003BBB RID: 15291 RVA: 0x0017B48B File Offset: 0x0017968B
		// (set) Token: 0x06003BBC RID: 15292 RVA: 0x0017B493 File Offset: 0x00179693
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

		// Token: 0x06003BBD RID: 15293 RVA: 0x0017B49C File Offset: 0x0017969C
		public RidgedMultifractal() : base(0)
		{
			this.UpdateWeights();
		}

		// Token: 0x06003BBE RID: 15294 RVA: 0x0017B4F0 File Offset: 0x001796F0
		public RidgedMultifractal(double frequency, double lacunarity, int octaves, int seed, QualityMode quality) : base(0)
		{
			this.Frequency = frequency;
			this.Lacunarity = lacunarity;
			this.OctaveCount = octaves;
			this.Seed = seed;
			this.Quality = quality;
		}

		// Token: 0x06003BBF RID: 15295 RVA: 0x0017B564 File Offset: 0x00179764
		private void UpdateWeights()
		{
			double num = 1.0;
			for (int i = 0; i < 30; i++)
			{
				this.weights[i] = Math.Pow(num, -1.0);
				num *= this.lacunarity;
			}
		}

		// Token: 0x06003BC0 RID: 15296 RVA: 0x0017B5A8 File Offset: 0x001797A8
		public override double GetValue(double x, double y, double z)
		{
			x *= this.frequency;
			y *= this.frequency;
			z *= this.frequency;
			double num = 0.0;
			double num2 = 1.0;
			double num3 = 1.0;
			double num4 = 2.0;
			for (int i = 0; i < this.octaveCount; i++)
			{
				double x2 = Utils.MakeInt32Range(x);
				double y2 = Utils.MakeInt32Range(y);
				double z2 = Utils.MakeInt32Range(z);
				long num5 = (long)(this.seed + i & int.MaxValue);
				double num6 = Utils.GradientCoherentNoise3D(x2, y2, z2, num5, this.quality);
				num6 = Math.Abs(num6);
				num6 = num3 - num6;
				num6 *= num6;
				num6 *= num2;
				num2 = num6 * num4;
				if (num2 > 1.0)
				{
					num2 = 1.0;
				}
				if (num2 < 0.0)
				{
					num2 = 0.0;
				}
				num += num6 * this.weights[i];
				x *= this.lacunarity;
				y *= this.lacunarity;
				z *= this.lacunarity;
			}
			return num * 1.25 - 1.0;
		}

		// Token: 0x04002701 RID: 9985
		private double frequency = 1.0;

		// Token: 0x04002702 RID: 9986
		private double lacunarity = 2.0;

		// Token: 0x04002703 RID: 9987
		private QualityMode quality = QualityMode.Medium;

		// Token: 0x04002704 RID: 9988
		private int octaveCount = 6;

		// Token: 0x04002705 RID: 9989
		private int seed;

		// Token: 0x04002706 RID: 9990
		private double[] weights = new double[30];
	}
}
