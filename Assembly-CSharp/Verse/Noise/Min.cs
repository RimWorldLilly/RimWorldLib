using System;

namespace Verse.Noise
{
	// Token: 0x02000776 RID: 1910
	public class Min : ModuleBase
	{
		// Token: 0x06003C33 RID: 15411 RVA: 0x0017D164 File Offset: 0x0017B364
		public Min() : base(2)
		{
		}

		// Token: 0x06003C34 RID: 15412 RVA: 0x0017D16D File Offset: 0x0017B36D
		public Min(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x06003C35 RID: 15413 RVA: 0x0017D188 File Offset: 0x0017B388
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			double value2 = this.modules[1].GetValue(x, y, z);
			return Math.Min(value, value2);
		}
	}
}
