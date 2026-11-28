using System;

namespace Verse.Noise
{
	// Token: 0x0200075B RID: 1883
	public class DistFromAxis_Directional : ModuleBase
	{
		// Token: 0x06003B88 RID: 15240 RVA: 0x0017ADEF File Offset: 0x00178FEF
		public DistFromAxis_Directional() : base(0)
		{
		}

		// Token: 0x06003B89 RID: 15241 RVA: 0x0017ADF8 File Offset: 0x00178FF8
		public DistFromAxis_Directional(float span) : base(0)
		{
			this.span = span;
		}

		// Token: 0x06003B8A RID: 15242 RVA: 0x0017AE08 File Offset: 0x00179008
		public override double GetValue(double x, double y, double z)
		{
			return x / (double)this.span;
		}

		// Token: 0x040026F0 RID: 9968
		public float span;
	}
}
