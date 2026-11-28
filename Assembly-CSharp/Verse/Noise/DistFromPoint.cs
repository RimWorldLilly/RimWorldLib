using System;

namespace Verse.Noise
{
	// Token: 0x0200075D RID: 1885
	public class DistFromPoint : ModuleBase
	{
		// Token: 0x06003B8E RID: 15246 RVA: 0x0017AEF2 File Offset: 0x001790F2
		public DistFromPoint() : base(0)
		{
		}

		// Token: 0x06003B8F RID: 15247 RVA: 0x0017AF06 File Offset: 0x00179106
		public DistFromPoint(float span) : base(0)
		{
			this.span = span;
		}

		// Token: 0x06003B90 RID: 15248 RVA: 0x0017AF21 File Offset: 0x00179121
		public override double GetValue(double x, double y, double z)
		{
			if (this.span <= 0f)
			{
				Log.ErrorOnce("Span must be greater than zero", 8745456);
				return 0.0;
			}
			return Math.Sqrt(x * x + z * z) / (double)this.span;
		}

		// Token: 0x040026F3 RID: 9971
		public float span = 1f;
	}
}
