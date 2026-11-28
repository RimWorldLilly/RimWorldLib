using System;

namespace Verse.Noise
{
	// Token: 0x0200076E RID: 1902
	public class Clamp : ModuleBase
	{
		// Token: 0x06003C08 RID: 15368 RVA: 0x0017CB10 File Offset: 0x0017AD10
		public Clamp() : base(1)
		{
		}

		// Token: 0x06003C09 RID: 15369 RVA: 0x0017CB37 File Offset: 0x0017AD37
		public Clamp(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06003C0A RID: 15370 RVA: 0x0017CB67 File Offset: 0x0017AD67
		public Clamp(double min, double max, ModuleBase input) : base(1)
		{
			this.Minimum = min;
			this.Maximum = max;
			this.modules[0] = input;
		}

		// Token: 0x17000BB4 RID: 2996
		// (get) Token: 0x06003C0B RID: 15371 RVA: 0x0017CBA5 File Offset: 0x0017ADA5
		// (set) Token: 0x06003C0C RID: 15372 RVA: 0x0017CBAD File Offset: 0x0017ADAD
		public double Maximum
		{
			get
			{
				return this.m_max;
			}
			set
			{
				this.m_max = value;
			}
		}

		// Token: 0x17000BB5 RID: 2997
		// (get) Token: 0x06003C0D RID: 15373 RVA: 0x0017CBB6 File Offset: 0x0017ADB6
		// (set) Token: 0x06003C0E RID: 15374 RVA: 0x0017CBBE File Offset: 0x0017ADBE
		public double Minimum
		{
			get
			{
				return this.m_min;
			}
			set
			{
				this.m_min = value;
			}
		}

		// Token: 0x06003C0F RID: 15375 RVA: 0x0017CBC7 File Offset: 0x0017ADC7
		public void SetBounds(double min, double max)
		{
			this.m_min = min;
			this.m_max = max;
		}

		// Token: 0x06003C10 RID: 15376 RVA: 0x0017CBD8 File Offset: 0x0017ADD8
		public override double GetValue(double x, double y, double z)
		{
			if (this.m_min > this.m_max)
			{
				double min = this.m_min;
				this.m_min = this.m_max;
				this.m_max = min;
			}
			double value = this.modules[0].GetValue(x, y, z);
			if (value < this.m_min)
			{
				return this.m_min;
			}
			if (value > this.m_max)
			{
				return this.m_max;
			}
			return value;
		}

		// Token: 0x04002731 RID: 10033
		private double m_min = -1.0;

		// Token: 0x04002732 RID: 10034
		private double m_max = 1.0;
	}
}
