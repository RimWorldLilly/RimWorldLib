using System;

namespace Verse.Noise
{
	// Token: 0x0200077F RID: 1919
	public class SmoothMin : ModuleBase
	{
		// Token: 0x06003C6C RID: 15468 RVA: 0x0017D9B1 File Offset: 0x0017BBB1
		public SmoothMin() : base(2)
		{
		}

		// Token: 0x06003C6D RID: 15469 RVA: 0x0017D9C9 File Offset: 0x0017BBC9
		public SmoothMin(ModuleBase lhs, ModuleBase rhs, double smoothness) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
			this.m_smoothness = smoothness;
		}

		// Token: 0x06003C6E RID: 15470 RVA: 0x0017D9FC File Offset: 0x0017BBFC
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			double value2 = this.modules[1].GetValue(x, y, z);
			return GenMath.SmoothMin(value, value2, this.m_smoothness);
		}

		// Token: 0x0400274E RID: 10062
		private double m_smoothness = 1.0;
	}
}
