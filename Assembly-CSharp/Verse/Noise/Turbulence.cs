using System;

namespace Verse.Noise
{
	// Token: 0x02000783 RID: 1923
	public class Turbulence : ModuleBase
	{
		// Token: 0x06003C87 RID: 15495 RVA: 0x0017DDBE File Offset: 0x0017BFBE
		public Turbulence() : base(1)
		{
			this.m_xDistort = new Perlin();
			this.m_yDistort = new Perlin();
			this.m_zDistort = new Perlin();
		}

		// Token: 0x06003C88 RID: 15496 RVA: 0x0017DDF8 File Offset: 0x0017BFF8
		public Turbulence(ModuleBase input) : base(1)
		{
			this.m_xDistort = new Perlin();
			this.m_yDistort = new Perlin();
			this.m_zDistort = new Perlin();
			this.modules[0] = input;
		}

		// Token: 0x06003C89 RID: 15497 RVA: 0x0017DE45 File Offset: 0x0017C045
		public Turbulence(double power, ModuleBase input) : this(new Perlin(), new Perlin(), new Perlin(), power, input)
		{
		}

		// Token: 0x06003C8A RID: 15498 RVA: 0x0017DE5E File Offset: 0x0017C05E
		public Turbulence(Perlin x, Perlin y, Perlin z, double power, ModuleBase input) : base(1)
		{
			this.m_xDistort = x;
			this.m_yDistort = y;
			this.m_zDistort = z;
			this.modules[0] = input;
			this.Power = power;
		}

		// Token: 0x17000BCE RID: 3022
		// (get) Token: 0x06003C8B RID: 15499 RVA: 0x0017DE9D File Offset: 0x0017C09D
		// (set) Token: 0x06003C8C RID: 15500 RVA: 0x0017DEAA File Offset: 0x0017C0AA
		public double Frequency
		{
			get
			{
				return this.m_xDistort.Frequency;
			}
			set
			{
				this.m_xDistort.Frequency = value;
				this.m_yDistort.Frequency = value;
				this.m_zDistort.Frequency = value;
			}
		}

		// Token: 0x17000BCF RID: 3023
		// (get) Token: 0x06003C8D RID: 15501 RVA: 0x0017DED0 File Offset: 0x0017C0D0
		// (set) Token: 0x06003C8E RID: 15502 RVA: 0x0017DED8 File Offset: 0x0017C0D8
		public double Power
		{
			get
			{
				return this.m_power;
			}
			set
			{
				this.m_power = value;
			}
		}

		// Token: 0x17000BD0 RID: 3024
		// (get) Token: 0x06003C8F RID: 15503 RVA: 0x0017DEE1 File Offset: 0x0017C0E1
		// (set) Token: 0x06003C90 RID: 15504 RVA: 0x0017DEEE File Offset: 0x0017C0EE
		public int Roughness
		{
			get
			{
				return this.m_xDistort.OctaveCount;
			}
			set
			{
				this.m_xDistort.OctaveCount = value;
				this.m_yDistort.OctaveCount = value;
				this.m_zDistort.OctaveCount = value;
			}
		}

		// Token: 0x17000BD1 RID: 3025
		// (get) Token: 0x06003C91 RID: 15505 RVA: 0x0017DF14 File Offset: 0x0017C114
		// (set) Token: 0x06003C92 RID: 15506 RVA: 0x0017DF21 File Offset: 0x0017C121
		public int Seed
		{
			get
			{
				return this.m_xDistort.Seed;
			}
			set
			{
				this.m_xDistort.Seed = value;
				this.m_yDistort.Seed = value + 1;
				this.m_zDistort.Seed = value + 2;
			}
		}

		// Token: 0x06003C93 RID: 15507 RVA: 0x0017DF4C File Offset: 0x0017C14C
		public override double GetValue(double x, double y, double z)
		{
			double x2 = x + this.m_xDistort.GetValue(x + 0.189422607421875, y + 0.99371337890625, z + 0.4781646728515625) * this.m_power;
			double y2 = y + this.m_yDistort.GetValue(x + 0.4046478271484375, y + 0.276611328125, z + 0.9230499267578125) * this.m_power;
			double z2 = z + this.m_zDistort.GetValue(x + 0.82122802734375, y + 0.1710968017578125, z + 0.6842803955078125) * this.m_power;
			return this.modules[0].GetValue(x2, y2, z2);
		}

		// Token: 0x04002754 RID: 10068
		private const double X0 = 0.189422607421875;

		// Token: 0x04002755 RID: 10069
		private const double Y0 = 0.99371337890625;

		// Token: 0x04002756 RID: 10070
		private const double Z0 = 0.4781646728515625;

		// Token: 0x04002757 RID: 10071
		private const double X1 = 0.4046478271484375;

		// Token: 0x04002758 RID: 10072
		private const double Y1 = 0.276611328125;

		// Token: 0x04002759 RID: 10073
		private const double Z1 = 0.9230499267578125;

		// Token: 0x0400275A RID: 10074
		private const double X2 = 0.82122802734375;

		// Token: 0x0400275B RID: 10075
		private const double Y2 = 0.1710968017578125;

		// Token: 0x0400275C RID: 10076
		private const double Z2 = 0.6842803955078125;

		// Token: 0x0400275D RID: 10077
		private double m_power = 1.0;

		// Token: 0x0400275E RID: 10078
		private Perlin m_xDistort;

		// Token: 0x0400275F RID: 10079
		private Perlin m_yDistort;

		// Token: 0x04002760 RID: 10080
		private Perlin m_zDistort;
	}
}
