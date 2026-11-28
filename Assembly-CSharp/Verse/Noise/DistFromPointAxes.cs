using System;

namespace Verse.Noise
{
	// Token: 0x0200075E RID: 1886
	public class DistFromPointAxes : ModuleBase
	{
		// Token: 0x06003B91 RID: 15249 RVA: 0x0017AF5D File Offset: 0x0017915D
		public DistFromPointAxes() : base(0)
		{
		}

		// Token: 0x06003B92 RID: 15250 RVA: 0x0017AF66 File Offset: 0x00179166
		public DistFromPointAxes(float spanX, float spanZ) : base(0)
		{
			this.spanX = spanX;
			this.spanZ = spanZ;
		}

		// Token: 0x06003B93 RID: 15251 RVA: 0x0017AF80 File Offset: 0x00179180
		public override double GetValue(double x, double y, double z)
		{
			double val = Math.Abs(x) / (double)this.spanX;
			double val2 = Math.Abs(z) / (double)this.spanZ;
			return Math.Max(val, val2);
		}

		// Token: 0x040026F4 RID: 9972
		public float spanX;

		// Token: 0x040026F5 RID: 9973
		public float spanZ;
	}
}
