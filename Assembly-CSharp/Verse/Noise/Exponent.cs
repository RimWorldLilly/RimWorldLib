using System;

namespace Verse.Noise
{
	// Token: 0x02000771 RID: 1905
	public class Exponent : ModuleBase
	{
		// Token: 0x06003C21 RID: 15393 RVA: 0x0017CF1A File Offset: 0x0017B11A
		public Exponent() : base(1)
		{
		}

		// Token: 0x06003C22 RID: 15394 RVA: 0x0017CF32 File Offset: 0x0017B132
		public Exponent(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06003C23 RID: 15395 RVA: 0x0017CF53 File Offset: 0x0017B153
		public Exponent(double exponent, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.Value = exponent;
		}

		// Token: 0x17000BBB RID: 3003
		// (get) Token: 0x06003C24 RID: 15396 RVA: 0x0017CF7B File Offset: 0x0017B17B
		// (set) Token: 0x06003C25 RID: 15397 RVA: 0x0017CF83 File Offset: 0x0017B183
		public double Value
		{
			get
			{
				return this.m_exponent;
			}
			set
			{
				this.m_exponent = value;
			}
		}

		// Token: 0x06003C26 RID: 15398 RVA: 0x0017CF8C File Offset: 0x0017B18C
		public override double GetValue(double x, double y, double z)
		{
			return Math.Pow(Math.Abs((this.modules[0].GetValue(x, y, z) + 1.0) / 2.0), this.m_exponent) * 2.0 - 1.0;
		}

		// Token: 0x04002734 RID: 10036
		private double m_exponent = 1.0;
	}
}
