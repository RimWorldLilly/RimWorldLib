using System;

namespace Verse.Noise
{
	// Token: 0x02000775 RID: 1909
	public class Max : ModuleBase
	{
		// Token: 0x06003C30 RID: 15408 RVA: 0x0017D10C File Offset: 0x0017B30C
		public Max() : base(2)
		{
		}

		// Token: 0x06003C31 RID: 15409 RVA: 0x0017D115 File Offset: 0x0017B315
		public Max(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x06003C32 RID: 15410 RVA: 0x0017D130 File Offset: 0x0017B330
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			double value2 = this.modules[1].GetValue(x, y, z);
			return Math.Max(value, value2);
		}
	}
}
