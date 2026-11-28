using System;

namespace Verse.Noise
{
	// Token: 0x02000780 RID: 1920
	public class Subtract : ModuleBase
	{
		// Token: 0x06003C6F RID: 15471 RVA: 0x0017DA36 File Offset: 0x0017BC36
		public Subtract() : base(2)
		{
		}

		// Token: 0x06003C70 RID: 15472 RVA: 0x0017DA3F File Offset: 0x0017BC3F
		public Subtract(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x06003C71 RID: 15473 RVA: 0x0017DA5A File Offset: 0x0017BC5A
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x, y, z) - this.modules[1].GetValue(x, y, z);
		}
	}
}
