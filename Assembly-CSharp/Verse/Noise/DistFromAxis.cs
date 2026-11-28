using System;

namespace Verse.Noise
{
	// Token: 0x0200075A RID: 1882
	public class DistFromAxis : ModuleBase
	{
		// Token: 0x06003B85 RID: 15237 RVA: 0x0017ADC6 File Offset: 0x00178FC6
		public DistFromAxis() : base(0)
		{
		}

		// Token: 0x06003B86 RID: 15238 RVA: 0x0017ADCF File Offset: 0x00178FCF
		public DistFromAxis(float span) : base(0)
		{
			this.span = span;
		}

		// Token: 0x06003B87 RID: 15239 RVA: 0x0017ADDF File Offset: 0x00178FDF
		public override double GetValue(double x, double y, double z)
		{
			return Math.Abs(x) / (double)this.span;
		}

		// Token: 0x040026EF RID: 9967
		public float span;
	}
}
