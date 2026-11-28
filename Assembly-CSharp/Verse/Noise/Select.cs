using System;

namespace Verse.Noise
{
	// Token: 0x0200077E RID: 1918
	public class Select : ModuleBase
	{
		// Token: 0x06003C5F RID: 15455 RVA: 0x0017D6E4 File Offset: 0x0017B8E4
		public Select() : base(3)
		{
		}

		// Token: 0x06003C60 RID: 15456 RVA: 0x0017D70C File Offset: 0x0017B90C
		public Select(ModuleBase inputA, ModuleBase inputB, ModuleBase controller) : base(3)
		{
			this.modules[0] = inputA;
			this.modules[1] = inputB;
			this.modules[2] = controller;
		}

		// Token: 0x06003C61 RID: 15457 RVA: 0x0017D759 File Offset: 0x0017B959
		public Select(double min, double max, double fallOff, ModuleBase inputA, ModuleBase inputB) : this(inputA, inputB, null)
		{
			this.m_min = min;
			this.m_max = max;
			this.FallOff = fallOff;
		}

		// Token: 0x17000BC4 RID: 3012
		// (get) Token: 0x06003C62 RID: 15458 RVA: 0x0017D77B File Offset: 0x0017B97B
		// (set) Token: 0x06003C63 RID: 15459 RVA: 0x0017D785 File Offset: 0x0017B985
		public ModuleBase Controller
		{
			get
			{
				return this.modules[2];
			}
			set
			{
				this.modules[2] = value;
			}
		}

		// Token: 0x17000BC5 RID: 3013
		// (get) Token: 0x06003C64 RID: 15460 RVA: 0x0017D790 File Offset: 0x0017B990
		// (set) Token: 0x06003C65 RID: 15461 RVA: 0x0017D798 File Offset: 0x0017B998
		public double FallOff
		{
			get
			{
				return this.m_fallOff;
			}
			set
			{
				double num = this.m_max - this.m_min;
				this.m_raw = value;
				this.m_fallOff = ((value > num / 2.0) ? (num / 2.0) : value);
			}
		}

		// Token: 0x17000BC6 RID: 3014
		// (get) Token: 0x06003C66 RID: 15462 RVA: 0x0017D7DC File Offset: 0x0017B9DC
		// (set) Token: 0x06003C67 RID: 15463 RVA: 0x0017D7E4 File Offset: 0x0017B9E4
		public double Maximum
		{
			get
			{
				return this.m_max;
			}
			set
			{
				this.m_max = value;
				this.FallOff = this.m_raw;
			}
		}

		// Token: 0x17000BC7 RID: 3015
		// (get) Token: 0x06003C68 RID: 15464 RVA: 0x0017D7F9 File Offset: 0x0017B9F9
		// (set) Token: 0x06003C69 RID: 15465 RVA: 0x0017D801 File Offset: 0x0017BA01
		public double Minimum
		{
			get
			{
				return this.m_min;
			}
			set
			{
				this.m_min = value;
				this.FallOff = this.m_raw;
			}
		}

		// Token: 0x06003C6A RID: 15466 RVA: 0x0017D816 File Offset: 0x0017BA16
		public void SetBounds(double min, double max)
		{
			this.m_min = min;
			this.m_max = max;
			this.FallOff = this.m_fallOff;
		}

		// Token: 0x06003C6B RID: 15467 RVA: 0x0017D834 File Offset: 0x0017BA34
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[2].GetValue(x, y, z);
			if (this.m_fallOff > 0.0)
			{
				if (value < this.m_min - this.m_fallOff)
				{
					return this.modules[0].GetValue(x, y, z);
				}
				if (value < this.m_min + this.m_fallOff)
				{
					double num = this.m_min - this.m_fallOff;
					double num2 = this.m_min + this.m_fallOff;
					double position = Utils.MapCubicSCurve((value - num) / (num2 - num));
					return Utils.InterpolateLinear(this.modules[0].GetValue(x, y, z), this.modules[1].GetValue(x, y, z), position);
				}
				if (value < this.m_max - this.m_fallOff)
				{
					return this.modules[1].GetValue(x, y, z);
				}
				if (value < this.m_max + this.m_fallOff)
				{
					double num3 = this.m_max - this.m_fallOff;
					double num4 = this.m_max + this.m_fallOff;
					double position = Utils.MapCubicSCurve((value - num3) / (num4 - num3));
					return Utils.InterpolateLinear(this.modules[1].GetValue(x, y, z), this.modules[0].GetValue(x, y, z), position);
				}
				return this.modules[0].GetValue(x, y, z);
			}
			else
			{
				if (value < this.m_min || value > this.m_max)
				{
					return this.modules[0].GetValue(x, y, z);
				}
				return this.modules[1].GetValue(x, y, z);
			}
		}

		// Token: 0x0400274A RID: 10058
		private double m_fallOff;

		// Token: 0x0400274B RID: 10059
		private double m_raw;

		// Token: 0x0400274C RID: 10060
		private double m_min = -1.0;

		// Token: 0x0400274D RID: 10061
		private double m_max = 1.0;
	}
}
