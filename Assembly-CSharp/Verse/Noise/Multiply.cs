using System;

namespace Verse.Noise
{
	// Token: 0x02000777 RID: 1911
	public class Multiply : ModuleBase
	{
		// Token: 0x06003C36 RID: 15414 RVA: 0x0017D1BC File Offset: 0x0017B3BC
		public Multiply() : base(2)
		{
		}

		// Token: 0x06003C37 RID: 15415 RVA: 0x0017D1C5 File Offset: 0x0017B3C5
		public Multiply(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x06003C38 RID: 15416 RVA: 0x0017D1E0 File Offset: 0x0017B3E0
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x, y, z) * this.modules[1].GetValue(x, y, z);
		}
	}
}
